using Dominio.Interfaces.Filas;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;

namespace Insfraestrutura.Filas;

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
        var messageBody = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));

        _channel.QueueDeclare(
                queue: nomeDaFila,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

        _channel.BasicPublish(exchange: "", routingKey: nomeDaFila, basicProperties: null, body: messageBody);
    }
}
