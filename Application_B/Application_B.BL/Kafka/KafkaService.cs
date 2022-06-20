using Application_B.BL.DataFlow;
using Application_B.Models;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application_B.BL.Kafka
{
    public class KafkaService : IKafkaService//, IHostedService
    {
        private static IProducer<int, Person> _producer;
        private Timer _timer;
        public KafkaService(IOptions<KafkaConfig> kOptions)
        {
            var config = new ProducerConfig()
            {
                BootstrapServers = kOptions.Value.BootstrapServers
            };

            _producer = new ProducerBuilder<int, Person>(config)
                .SetValueSerializer(new MsgPackSerializer<Person>())
                .Build();

            
        }
        public async Task SendPersonKafka(Person p)
        {
           await Task.Factory.StartNew(async () =>
            {
                Console.WriteLine($"Send Obj id:{p.Id}, Name:{p.Name}, Date:{p.LastUpdated}");
                await _producer.ProduceAsync("Test", new Message<int, Person>()
                {
                    Key = p.Id,
                    Value = p
                });               
            });  
        }
    }
}
