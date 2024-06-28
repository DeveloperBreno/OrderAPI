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

class Program
{
    static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var serviceScope = host.Services.CreateScope())
        {
            var services = serviceScope.ServiceProvider;
            var aplicacaoUsuario = services.GetRequiredService<IAplicacaoUsuario>();
            var configuration = services.GetRequiredService<IConfiguration>();

            await StartConsumerAsync(aplicacaoUsuario, configuration);
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

                services.AddScoped<IAplicacaoUsuario, AplicacaoUsuario>();
                services.AddScoped<IUsuario, RepositorioUsuario>();
            });

    static async Task StartConsumerAsync(IAplicacaoUsuario aplicacaoUsuario, IConfiguration configuration)
    {
        var rabbitMQConfig = configuration.GetSection("RabbitMQ").Get<RabbitMQConfig>();

        var factory = new ConnectionFactory()
        {
            HostName = rabbitMQConfig.HostName,
            UserName = rabbitMQConfig.UserName,
            Password = rabbitMQConfig.Password
        };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            var queueName = rabbitMQConfig.QueueName;
            channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            Console.WriteLine(" [*] Aguardando mensagens na fila {0}.", queueName);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                channel.BasicAck(ea.DeliveryTag, false);
                try
                {
                    var applicationUser = JsonConvert.DeserializeObject<ApplicationUser>(message);
                    await ProcessarItemAsync(applicationUser, aplicacaoUsuario);
                }
                catch (Exception ex)
                {
                    RepublisarMensagem(channel, queueName, message);
                }
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: false,
                                 consumer: consumer);

            await Task.Delay(Timeout.Infinite); // Mantém o programa em execução para continuar consumindo mensagens
        }
    }

    static async Task ProcessarItemAsync(ApplicationUser applicationUser, IAplicacaoUsuario aplicacaoUsuario)
    {
        await aplicacaoUsuario.AdicionarUsuario(applicationUser.Email, applicationUser.PasswordHash, applicationUser.DataDeNascimento, applicationUser.Celular, applicationUser.NormalizedUserName);
    }

    static void RepublisarMensagem(IModel channel, string queueName, string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
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
    public string UserName { get; set; }
    public string Password { get; set; }
    public string QueueName { get; set; }
}
