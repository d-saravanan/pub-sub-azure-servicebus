using Azure.ServiceBus.Wrapper;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureServicebus.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            AzureServiceBusClient.Initialize("testingtopic");
            var factory = AzureServiceBusClient.MessageFactory;

            AzureServiceBusClient.ManageSubscriptions("testingtopic", "Inventory");
            var sender = AzureServiceBusClient.GetMessageSender("testingtopic");

            //Sending a message 
            BrokeredMessage message = new BrokeredMessage(System.IO.Path.GetRandomFileName());
            sender.Send(message);
            Console.WriteLine("Message(s) sent.");

            Console.WriteLine("Done, press a key to continue...");

            //MessageReceiver testQueueReceiver = AzureServiceBusClient.GetMessageReceiver("testingtopic");
            //while (true)
            //{
            //    using (BrokeredMessage retrievedMessage = testQueueReceiver.Receive())
            //    {
            //        try
            //        {
            //            Console.WriteLine("Message(s) Retrieved: " + retrievedMessage.GetBody<string>());
            //            retrievedMessage.Complete();
            //        }
            //        catch (Exception ex)
            //        {
            //            Console.WriteLine(ex.ToString());
            //            retrievedMessage.Abandon();
            //        }
            //    }
            //}

            Console.ReadKey();
        }
    }

    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
