using Application_A.BL.Interfaces;
using Application_A.Models;
using MessagePack;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Threading.Tasks;
using IConnection = RabbitMQ.Client.IConnection;

namespace Application_A.BL.Services
{
    public class RabbitPublisher : IRabbitMqService, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitPublisher(IOptions<RabbitMqConfig> rOptions)
        {
            var factory = new ConnectionFactory()
            {
                HostName = rOptions.Value.Host,
                Port = rOptions.Value.Port
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare("Test", ExchangeType.Fanout);

            _channel.QueueDeclare("person queue", true, false, autoDelete: false);
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }


        public async Task PublshPersonRabbitAsync(Person p)
        {
            await Task.Factory.StartNew(() =>
            {
                byte[] bytes = MessagePackSerializer.Serialize(p);
                _channel.BasicPublish(exchange: "", routingKey: "person queue", body: bytes);
            });
        }
    }
    
}
