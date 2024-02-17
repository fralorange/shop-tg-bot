using FreelanceBotBase.Bot.Commands.Callback.AddProduct;
using FreelanceBotBase.Bot.Commands.Callback.Null;
using FreelanceBotBase.Bot.Commands.Callback.Pages;
using FreelanceBotBase.Bot.Commands.Callback.Reset;
using FreelanceBotBase.Bot.Commands.Callback.Search;
using FreelanceBotBase.Bot.Commands.Callback.Select;
using FreelanceBotBase.Bot.Commands.Interface;
using FreelanceBotBase.Bot.Commands.Text.GetCart;
using FreelanceBotBase.Bot.Commands.Text.GetProducts;
using FreelanceBotBase.Bot.Commands.Text.Null;
using FreelanceBotBase.Bot.Services.BotState;
using FreelanceBotBase.Bot.Services.Cart;
using FreelanceBotBase.Domain.States;
using FreelanceBotBase.Infrastructure.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;

namespace FreelanceBotBase.Bot.Commands.Factory
{
    public class CommandFactory
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IBotStateService _botStateService;
        private readonly IMemoryCache _cache;
        private readonly ICartService _cartService;
        private readonly GoogleSheetsHelper _googleSheetsHelper;

        public CommandFactory(
            ITelegramBotClient botClient, 
            IMemoryCache cache, 
            IBotStateService botStateService, 
            ICartService cartService, 
            GoogleSheetsHelper googleSheetsHelper)
        {
            _botClient = botClient;
            _botStateService = botStateService;
            _cache = cache;
            _cartService = cartService;
            _googleSheetsHelper = googleSheetsHelper;
        }

        public ICommand CreateCommand(string commandName)
        {
            return commandName switch
            {
                "/getproducts" => new GetProductsCommand(_botClient, _googleSheetsHelper, _cache),
                "/getcart" => new GetCartCommand(_botClient, _cartService),
                _ => new NullCommand()
            };
        }

        public ICallbackCommand CreateCallbackCommand(string commandParam, long chatId)
        {
            var botState = _botStateService.GetOrCreateBotState(chatId);

            return commandParam switch
            {
                "prev_page" => new PagesCallbackCommand(_botClient, _cache),
                "next_page" => new PagesCallbackCommand(_botClient, _cache),
                "search" => new SearchCallbackCommand(_botClient, _cache, botState),
                "reset" => new ResetCallbackCommand(_botClient, _googleSheetsHelper, _cache),
                "select" => new SelectCallbackCommand(_botClient, _cache, botState),
                "confirm" => new AddProductCallbackCommand(_botClient, _cartService, _cache),
                _ => new NullCallbackCommand()
            };
        }

        // Maybe refactor and remove this method somehow since it overextends factory imo.
        public ICallbackCommandWithInput CreateCallbackCommandWithUserInput(BotState.InputState inputState, long chatId)
        {
            var botState = _botStateService.GetOrCreateBotState(chatId);

            return inputState switch
            {
                BotState.InputState.Search => new SearchCallbackCommand(_botClient, _cache, botState),
                BotState.InputState.Selection => new SelectCallbackCommand(_botClient, _cache, botState),
                _ => new NullCallbackCommand()
            };
        }
    }
}
