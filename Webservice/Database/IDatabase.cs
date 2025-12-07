using MongoDB.Driver;

namespace Database
{
    public interface IDatabase<T>
    {
        IMongoCollection<T> GetCollection(string name);
    }
}
