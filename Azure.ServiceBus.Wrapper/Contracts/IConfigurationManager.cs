namespace Azure.ServiceBus.Wrapper.Configurations.Contracts
{
    public interface IConfigurationManager
    {
        ICacheStore CacheStore { get; }
        string GetValue(string key);
        bool CanCacheValues { get; }
    }
}
