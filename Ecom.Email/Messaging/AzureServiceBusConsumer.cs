using Azure.Messaging.ServiceBus;
using ECOM.Services.EmailAPI.Models.DTO;
using Newtonsoft.Json;
using System.Text;

namespace Ecom.EmailAPI.Messaging;

public class AzureServiceBusConsumer : IAzureServiceBusConsumer
{
    private readonly String _ServiceBusConnectionString;
    private readonly String _EmailCartQue;
    private readonly IConfiguration _Configuration;

    private ServiceBusProcessor _EmailServiceProcessor;

    public AzureServiceBusConsumer(IConfiguration configuration)
    {
        _Configuration = configuration;
        _ServiceBusConnectionString = _Configuration.GetValue<string>("ServiceBusConnectionString");
        _EmailCartQue = _Configuration.GetValue<string>("TopicAndQueNames:EmailShoppingCartQue");

        var client = new ServiceBusClient(_ServiceBusConnectionString);
        _EmailServiceProcessor = client.CreateProcessor(_EmailCartQue);

    }

    public async Task Start()
    {
        _EmailServiceProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
        _EmailServiceProcessor.ProcessErrorAsync += ErrorHandler;
        await _EmailServiceProcessor.StartProcessingAsync();
    }
    private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs arg)
    {
        var message = arg.Message;
        var body = Encoding.UTF8.GetString(message.Body);
        ShoppingCartDTO shoppingCart = JsonConvert.DeserializeObject<ShoppingCartDTO>(body);

        try
        {
            await arg.CompleteMessageAsync(arg.Message);

        }
        catch (Exception)
        {

            throw;
        }

    }
    private Task ErrorHandler(ProcessErrorEventArgs arg)
    {
        //throw the email
        Console.WriteLine(arg.Exception);
        return Task.CompletedTask;
    }

    public async Task Stop()
    {
        await _EmailServiceProcessor.StopProcessingAsync();
        await _EmailServiceProcessor.DisposeAsync();
    }
}
