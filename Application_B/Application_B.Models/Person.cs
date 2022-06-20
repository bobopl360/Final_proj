using MessagePack;
using System;

namespace Application_B.Models
{
    [MessagePackObject]
    public class Person
        {
            [Key(0)]
            public int Id { get; set; }
            [Key(1)]
            public string Name { get; set; }
            [Key(2)]
            public DateTime LastUpdated { get; set; }
    }
}
