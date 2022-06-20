using Application_B.BL.DataFlow;
using Application_B.Models;
using MessagePack;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application_B.BL
{
    public class RabbitMqConsumer : IHostedService, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IPersonDataFlow _personDataFlow;

        
        public RabbitMqConsumer(IOptions<RabbitMqConfig> rOptions, IPersonDataFlow personDataFlow)
        {

            var factory = new ConnectionFactory()
            {
                HostName = rOptions.Value.Host,
                Port = rOptions.Value.Port
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare("Test", ExchangeType.Fanout);
            _personDataFlow = personDataFlow;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Factory.StartNew(() =>
            {

                _channel.QueueDeclare("person queue", true, false, autoDelete: false);

                var consumer = new EventingBasicConsumer(_channel);

                consumer.Received += (sender, ea) =>
                {
                   
                    _personDataFlow.SendPerson(ea.Body.ToArray());
                };
                _channel.BasicConsume(queue: "person queue", autoAck: true, consumer: consumer);
            }, cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
