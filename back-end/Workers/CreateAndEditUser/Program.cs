using System;
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
using System.Threading.Tasks;
using Aplicacao.Aplicacoes;

class Program
{
    static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var serviceScope = host.Services.CreateScope())
        {
            var services = serviceScope.ServiceProvider;
            var aplicacaoUsuario = services.GetRequiredService<IAplicacaoUsuario>();

            await StartConsumerAsync(aplicacaoUsuario);
        }

        Console.WriteLine(" Pressione [enter] para sair.");
        Console.ReadLine();
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                var connectionString = "Host=localhost;Port=5432;Database=MyDatabase;Username=postgres;Password=SuaSenha"; // Substitua pela sua string de conexão

                services.AddDbContext<Contexto>(options =>
                    options.UseNpgsql(connectionString));

                services.AddScoped<IAplicacaoUsuario, AplicacaoUsuario>();
                services.AddScoped<IUsuario, RepositorioUsuario>(); // Certifique-se de registrar o repositório também

                // Outros serviços necessários
            });

    static async Task StartConsumerAsync(IAplicacaoUsuario aplicacaoUsuario)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            var queueName = "InsertApplicationUser";
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
