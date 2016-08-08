namespace Azure.ServiceBus.Wrapper.Configurations.Contracts
{
    public interface ICacheStore
    {
        void AddToCache<T>(string key, T value);
        T Get<T>(string key);
        bool Remove(string key);
        bool Contains(string key);
    }
}