using Application_A.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application_A.DL
{
    public interface IPersonRepo
    {
        Task Add(Person p);
        Task<IEnumerable<Person>> GetAllByDate(DateTime lastUpdated);
    }
}
