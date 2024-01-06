using Azure.Messaging.ServiceBus;
using Ecom.EmailAPI.Services;
using ECOM.Services.EmailAPI.Models.DTO;
using Newtonsoft.Json;
using System.Text;

namespace Ecom.EmailAPI.Messaging;

public class AzureServiceBusConsumer : IAzureServiceBusConsumer
{
    private readonly String _ServiceBusConnectionString;
    private readonly String _EmailCartQue;
    private readonly string _RegisterUserQue;
    private readonly IConfiguration _Configuration;
    private readonly EmailService _EmailService;

    private ServiceBusProcessor _EmailServiceProcessor;
    private ServiceBusProcessor _RegisterUserProcessor;

    public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
    {
        _Configuration = configuration;
        _ServiceBusConnectionString = _Configuration.GetValue<string>("ServiceBusConnectionString");
        _EmailCartQue = _Configuration.GetValue<string>("TopicAndQueNames:EmailShoppingCartQue");
        _RegisterUserQue = _Configuration.GetValue<string>("TopicAndQueNames:RegisterUserQue");


        var client = new ServiceBusClient(_ServiceBusConnectionString);
        _EmailServiceProcessor = client.CreateProcessor(_EmailCartQue);
        _RegisterUserProcessor = client.CreateProcessor(_RegisterUserQue);
        _EmailService = emailService;
    }

    public async Task Start()
    {
        _EmailServiceProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
        _EmailServiceProcessor.ProcessErrorAsync += ErrorHandler;
        await _EmailServiceProcessor.StartProcessingAsync();

        _RegisterUserProcessor.ProcessMessageAsync += OnRegisterUserRequestReceived;
        _RegisterUserProcessor.ProcessErrorAsync += ErrorHandler;
        await _RegisterUserProcessor.StartProcessingAsync();
    }



    private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs arg)
    {
        var message = arg.Message;
        var body = Encoding.UTF8.GetString(message.Body);
        ShoppingCartDTO shoppingCart = JsonConvert.DeserializeObject<ShoppingCartDTO>(body);

        try
        {
            await _EmailService.EmailCartAndLog(shoppingCart);
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

        await _RegisterUserProcessor.StopProcessingAsync();
        await _RegisterUserProcessor.DisposeAsync();
    }


    private async Task OnRegisterUserRequestReceived(ProcessMessageEventArgs arg)
    {
        var message = arg.Message;
        var body = Encoding.UTF8.GetString(message.Body);
        string email = JsonConvert.DeserializeObject<string>(body);

        try
        {
            await _EmailService.EmailRegisteredUserAndLog(email);
            await arg.CompleteMessageAsync(arg.Message);

        }
        catch (Exception)
        {

            throw;
        }
    }
}
