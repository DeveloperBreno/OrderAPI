using Dominio.Interfaces;
using Dominio.Interfaces.Filas;
using Dominio.Interfaces.Genericos;
using Dominio.Interfaces.InterfaceServicos;
using Dominio.Servicos;
using Insfraestrutura.Filas;
using Insfraestrutura.Repositorio;
using Insfraestrutura.Repositorio.Genericos;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Aplicacao.Interfaces;
using Aplicacao.Aplicacoes;

namespace Testes;

// TestDependencyInjection.cs
public static class TestDependencyInjection
{
    public static IServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();

        // Registre as dependências necessárias
        services.AddScoped<IServicoNoticia, ServicoNoticia>();
        services.AddScoped<INoticia, RepositorioNoticia>();

        //// Adiciona os serviços de Identity
        //services.AddIdentity<ApplicationUser, IdentityRole>()
        //    .AddEntityFrameworkStores<Contexto>()
        //    .AddDefaultTokenProviders();

        // Interface e repositório
        services.AddScoped(typeof(IGenericos<>), typeof(RepositorioGenerico<>));
        services.AddScoped(typeof(INoticia), typeof(RepositorioNoticia));
        services.AddScoped(typeof(IUsuario), typeof(RepositorioUsuario));

        // Serviço domínio
        services.AddScoped<IServicoNoticia, ServicoNoticia>();

        // Interface aplicação
        services.AddScoped<IAplicacaoNoticia, AplicacaoNoticia>();
        services.AddScoped<IAplicacaoUsuario, AplicacaoUsuario>();

        // Configurar RabbitMQ
        services.AddSingleton<IConnection>(sp =>
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost", // substitua pelo hostname do RabbitMQ
                UserName = "guest", // substitua pelo username do RabbitMQ
                Password = "guest" // substitua pelo password do RabbitMQ
            };
            return factory.CreateConnection();
        });

        services.AddScoped<IInsereNaFila, InserirNaFila>();

        return services.BuildServiceProvider();
    }
}


[TestClass]
public class ServicoNoticiaTests
{
    private readonly IServicoNoticia _servicoNoticia;
    private readonly IAplicacaoUsuario _aplicacaoUsuario;

    public ServicoNoticiaTests()
    {
        var serviceProvider = TestDependencyInjection.BuildServiceProvider();
        _servicoNoticia = serviceProvider.GetRequiredService<IServicoNoticia>();
        _aplicacaoUsuario = serviceProvider.GetRequiredService<IAplicacaoUsuario>();
    }

    [TestMethod]
    public async void ObterNoticias_DeveRetornarTodasAsNoticias()
    {
        var noticias = await _servicoNoticia.ListarNoticiasAtivas();

        // Assert
        Assert.IsNotNull(noticias);
    }

    [TestMethod]
    public async void ObterUsuario()
    {
        var user = await _aplicacaoUsuario.RetornaIdUsuario("developerbreno@gmail.com");

        // Assert
        Assert.IsNotNull(user);
    }
}
