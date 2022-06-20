using Application_A.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_A.DL
{
    public class PersonRepo : IPersonRepo
    {
        private readonly IMongoCollection<Person> _collection;
        public PersonRepo(IOptions<MongoDbConfiguration> mOptions)
        {
            var client = new MongoClient(mOptions.Value.ConnectionString);
            var database = client.GetDatabase(mOptions.Value.DatabaseName);

            _collection = database.GetCollection<Person>("Person");
        }
        public async Task Add(Person p)
        {
            await _collection.InsertOneAsync(p);

        }

        public async Task<IEnumerable<Person>> GetAllByDate(DateTime lastUpdated)
        {
            var result = await _collection.FindAsync(p => p.LastUpdated >= lastUpdated);

            return result.ToList();
        }
    }
}
