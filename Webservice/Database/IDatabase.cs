using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public interface IDatabase<T>
    {
        IMongoCollection<T> GetCollection(string name);
    }
}
