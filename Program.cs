using Microsoft.Extensions.DependencyInjection;
using TimeLoggerService;
IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureServices((context, services) =>
    {
        services.Configure<appsettings>(context.Configuration.GetSection("appsettings"));
        services.AddHostedService<Worker>();
    })
    .Build();
await host.RunAsync();
