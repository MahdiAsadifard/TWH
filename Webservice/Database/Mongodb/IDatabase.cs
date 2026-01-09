using MongoDB.Driver;

namespace Database.Mongodb
{
    public interface IDatabase<T>
    {
        IMongoCollection<T> GetCollection(string name);
    }
}
