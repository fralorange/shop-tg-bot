using FreelanceBotBase.Bot;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddConfigurations(context);

        services.AddHttpClients();

        services.AddMapper();

        services.AddRepositories();
        services.AddFacades();

        services.AddMemoryCache();

        services.AddDbContext();

        services.AddHelpers();

        services.AddFactories();

        services.AddServices();
        services.AddHandlers();
    })
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddUserSecrets<Program>();
    })
    .Build();

await host.RunAsync();