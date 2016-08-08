using Azure.ServiceBus.Wrapper.CacheStore;
using Azure.ServiceBus.Wrapper.Configurations.Contracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.ServiceBus.Wrapper.Configurations
{
    public sealed class AzureServiceBusConfigurationManager : IConfigurationManager
    {
        private static volatile AzureServiceBusConfigurationManager instance;
        private static object syncRoot = new object();

        public AzureServiceBusConfigurationManager() { }

        public static AzureServiceBusConfigurationManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new AzureServiceBusConfigurationManager();
                    }
                }
                return instance;
            }

        }

        private static ICacheStore _cacheStore;

        public ICacheStore CacheStore
        {
            get
            {
                if (_cacheStore == null)
                    _cacheStore = new InMemoryStore();

                return _cacheStore;
            }
        }

        public bool CanCacheValues
        {
            get
            {
                return true;
            }
        }

        public string GetValue(string key)
        {
            if (CanCacheValues && instance.CacheStore.Contains(key))
                return instance.CacheStore.Get<string>(key);

            string val = ConfigurationManager.AppSettings[key];
            if (CanCacheValues)
                instance.CacheStore.AddToCache(key, val);
            return val;
        }
    }
}
