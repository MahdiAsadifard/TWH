namespace Database.Redis
{
    public interface IRedisProvider
    {
        StackExchange.Redis.IDatabase Build();
    }
}