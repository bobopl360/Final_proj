using Application_B.BL.Kafka;
using Application_B.Models;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Application_B.BL.DataFlow
{
        public class PersonDataFlow : IPersonDataFlow
        {

        private readonly IKafkaService _kafkaService;
            TransformBlock<byte[], Person> entryBlock;
            public PersonDataFlow(IKafkaService kafkaService)
            {
            _kafkaService = kafkaService;

            entryBlock = new TransformBlock<byte[], Person>(data => MessagePackSerializer.Deserialize<Person>(data));

                var enrichBlock = new TransformBlock<Person, Person>(p =>
                {
                    Random rd = new Random();

                    p.Id = rd.Next(0, 150);

                    return p;

                });

                var publishBlock = new ActionBlock<Person>(p =>
                {
                    Console.WriteLine($"Updated date value:{p.Id}");
                    _kafkaService.SendPersonKafka(p);
                });

                var linkOptions = new DataflowLinkOptions()
                {
                    PropagateCompletion = true
                };


                entryBlock.LinkTo(enrichBlock, linkOptions);
                enrichBlock.LinkTo(publishBlock, linkOptions);
            }
            public async Task SendPerson(byte[] data)
            {
                var obj = MessagePackSerializer.Deserialize<Person>(data);
                Console.WriteLine($"Consumed obj: id:{obj.Id}, Name: {obj.Name}, Date: {obj.LastUpdated}");
                await entryBlock.SendAsync(data);
            }

        }
}
