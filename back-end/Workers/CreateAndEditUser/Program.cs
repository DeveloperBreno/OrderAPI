using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.DependencyInjection;
using Aplicacao.Interfaces;
using Microsoft.Extensions.Hosting;
using Aplicacao.Aplicacoes;
using Microsoft.EntityFrameworkCore;
using Insfraestrutura.Configuracoes;
using Entidades.Entidades;
using Newtonsoft.Json;
using Dominio.Interfaces;
using Dominio.Interfaces.Genericos;
using Dominio.Interfaces.InterfaceServicos;
using Dominio.Servicos;
using Insfraestrutura.Repositorio.Genericos;
using Insfraestrutura.Repositorio;


class Program
{
    static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var serviceScope = host.Services.CreateScope())
        {
            var services = serviceScope.ServiceProvider;

            var aplicacaoUsuario = services.GetRequiredService<IAplicacaoUsuario>();

            // var aplicacaoUsuario = services.AddScoped<IAplicacaoUsuario, AplicacaoUsuario>();

            StartConsumer(aplicacaoUsuario);
        }
        Console.WriteLine(" Pressione [enter] para sair.");
        Console.ReadLine();

    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            // Recupera a string de conexão do appsettings.json
            var connectionString = "Host=localhost;Port=5432;Database=MyDatabase;Username=postgres;Password=SuaSenha"; // Substitua pela sua string de conexão

            // Adiciona o DbContext ao contêiner de serviços
            services.AddDbContext<Contexto>(options =>
                options.UseNpgsql(connectionString));

            // Registra as interfaces e implementações necessárias
            services.AddScoped<IAplicacaoUsuario, AplicacaoUsuario>();

            // Interface e repositório
            services.AddScoped(typeof(IGenericos<>), typeof(RepositorioGenerico<>));
            services.AddScoped(typeof(INoticia), typeof(RepositorioNoticia));
            services.AddScoped(typeof(IUsuario), typeof(RepositorioUsuario));

            // Serviço domínio
            services.AddScoped<IServicoNoticia, ServicoNoticia>();

            // Interface aplicação
            services.AddScoped<IAplicacaoNoticia, AplicacaoNoticia>();
            services.AddScoped<IAplicacaoUsuario, AplicacaoUsuario>();


        });

    private static void StartConsumer(IAplicacaoUsuario aplicacaoUsuario)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        var queueName = "InsertApplicationUser";
        channel.QueueDeclare(queue: queueName,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        Console.WriteLine(" [*] Aguardando mensagens na fila {0}.", queueName);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            ApplicationUser applicationUser = JsonConvert.DeserializeObject<ApplicationUser>(message);

            try
            {
                ProcessarItem(applicationUser, aplicacaoUsuario);
                channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(" [!] Erro ao processar item: {0}", ex.Message);
                channel.BasicNack(ea.DeliveryTag, false, false);
                RepublisarMensagem(channel, queueName, message);
            }
        };

        channel.BasicConsume(queue: queueName,
                             autoAck: false,
                             consumer: consumer);
    }

    private static void ProcessarItem(ApplicationUser applicationUser, IAplicacaoUsuario aplicacaoUsuario)
    {
        // Use o serviço de aplicação de usuário
        aplicacaoUsuario.AdicionarUsuario(applicationUser.Email, applicationUser.PasswordHash, applicationUser.DataDeNascimento, applicationUser.Celular);

    }

    private static void RepublisarMensagem(IModel channel, string queueName, string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        Console.WriteLine(" [x] Mensagem republicada: {0}", message);
    }
}
