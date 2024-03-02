using AutoMapper;
using FreelanceBotBase.Bot.Commands.Factory;
using FreelanceBotBase.Bot.Handlers.Update;
using FreelanceBotBase.Bot.Services.State;
using FreelanceBotBase.Bot.Services.Cart;
using FreelanceBotBase.Bot.Services.Polling;
using FreelanceBotBase.Bot.Services.Receiver;
using FreelanceBotBase.Infrastructure.ComponentRegistrar.Mappers.Product;
using FreelanceBotBase.Infrastructure.ComponentRegistrar.Mappers.User;
using FreelanceBotBase.Infrastructure.Configuration;
using FreelanceBotBase.Infrastructure.DataAccess;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.DeliveryPoint.Facades;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.DeliveryPoint.Repositories;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.User.Facades;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.User.Repositories;
using FreelanceBotBase.Infrastructure.DataAccess.Interfaces;
using FreelanceBotBase.Infrastructure.Extensions;
using FreelanceBotBase.Infrastructure.Helpers;
using FreelanceBotBase.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Polling;
using FreelanceBotBase.Infrastructure.ComponentRegistrar.Mappers.DeliveryPoint;
using FreelanceBotBase.Bot.Services.Chat;
// to-do: create clean architecture.
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
        #region Mappers
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductMapper>();
            cfg.AddProfile<UserMapper>();
            cfg.AddProfile<DeliveryPointMapper>();
        });

        config.AssertConfigurationIsValid();
        IMapper mapper = config.CreateMapper();
        services.AddSingleton(mapper);
        #endregion

        #region Repos
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IDeliveryPointRepository, DeliveryPointRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        #region Facades
        services.AddScoped<IDeliveryPointFacade, DeliveryPointFacade>();
        services.AddScoped<IUserFacade, UserFacade>();
        #endregion
        #endregion

        services.AddMemoryCache();

        services.AddScoped<IDbInitializer, DbInitializer>();
        services.AddSingleton<IDbContextOptionsConfigurator<BaseDbContext>, BaseDbContextOptionsConfigurator>();
        services.AddDbContext<BaseDbContext>((sp, opt) =>
        {
            var configurator = sp.GetRequiredService<IDbContextOptionsConfigurator<BaseDbContext>>();
            configurator.Configure((DbContextOptionsBuilder<BaseDbContext>)opt);
        });
        services.AddScoped<DbContext, BaseDbContext>();

        services.AddTransient<GoogleSheetsHelper>();
        services.AddScoped<CommandFactory>();
        services.AddSingleton<IBotStateService, BotStateService>();
        services.AddSingleton<IChatService, ChatService>();
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