using NRedisStack;

namespace Database.Redis
{
    public interface IRedisCommandsBuilder
    {
        Task<bool> SetAddAsync(string key, string value);
        Task<string> GetValue(string key);
        // -----------
        Task<BloomCommands> GetBloomCommands();
        Task<CmsCommands> GetCmsCommands();
        Task<CuckooCommands> GetCuckooCommands();
        Task<JsonCommands> GetJsonCommands();
        Task<SearchCommands> GetSearchCommands();
        Task<TdigestCommands> GetTdigestCommands();
        Task<TimeSeriesCommands> GetTimeSeriesCommands();
        Task<TopKCommands> GetTopKCommands();
    }
}