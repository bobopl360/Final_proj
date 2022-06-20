using Application_A.BL.Kafka;
using Application_A.Models;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application_A.BL.Serializers
{
    public class KafkaConsumer : IHostedService
    {
        private static IConsumer<int, Person> _consumer;
        public KafkaConsumer()
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost",
                GroupId = $"AppName:{Guid.NewGuid().ToString()}",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true,
                ClientId = "2"

            };
            _consumer = new ConsumerBuilder<int, Person>(config)
                .SetValueDeserializer(new MsgPackDeserializer<Person>())
                .Build();


        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _consumer.Subscribe("Test");
            Task.Factory.StartNew(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var cr = _consumer.Consume(cancellationToken);
                        Console.WriteLine($"Consumed: {cr.Message.Value} At: {cr.TopicPartitionOffset}");
                        Console.WriteLine($"Consumed object have: id:{cr.Message.Value.id} Name: {cr.Message.Value.Name}, Date: {cr.Message.Value.LastUpdated}");
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error: {e.Error.Reason}");
                    }
                }


            }, cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
