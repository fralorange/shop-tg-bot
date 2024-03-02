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
using FreelanceBotBase.Bot.Commands.Text.GetCurrentId;
using FreelanceBotBase.Bot.Commands.Text.AddDeliveryPoint;
using FreelanceBotBase.Bot.Commands.Callback.CreateDP;
using FreelanceBotBase.Bot.Commands.Text.GetDeliveryPoints;
using FreelanceBotBase.Bot.Commands.Callback.AsignDp;
using FreelanceBotBase.Bot.Commands.Callback.User;
using FreelanceBotBase.Bot.Commands.Callback.Notification;
using FreelanceBotBase.Bot.Services.Chat;
using FreelanceBotBase.Bot.Commands.Callback.Chat;
using FreelanceBotBase.Bot.Commands.Text.Chat;

namespace FreelanceBotBase.Bot.Commands.Factory
{
    /// <summary>
    /// Command factory<br/>
    /// One factory to rule them all!
    /// </summary>
    public class CommandFactory
    {
        /// <summary>
        /// Bot client.
        /// </summary>
        private readonly ITelegramBotClient _botClient;
        /// <summary>
        /// Bot state service.
        /// </summary>
        private readonly IBotStateService _botStateService; // maybe store botState and init it in constructor but still inject service.       
        /// <summary>
        /// Memory cache.
        /// </summary>
        private readonly IMemoryCache _cache;
        /// <summary>
        /// Cart service.
        /// </summary>
        private readonly ICartService _cartService;
        /// <summary>
        /// Chat service.
        /// </summary>
        private readonly IChatService _chatService;
        /// <summary>
        /// Mapper.
        /// </summary>
        private readonly IMapper _mapper;
        /// <summary>
        /// Google sheets helper.
        /// </summary>
        private readonly GoogleSheetsHelper _googleSheetsHelper;
        #region Repos and Facades
        /// <summary>
        /// Deliver point repository.
        /// </summary>
        private readonly IDeliveryPointRepository _deliveryPointRepository;
        /// <summary>
        /// Delivery point facade.
        /// </summary>
        private readonly IDeliveryPointFacade _deliveryPointFacade;
        /// <summary>
        /// User repository.
        /// </summary>
        private readonly IUserRepository _userRepository;
        /// <summary>
        /// User facade.
        /// </summary>
        private readonly IUserFacade _userFacade;
        #endregion

        /// <summary>
        /// Creates mew factory.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="cache"></param>
        /// <param name="botStateService"></param>
        /// <param name="cartService"></param>
        /// <param name="mapper"></param>
        /// <param name="googleSheetsHelper"></param>
        /// <param name="deliveryPointRepository"></param>
        /// <param name="deliveryPointFacade"></param>
        /// <param name="userRepository"></param>
        /// <param name="userFacade"></param>
        /// <param name="chatService"></param>
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
            IUserFacade userFacade,
            IChatService chatService)
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
            _chatService = chatService;
        }

        /// <summary>
        /// Creates new command.
        /// </summary>
        /// <param name="commandName">Command prefix and name.</param>
        /// <returns>New command <see cref="ICommand"/></returns>
        public ICommand CreateCommand(string commandName)
        {
            return commandName switch
            {
                "/getproducts" => new GetProductsCommand(_botClient, _googleSheetsHelper, _cache, _mapper),
                "/getcart" => new GetCartCommand(_botClient, _cartService),
                "/getcurrentid" => new GetCurrentIdCommand(_botClient),
                "/getusers" => new GetUsersCommand(_botClient, _userRepository, _mapper),
                "/getdeliverypoints" => new GetDeliveryPointsCommand(_botClient, _deliveryPointRepository, _userRepository, _mapper),
                "/createdp" => new AddDeliveryPointCommand(_botClient, _userRepository),
                _ => new NullCommand()
            };
        }

        /// <summary>
        /// Creates new callback command.
        /// </summary>
        /// <param name="commandParam">Command data.</param>
        /// <param name="chatId">Chat to init user's chatbot state.</param>
        /// <returns></returns>
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
                "add_dp_name" => new AddFieldToDpCallbackCommand(_botClient, botState, _cache, commandParam, "Название"),
                "add_dp_location" => new AddFieldToDpCallbackCommand(_botClient, botState, _cache, commandParam, "Локация"),
                "create_dp" => new ConfirmDpConfigurationCallbackCommand(_botClient, _deliveryPointRepository),
                "select_dp" => new AsignDeliveryPointCallbackCommand(_botClient, _deliveryPointFacade, _deliveryPointRepository, _userFacade, _userRepository, botState),
                "clear_dp" => new ClearDeliveryPointCallbackCommand(_botClient, _deliveryPointFacade, _userFacade, _userRepository),
                "add_manager" => new AddUserAsManager(_botClient, _userRepository, botState),
                "remove_manager" => new RemoveUserFromManager(_botClient, _userRepository, botState),
                "notification" => new NotificationCallbackCommand(_botClient, _deliveryPointRepository),
                "chat" => new ChatCallbackCommand(_botClient, _chatService, _botStateService, _cartService),
                _ => new NullCallbackCommand()
            };
        }

        // Maybe refactor and remove this method somehow since it overextends factory imo.
        /// <summary>
        /// Create new callback command with user input.
        /// </summary>
        /// <param name="inputState">Input state.</param>
        /// <param name="chatId">Chat to init user's chatbot state.</param>
        /// <returns></returns>
        public ICallbackCommandWithInput CreateCallbackCommandWithUserInput(BotState.InputState inputState, long chatId)
        {
            var botState = _botStateService.GetOrCreateBotState(chatId);

            return inputState switch
            {
                BotState.InputState.Search => new SearchCallbackCommand(_botClient, _cache, botState),
                BotState.InputState.Selection => new SelectCallbackCommand(_botClient, _cache, botState),
                BotState.InputState.Delete => new DeleteCallbackCommand(_botClient, _cartService, botState),
                BotState.InputState.ChoosingDeliveryPoint => new CheckoutCallbackCommand(_botClient, _deliveryPointRepository, _mapper, botState),
                BotState.InputState.EditingDpName => new AddFieldToDpCallbackCommand(_botClient, botState, _cache, "add_dp_name", "Название"),
                BotState.InputState.EditingDpLocation => new AddFieldToDpCallbackCommand(_botClient, botState, _cache, "add_dp_location", "Локация"),
                BotState.InputState.AsigningDpManager => new AsignDeliveryPointCallbackCommand(_botClient, _deliveryPointFacade, _deliveryPointRepository, _userFacade, _userRepository, botState),
                BotState.InputState.AddingUser => new AddUserAsManager(_botClient, _userRepository, botState),
                BotState.InputState.RemovingUser => new RemoveUserFromManager(_botClient, _userRepository, botState),
                _ => new NullCallbackCommand()
            };
        }

        public ICommand CreateChatCommand(string commandName)
        {
            return commandName switch
            {
                "Завершить сессию чата" => new DisconnectChatSessionCommand(_botClient, _chatService, _botStateService),
                _ => new RespondChatMessageCommand(_botClient, _chatService)
            };
        }
    }
}
