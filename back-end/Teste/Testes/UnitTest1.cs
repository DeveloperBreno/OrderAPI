using Aplicacao.Aplicacoes;
using Aplicacao.Interfaces;
using Dominio.Interfaces;
using Dominio.Interfaces.Filas;
using Dominio.Interfaces.Genericos;
using Dominio.Interfaces.InterfaceServicos;
using Dominio.Servicos;
using Insfraestrutura.Filas;
using Insfraestrutura.Repositorio;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RabbitMQ.Client;
using System;
using System.Threading.Tasks;

namespace Testes
{
    // Configuração de injeção de dependência para os testes
    public static class TestDependencyInjection
    {
        public static IServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();

            // Serviço de notícia
            services.AddScoped<IServicoNoticia, ServicoNoticia>();
            services.AddScoped<INoticia, RepositorioNoticia>();

            // Configuração RabbitMQ
            services.AddSingleton<IConnection>(sp =>
            {
                var factory = new ConnectionFactory
                {
                    HostName = "localhost", // substitua pelo hostname do RabbitMQ
                    UserName = "guest",     // substitua pelo username do RabbitMQ
                    Password = "guest"      // substitua pelo password do RabbitMQ
                };
                return factory.CreateConnection();
            });

            services.AddScoped<IInsereNaFila, InserirNaFila>();

            // Interface aplicação de usuário
            services.AddScoped<IAplicacaoUsuario, AplicacaoUsuario>();
            services.AddScoped<IUsuario, RepositorioUsuario>();
            services.AddScoped(typeof(IUsuario), typeof(RepositorioUsuario));

            services.AddScoped<IAplicacaoNoticia, AplicacaoNoticia>();
            services.AddScoped<IAplicacaoUsuario, AplicacaoUsuario>();

            return services.BuildServiceProvider();

        }
    }

    // Testes unitários usando MSTest
    [TestClass]
    public class ServicoNoticiaTests
    {
        private readonly IServicoNoticia _servicoNoticia;
        private readonly IAplicacaoUsuario _aplicacaoUsuario;
        private readonly IUsuario _usuario;

        public ServicoNoticiaTests()
        {
            var serviceProvider = TestDependencyInjection.BuildServiceProvider();
            _servicoNoticia = serviceProvider.GetRequiredService<IServicoNoticia>();
            _aplicacaoUsuario = serviceProvider.GetRequiredService<IAplicacaoUsuario>();
            _usuario = serviceProvider.GetRequiredService<IUsuario>();
        }

        [TestMethod]
        public void CriaUsuarioAsync()
        {
            var result = _aplicacaoUsuario.AdicionarUsuario("userteste@gmail.com", "123456", new DateTime(1997, 3, 20), "11977300114", "Teste").Result;

            // Assert
            Assert.IsTrue(result);
        }
    }
}
