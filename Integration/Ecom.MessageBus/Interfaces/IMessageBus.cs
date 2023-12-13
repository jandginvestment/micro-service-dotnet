namespace Ecom.MessageBus.Interfaces
{
    public interface IMessageBus
    {
        Task PublishMessage(object message, string topicQueName);
    }
}
