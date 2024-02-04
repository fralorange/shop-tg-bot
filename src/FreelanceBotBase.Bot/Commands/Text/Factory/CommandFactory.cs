using FreelanceBotBase.Bot.Commands.Text.GetProducts;
using FreelanceBotBase.Bot.Commands.Text.Interface;
using FreelanceBotBase.Bot.Commands.Text.Null;
using FreelanceBotBase.Infrastructure.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;

namespace FreelanceBotBase.Bot.Commands.Text.Factory
{
    public class CommandFactory
    {
        private readonly ITelegramBotClient _botClient;
        private readonly GoogleSheetsHelper _googleSheetsHelper;
        private readonly IMemoryCache _cache;

        public CommandFactory(ITelegramBotClient botClient, GoogleSheetsHelper googleSheetsHelper, IMemoryCache cache)
        {
            _botClient = botClient;
            _googleSheetsHelper = googleSheetsHelper;
            _cache = cache;
        }

        public ICommand CreateCommand(string commandName)
        {
            return commandName switch
            {
                "/getproducts" => new GetProductsCommand(_botClient, _googleSheetsHelper, _cache),
                _ => new NullCommand()
                //_ => new UsageCommand(_botClient)
            };
        }
    }
}
