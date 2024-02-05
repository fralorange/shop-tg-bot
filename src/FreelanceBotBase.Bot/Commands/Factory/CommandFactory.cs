using FreelanceBotBase.Bot.Commands.Callback.Null;
using FreelanceBotBase.Bot.Commands.Callback.Pages;
using FreelanceBotBase.Bot.Commands.Callback.Reset;
using FreelanceBotBase.Bot.Commands.Callback.Search;
using FreelanceBotBase.Bot.Commands.Interface;
using FreelanceBotBase.Bot.Commands.Text.GetProducts;
using FreelanceBotBase.Bot.Commands.Text.Null;
using FreelanceBotBase.Domain.States;
using FreelanceBotBase.Infrastructure.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;

namespace FreelanceBotBase.Bot.Commands.Factory
{
    public class CommandFactory
    {
        private readonly ITelegramBotClient _botClient;
        private readonly GoogleSheetsHelper _googleSheetsHelper;
        private readonly IMemoryCache _cache;
        private readonly BotState _botState;

        public CommandFactory(ITelegramBotClient botClient, GoogleSheetsHelper googleSheetsHelper, IMemoryCache cache, BotState botState)
        {
            _botClient = botClient;
            _googleSheetsHelper = googleSheetsHelper;
            _cache = cache;
            _botState = botState;
        }

        public ICommand CreateCommand(string commandName)
        {
            return commandName switch
            {
                "/getproducts" => new GetProductsCommand(_botClient, _googleSheetsHelper, _cache),
                _ => new NullCommand()
            };
        }

        public ICallbackCommand CreateCallbackCommand(string commandParam)
        {
            return commandParam switch
            {
                "prev_page" => new PagesCallbackCommand(_botClient, _cache),
                "next_page" => new PagesCallbackCommand(_botClient, _cache),
                "search" => new SearchCallbackCommand(_botClient, _cache, _botState),
                "reset" => new ResetCallbackCommand(_botClient, _googleSheetsHelper, _cache),
                _ => new NullCallbackCommand()
            };
        }

        // Maybe refactor and remove this method somehow since it overextends factory imo.
        public ICallbackCommandWithInput CreateCallbackCommandWithUserInput(BotState.InputState inputState)
        {
            return inputState switch
            {
                BotState.InputState.Search => new SearchCallbackCommand(_botClient, _cache, _botState),
                _ => new NullCallbackCommand()
            };
        }
    }
}
