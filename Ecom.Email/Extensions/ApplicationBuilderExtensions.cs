namespace Ecom.EmailAPI.Extensions;

public static class ApplicationBuilderExtensions
{
    private static IAzureServiceBusConsumer _serviceBusConsumer { get; set; }
    public static IApplicationBuilder UseAzureBusConsumer(this IApplicationBuilder app)
    {
        _serviceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();
        var hostRunTime = app.ApplicationServices.GetService<IHostApplicationLifetime>();

        hostRunTime.ApplicationStarted.Register(OnStart);
        hostRunTime.ApplicationStopping.Register(OnStop);


        return app;
    }

    private static void OnStop()
    {
        _serviceBusConsumer.Stop();
    }

    private static void OnStart()
    {
        _serviceBusConsumer?.Start();
    }
}
