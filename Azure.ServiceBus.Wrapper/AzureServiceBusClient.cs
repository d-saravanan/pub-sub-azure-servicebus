using Azure.ServiceBus.Wrapper.Configurations;
using Azure.ServiceBus.Wrapper.Configurations.Contracts;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.ServiceBus.Wrapper
{
    public static class AzureServiceBusClient
    {
        private static IConfigurationManager _configManager = AzureServiceBusConfigurationManager.Instance;

        public static string ServiceBusConnectionString
        {
            get
            {
                return _configManager.GetValue("Azure.ServiceBus.ConnectionString");
            }
        }

        private static string ServicebusNameSpace
        {
            get
            {
                return _configManager.GetValue("Azure.ServiceBus.Namespace");
            }
        }

        private static string ServicebusSharedAccessKeyName
        {
            get
            {
                return _configManager.GetValue("Azure.ServiceBus.SharedAccessKeyName");
            }
        }

        private static string ServicebusSharedAccessKey
        {
            get
            {
                return _configManager.GetValue("Azure.ServiceBus.SharedAccessKey");
            }
        }

        private static MessagingFactory factory;

        static AzureServiceBusClient()
        {
            try
            {
                uri = ServiceBusEnvironment.CreateServiceUri("sb", ServicebusNameSpace, string.Empty);
                tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(ServicebusSharedAccessKeyName, ServicebusSharedAccessKey);
                namespaceManager = new NamespaceManager(uri, tokenProvider);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private static Uri uri = null;
        private static TokenProvider tokenProvider = null;
        private static NamespaceManager namespaceManager = null;

        public static void Initialize(string topicName)
        {
            if (!namespaceManager.TopicExists(topicName))
                namespaceManager.CreateTopic(topicName);
        }

        public static SubscriptionDescription ManageSubscriptions(string topicName, string subscriptionName)
        {
            if (!namespaceManager.SubscriptionExists(topicName, subscriptionName))
                return namespaceManager.CreateSubscription(topicName, subscriptionName);
            return null;
        }

        public static MessagingFactory MessageFactory
        {
            get
            {
                if (factory == null)
                {
                    if (string.IsNullOrWhiteSpace(ServiceBusConnectionString))
                        factory = MessagingFactory.CreateFromConnectionString(ServiceBusConnectionString);
                    else
                        factory = MessagingFactory.Create(uri, tokenProvider);
                }
                return factory;
            }
        }

        private static ConcurrentDictionary<string, MessageSender> _msgSenders = new ConcurrentDictionary<string, MessageSender>();
        private static ConcurrentDictionary<string, MessageReceiver> _msgReceivers = new ConcurrentDictionary<string, MessageReceiver>();

        public static MessageSender GetMessageSender(string namespaceorQueueName)
        {
            if (string.IsNullOrWhiteSpace(namespaceorQueueName)) return null;

            if (_msgSenders.ContainsKey(namespaceorQueueName)) return _msgSenders[namespaceorQueueName];
            MessageSender msgSender = null;

            msgSender = factory.CreateMessageSender(namespaceorQueueName);

            _msgSenders.TryAdd(namespaceorQueueName, msgSender);

            return msgSender;
        }

        public static MessageReceiver GetMessageReceiver(string namespaceorQueueName, string subscriberName = null)
        {
            if (string.IsNullOrWhiteSpace(namespaceorQueueName)) return null;

            string key = string.IsNullOrWhiteSpace(subscriberName)
                ? namespaceorQueueName
                : $"{namespaceorQueueName}/subscriptions/{subscriberName}";

            if (_msgReceivers.ContainsKey(key)) return _msgReceivers[key];

            MessageReceiver msgSender = factory.CreateMessageReceiver(key);

            _msgReceivers.TryAdd(key, msgSender);

            return msgSender;
        }
    }
}
