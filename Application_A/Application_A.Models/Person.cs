using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application_A.Models
{
    [MessagePackObject]
    public class Person
    {
        [Key(0)]
        public int id { get; set; }
        [Key(1)]
        public string Name { get; set; }
        [Key(2)]
        public DateTime LastUpdated { get; set; }
    }
}
