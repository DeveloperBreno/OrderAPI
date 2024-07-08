using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Entidades.Entidades;
using Aplicacao.Interfaces;
using Insfraestrutura.Configuracoes;
using Dominio.Interfaces;
using Insfraestrutura.Repositorio;
using Aplicacao.Aplicacoes;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
class Program
{

    private readonly UserManager<ApplicationUser> _userManager;

    public Program(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var serviceScope = host.Services.CreateScope())
        {
            var services = serviceScope.ServiceProvider;
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            var program = new Program(userManager);

            await program.StartConsumerAsync(services.GetRequiredService<IConfiguration>());
        }

        Console.WriteLine("Pressione [enter] para sair.");
        Console.ReadLine();
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((hostContext, services) =>
            {
                var configuration = hostContext.Configuration;
                var connectionString = configuration.GetConnectionString("DefaultConnection");

                services.AddDbContext<Contexto>(options =>
                    options.UseNpgsql(connectionString));

                // Adiciona os serviços de Identity
                services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<Contexto>()
                    .AddDefaultTokenProviders();
                services.AddScoped<IUsuario, RepositorioUsuario>();
            });

    public async Task StartConsumerAsync(IConfiguration configuration)
    {
        var rabbitMQConfig = configuration.GetSection("RabbitMQ").Get<RabbitMQConfig>();

        var factory = new ConnectionFactory()
        {
            HostName = rabbitMQConfig.HostName,
            Port = rabbitMQConfig.Port,
            UserName = rabbitMQConfig.UserName,
            Password = rabbitMQConfig.Password
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        var queueName = rabbitMQConfig.QueueName;
        channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        // Define a quantidade de mensagens que cada worker pode processar por vez
        channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

        Console.WriteLine(" [*] Aguardando mensagens na fila {0}.", queueName);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            try
            {
                var applicationUser = JsonConvert.DeserializeObject<ApplicationUser>(message);
                // simula que o usuario fez a confirmação do codigo via e-mail, para produção devemos enviar o email de fato
                applicationUser.EmailConfirmed = true;
                var resultado = await _userManager.CreateAsync(applicationUser, applicationUser.PasswordHash);

                if (resultado.Errors.Any())
                {
                    throw new Exception(resultado.Errors.ToString());
                }
            }
            catch (Exception ex)
            {
                // Republica a mensagem em caso de falha
                RepublisarMensagem(channel, queueName, message);
            }

            // Confirma que a mensagem foi processada
            channel.BasicAck(ea.DeliveryTag, false);

        };

        channel.BasicConsume(queue: queueName,
                             autoAck: false,
                             consumer: consumer);

        await Task.Delay(Timeout.Infinite); // Mantém o programa em execução para continuar consumindo mensagens
    }

    static async Task ProcessarItemAsync(ApplicationUser applicationUser, UserManager<ApplicationUser> userManager)
    {


        await Task.CompletedTask;
    }

    static void RepublisarMensagem(IModel channel, string queueName, string message)
    {
        queueName = $"FilaDeErro{queueName}";
        var body = Encoding.UTF8.GetBytes(message);

        channel.QueueDeclare(queue: queueName,
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

        channel.BasicPublish(exchange: "",
                             routingKey: queueName,
                             basicProperties: null,
                             body: body);

        Console.WriteLine(" [x] Mensagem republicada: {0}", message);
    }
}

public class RabbitMQConfig
{
    public string HostName { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string QueueName { get; set; }
}
