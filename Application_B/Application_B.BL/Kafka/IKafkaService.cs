using Application_B.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_B.BL.Kafka
{
    public interface IKafkaService
    {
        public Task SendPersonKafka(Person p);
    }
}
