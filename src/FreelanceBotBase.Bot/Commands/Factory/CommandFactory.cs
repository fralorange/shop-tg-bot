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
using FreelanceBotBase.Bot.Commands.Interface;
using FreelanceBotBase.Bot.Commands.Text.GetCart;
using FreelanceBotBase.Bot.Commands.Text.GetProducts;
using FreelanceBotBase.Bot.Commands.Text.Null;
using FreelanceBotBase.Bot.Services.State;
using FreelanceBotBase.Bot.Services.Cart;
using FreelanceBotBase.Domain.State;
using FreelanceBotBase.Infrastructure.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.DeliveryPoint.Repositories;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.DeliveryPoint.Facades;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.User.Repositories;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.User.Facades;
using FreelanceBotBase.Bot.Commands.Text.GetUsers;

namespace FreelanceBotBase.Bot.Commands.Factory
{
    public class CommandFactory
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IBotStateService _botStateService;
        private readonly IMemoryCache _cache;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;
        private readonly GoogleSheetsHelper _googleSheetsHelper;
        #region Repos and Facades
        private readonly IDeliveryPointRepository _deliveryPointRepository;
        private readonly IDeliveryPointFacade _deliveryPointFacade;
        private readonly IUserRepository _userRepository;
        private readonly IUserFacade _userFacade;
        #endregion

        public CommandFactory(
            ITelegramBotClient botClient,
            IMemoryCache cache,
            IBotStateService botStateService,
            ICartService cartService,
            IMapper mapper,
            GoogleSheetsHelper googleSheetsHelper,
            IDeliveryPointRepository deliveryPointRepository,
            IDeliveryPointFacade deliveryPointFacade,
            IUserRepository userRepository,
            IUserFacade userFacade)
        {
            _botClient = botClient;
            _botStateService = botStateService;
            _cache = cache;
            _cartService = cartService;
            _mapper = mapper;
            _googleSheetsHelper = googleSheetsHelper;
            _deliveryPointRepository = deliveryPointRepository;
            _deliveryPointFacade = deliveryPointFacade;
            _userRepository = userRepository;
            _userFacade = userFacade;
        }

        public ICommand CreateCommand(string commandName)
        {
            return commandName switch
            {
                "/getproducts" => new GetProductsCommand(_botClient, _googleSheetsHelper, _cache, _mapper),
                "/getcart" => new GetCartCommand(_botClient, _cartService),
                "/getusers" => new GetUsersCommand(_botClient, _userRepository, _mapper),
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
                "checkout" => new CheckoutCallbackCommand(_botClient, _cartService, botState),
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
                BotState.InputState.Delete => new DeleteCallbackCommand(_botClient, _cartService, botState),
                BotState.InputState.ChoosingDeliveryPoint => new CheckoutCallbackCommand(_botClient, _cartService, botState),
                _ => new NullCallbackCommand()
            };
        }
    }
}
