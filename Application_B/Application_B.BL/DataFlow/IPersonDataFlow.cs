using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_B.BL.DataFlow
{
    public interface IPersonDataFlow
    {
        public Task SendPerson(byte[] data);
    }
}
