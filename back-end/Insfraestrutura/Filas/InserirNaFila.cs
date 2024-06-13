using Dominio.Interfaces.Filas;
using RabbitMQ.Client;
using System.Text;

namespace Insfraestrutura.Filas
{
    public class InserirNaFila : IInsereNaFila
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public InserirNaFila(IConnection connection)
        {
            _connection = connection;
            _channel = _connection.CreateModel();
        }

        public void Inserir(object obj, string nomeDaFila)
        {
            string fila = nomeDaFila.ToString();

            // Converta o objeto para uma mensagem apropriada
            var messageBody = Encoding.UTF8.GetBytes(obj.ToString());

            _channel.QueueDeclare(
                    queue: fila,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

            _channel.BasicPublish(exchange: "", routingKey: fila, basicProperties: null, body: messageBody);
        }
    }
}
