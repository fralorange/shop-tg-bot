using AutoMapper;
using FreelanceBotBase.Bot.Commands.Callback.Null;
using FreelanceBotBase.Bot.Commands.Callback.User;
using FreelanceBotBase.Bot.Commands.Factory.Abstract;
using FreelanceBotBase.Bot.Commands.Interface;
using FreelanceBotBase.Bot.Commands.Text.GetCurrentId;
using FreelanceBotBase.Bot.Commands.Text.GetUsers;
using FreelanceBotBase.Bot.Commands.Text.Null;
using FreelanceBotBase.Bot.Services.State;
using FreelanceBotBase.Domain.State;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.User.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;

namespace FreelanceBotBase.Bot.Commands.Factory.Factories
{
    /// <summary>
    /// Factory that creates various commands for user only.
    /// </summary>
    public class UserCommandFactory : ITextCommandFactory, ICallbackCommandFactory, ICallbackCommandWithInputFactory
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IMemoryCache _cache;
        private readonly IBotStateService _botStateService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Creates new user command factory.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="cache"></param>
        /// <param name="botStateService"></param>
        /// <param name="userRepository"></param>
        /// <param name="mapper"></param>
        public UserCommandFactory(
            ITelegramBotClient botClient,
            IMemoryCache cache,
            IBotStateService botStateService,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _botClient = botClient;
            _cache = cache;
            _botStateService = botStateService;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public ICommand CreateCommand(string commandName)
        {
            return commandName switch
            {
                "/getcurrentid" => new GetCurrentIdCommand(_botClient),
                "/getusers" => new GetUsersCommand(_botClient, _userRepository, _mapper),
                _ => new NullCommand()
            };
        }

        public ICallbackCommand CreateCallbackCommand(string commandParam, long chatId)
        {
            var botState = _botStateService.GetOrCreateBotState(chatId);

            return commandParam switch
            {
                "add_manager" => new AddUserAsManager(_botClient, _userRepository, botState),
                "remove_manager" => new RemoveUserFromManager(_botClient, _userRepository, botState),
                _ => new NullCallbackCommand()
            };
        }

        public ICallbackCommandWithInput CreateCallbackCommandWithUserInput(BotState.InputState inputState, long chatId)
        {
            var botState = _botStateService.GetOrCreateBotState(chatId);

            return inputState switch
            {
                BotState.InputState.AddingUser => new AddUserAsManager(_botClient, _userRepository, botState),
                BotState.InputState.RemovingUser => new RemoveUserFromManager(_botClient, _userRepository, botState),
                _ => new NullCallbackCommand()
            };
        }
    }
}
