using Application_A.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_A.BL.Interfaces
{
    public interface IRabbitMqService
    {
        public Task PublshPersonRabbitAsync(Person p);
    }
}
