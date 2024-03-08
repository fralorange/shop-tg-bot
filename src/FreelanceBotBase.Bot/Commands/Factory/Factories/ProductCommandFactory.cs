using AutoMapper;
using FreelanceBotBase.Bot.Commands.Callback.AddProduct;
using FreelanceBotBase.Bot.Commands.Callback.Back;
using FreelanceBotBase.Bot.Commands.Callback.Checkout;
using FreelanceBotBase.Bot.Commands.Callback.Clear;
using FreelanceBotBase.Bot.Commands.Callback.Delete;
using FreelanceBotBase.Bot.Commands.Callback.Null;
using FreelanceBotBase.Bot.Commands.Callback.Pages;
using FreelanceBotBase.Bot.Commands.Callback.Reset;
using FreelanceBotBase.Bot.Commands.Callback.Search;
using FreelanceBotBase.Bot.Commands.Callback.Select;
using FreelanceBotBase.Bot.Commands.Factory.Abstract;
using FreelanceBotBase.Bot.Commands.Interface;
using FreelanceBotBase.Bot.Commands.Text.GetCart;
using FreelanceBotBase.Bot.Commands.Text.GetProducts;
using FreelanceBotBase.Bot.Commands.Text.Null;
using FreelanceBotBase.Bot.Services.Cart;
using FreelanceBotBase.Bot.Services.State;
using FreelanceBotBase.Domain.State;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.DeliveryPoint.Repositories;
using FreelanceBotBase.Infrastructure.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;

namespace FreelanceBotBase.Bot.Commands.Factory.Factories
{
    /// <summary>
    /// Factory that creates various commands for products only.
    /// </summary>
    public class ProductCommandFactory : ITextCommandFactory, ICallbackCommandFactory, ICallbackCommandWithInputFactory
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IMemoryCache _cache;
        private readonly IBotStateService _botStateService;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;
        private readonly GoogleSheetsHelper _googleSheetsHelper;
        private readonly IDeliveryPointRepository _deliveryPointRepository;

        /// <summary>
        /// Creates new product command factory.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="cache"></param>
        /// <param name="botStateService"></param>
        /// <param name="cartService"></param>
        /// <param name="mapper"></param>
        /// <param name="googleSheetsHelper"></param>
        /// <param name="deliveryPointRepository"></param>
        public ProductCommandFactory(ITelegramBotClient botClient,
            IMemoryCache cache,
            IBotStateService botStateService,
            ICartService cartService,
            IMapper mapper,
            GoogleSheetsHelper googleSheetsHelper,
            IDeliveryPointRepository deliveryPointRepository)
        {
            _botClient = botClient;
            _cache = cache;
            _botStateService = botStateService;
            _cartService = cartService;
            _mapper = mapper;
            _googleSheetsHelper = googleSheetsHelper;
            _deliveryPointRepository = deliveryPointRepository;
        }

        public ICommand CreateCommand(string commandName)
        {
            return commandName switch
            {
                "/getproducts" => new GetProductsCommand(_botClient, _googleSheetsHelper, _cache, _mapper),
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
                "delete" => new DeleteCallbackCommand(_botClient, _cartService, botState),
                "clear" => new ClearCallbackCommand(_botClient, _cartService),
                "back" => new BackCartCallbackCommand(_botClient, _cartService),
                "checkout" => new CheckoutCallbackCommand(_botClient, _deliveryPointRepository, _mapper, botState),
                _ => new NullCallbackCommand()
            };
        }

        public ICallbackCommandWithInput CreateCallbackCommandWithUserInput(BotState.InputState inputState, long chatId)
        {
            var botState = _botStateService.GetOrCreateBotState(chatId);

            return inputState switch
            {
                BotState.InputState.Search => new SearchCallbackCommand(_botClient, _cache, botState),
                BotState.InputState.Selection => new SelectCallbackCommand(_botClient, _cache, botState),
                BotState.InputState.Delete => new DeleteCallbackCommand(_botClient, _cartService, botState),
                BotState.InputState.ChoosingDeliveryPoint => new CheckoutCallbackCommand(_botClient, _deliveryPointRepository, _mapper, botState),
                _ => new NullCallbackCommand()
            };
        }
    }
}
