using Azure.ServiceBus.Wrapper.Configurations.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Azure.ServiceBus.Wrapper.CacheStore
{
    public class InMemoryStore : ICacheStore
    {
        private static MemoryCache _store = MemoryCache.Default;

        public void AddToCache<T>(string key, T value)
        {
            if (_store.Contains(key))
                _store.Remove(key);

            _store.Add(key, value, new CacheItemPolicy());
        }

        public T Get<T>(string key)
        {
            T result = default(T);
            if (!_store.Contains(key)) return result;
            var value = _store.Get(key);
            if (value == null) return result;
            return (T)value;
        }

        public bool Remove(string key)
        {
            if (!_store.Contains(key)) return false;
            _store.Remove(key);
            return true;
        }

        public bool Contains(string key)
        {
            return _store.Contains(key);
        }
    }
}
