using FreelanceBotBase.Bot.Commands.Factory.Abstract;
using FreelanceBotBase.Bot.Commands.Interface;
using FreelanceBotBase.Bot.Commands.Text.Chat;
using FreelanceBotBase.Bot.Services.Chat;
using FreelanceBotBase.Bot.Services.State;
using Telegram.Bot;

namespace FreelanceBotBase.Bot.Commands.Factory.Factories
{
    /// <summary>
    /// Factory that creates chat commands only.
    /// </summary>
    public class ChatCommandFactory : IChatCommandFactory
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IChatService _chatService;
        private readonly IBotStateService _botStateService;

        /// <summary>
        /// Creates new chat command factory.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="chatService"></param>
        /// <param name="botStateService"></param>
        public ChatCommandFactory(ITelegramBotClient botClient, IChatService chatService, IBotStateService botStateService)
        {
            _botClient = botClient;
            _chatService = chatService;
            _botStateService = botStateService;
        }

        public ICommand CreateChatCommand(string commandName)
        {
            return commandName switch
            {
                "Завершить сессию чата" => new DisconnectChatSessionCommand(_botClient, _chatService, _botStateService),
                _ => new RespondChatMessageCommand(_botClient, _chatService),
            };
        }
    }
}
