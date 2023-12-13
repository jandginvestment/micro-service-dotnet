using Azure.Messaging.ServiceBus;
using Ecom.MessageBus.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace Ecom.MessageBus
{
    public class MessageBus : IMessageBus
    {
        private string _constring = "";
        public async Task PublishMessage(object message, string topicQueName)
        {
            await using var client = new ServiceBusClient(_constring);
            ServiceBusSender sender = client.CreateSender(topicQueName);
            var jsonMessage = JsonConvert.SerializeObject(message);
            ServiceBusMessage serviceBusMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage)) { CorrelationId = Guid.NewGuid().ToString() };

            await sender.SendMessageAsync(serviceBusMessage);
            await client.DisposeAsync();
        }
    }
}
