using FreelanceBotBase.Bot.Commands.Factory;
using FreelanceBotBase.Bot.Handlers.Update;
using FreelanceBotBase.Bot.Services.BotState;
using FreelanceBotBase.Bot.Services.Cart;
using FreelanceBotBase.Bot.Services.Polling;
using FreelanceBotBase.Bot.Services.Receiver;
using FreelanceBotBase.Domain.States;
using FreelanceBotBase.Infrastructure.Configuration;
using FreelanceBotBase.Infrastructure.Extensions;
using FreelanceBotBase.Infrastructure.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Polling;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<BotConfiguration>(
            context.Configuration.GetSection(BotConfiguration.Configuration));

        services.Configure<ReceiverOptions>(
            context.Configuration.GetSection(nameof(ReceiverOptions)));


        services.AddHttpClient("telegram_bot_client")
            .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
            {
                BotConfiguration? botConfig = sp.GetConfiguration<BotConfiguration>();
                TelegramBotClientOptions options = new(botConfig.BotToken);
                return new TelegramBotClient(options, httpClient);
            });

        services.AddHttpClient("google_sheets_client");

        services.AddMemoryCache();

        services.AddTransient<GoogleSheetsHelper>();
        services.AddSingleton<CommandFactory>();
        services.AddSingleton<IBotStateService, BotStateService>();
        services.AddSingleton<ICartService, CartService>();
        services.AddScoped<UpdateHandler>();
        services.AddScoped<ReceiverService>();
        services.AddHostedService<PollingService>();
    })
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddUserSecrets<Program>();
    })
    .Build();

await host.RunAsync();