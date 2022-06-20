using Application_A.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_A.BL.Serializers
{
    public interface IKafkaConsumer
    {
        public Task PublshPersonAsync(Person p);
    }
}
