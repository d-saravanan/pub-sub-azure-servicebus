using Azure.ServiceBus.Wrapper;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureServicebus.Consumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AzureServiceBusClient.Initialize("testingtopic");
            var factory = AzureServiceBusClient.MessageFactory;

            MessageReceiver receiver = AzureServiceBusClient.GetMessageReceiver("testingtopic", "Inventory");
            BrokeredMessage receivedMessage = receiver.Receive();
            try
            {
                Console.WriteLine("Message(s) Retrieved: " + receivedMessage.GetBody<string>());
                receivedMessage.Complete();
            }
            catch (Exception e)
            {
                receivedMessage.Abandon();
            }

            Console.WriteLine("Done reading the message from the queue");
            Console.ReadKey();

        }
    }
}
