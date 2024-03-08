using AutoMapper;
using FreelanceBotBase.Bot.Commands.Factory;
using FreelanceBotBase.Bot.Commands.Factory.Factories;
using FreelanceBotBase.Bot.Handlers.Update;
using FreelanceBotBase.Bot.Services.Cart;
using FreelanceBotBase.Bot.Services.Chat;
using FreelanceBotBase.Bot.Services.Polling;
using FreelanceBotBase.Bot.Services.Receiver;
using FreelanceBotBase.Bot.Services.State;
using FreelanceBotBase.Infrastructure.ComponentRegistrar.Mappers.DeliveryPoint;
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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace FreelanceBotBase.Bot
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Add services into DI.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IBotStateService, BotStateService>();
            services.AddSingleton<IChatService, ChatService>();
            services.AddSingleton<ICartService, CartService>();
            services.AddScoped<ReceiverService>();
            services.AddHostedService<PollingService>();

            return services;
        }

        /// <summary>
        /// Add handlers to DI.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddScoped<UpdateHandler>();

            return services;
        }

        /// <summary>
        /// Add repositories to DI.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IDeliveryPointRepository, DeliveryPointRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        /// <summary>
        /// Add facades to DI.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddFacades(this IServiceCollection services)
        {
            services.AddScoped<IDeliveryPointFacade, DeliveryPointFacade>();
            services.AddScoped<IUserFacade, UserFacade>();

            return services;
        }

        /// <summary>
        /// Add DbContext to DI.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDbContext(this IServiceCollection services)
        {
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddSingleton<IDbContextOptionsConfigurator<BaseDbContext>, BaseDbContextOptionsConfigurator>();
            services.AddDbContext<BaseDbContext>((sp, opt) =>
            {
                var configurator = sp.GetRequiredService<IDbContextOptionsConfigurator<BaseDbContext>>();
                configurator.Configure((DbContextOptionsBuilder<BaseDbContext>)opt);
            });
            services.AddScoped<DbContext, BaseDbContext>();

            return services;
        }

        /// <summary>
        /// Add helpers to DI.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddHelpers(this IServiceCollection services)
        {
            services.AddTransient<GoogleSheetsHelper>();

            return services;
        }

        /// <summary>
        /// Add factories to DI.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddFactories(this IServiceCollection services)
        {
            services.AddScoped<UserCommandFactory>();
            services.AddScoped<ProductCommandFactory>();
            services.AddScoped<ChatCommandFactory>();
            services.AddScoped<CommandFactory>();

            return services;
        }

        /// <summary>
        /// Add mapper to DI.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductMapper>();
                cfg.AddProfile<UserMapper>();
                cfg.AddProfile<DeliveryPointMapper>();
            });

            config.AssertConfigurationIsValid();
            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }

        /// <summary>
        /// Add http clients to DI.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient("telegram_bot_client")
            .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
            {
                BotConfiguration? botConfig = sp.GetConfiguration<BotConfiguration>();
                TelegramBotClientOptions options = new(botConfig.BotToken);
                return new TelegramBotClient(options, httpClient);
            });

            services.AddHttpClient("google_sheets_client");

            return services;
        }

        /// <summary>
        /// Add configurations to DI.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IServiceCollection AddConfigurations(this IServiceCollection services, HostBuilderContext context)
        {
            services.Configure<BotConfiguration>(
                context.Configuration.GetSection(BotConfiguration.Configuration));

            services.Configure<ReceiverOptions>(
                context.Configuration.GetSection(nameof(ReceiverOptions)));

            return services;
        }
    }
}
