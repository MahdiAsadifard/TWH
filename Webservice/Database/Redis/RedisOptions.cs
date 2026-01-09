namespace Database.Redis
{
    public record RedisOptions
    {
        public const string OptionName = "Redis";

        public required string Host { get; set; }
        public int Port { get; set; }
    }
}
