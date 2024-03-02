using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Services.Chat;
using FreelanceBotBase.Bot.Services.State;
using FreelanceBotBase.Domain.State;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBotBase.Bot.Commands.Text.Chat
{
    /// <summary>
    /// Command that disconnects both users from chat session.
    /// </summary>
    public class DisconnectChatSessionCommand : CommandBase
    {
        private readonly IChatService _chatService;
        private readonly IBotStateService _botStateService;

        /// <summary>
        /// Creates new disconnect chat session command.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="chatService"></param>
        /// <param name="botStateService"></param>
        public DisconnectChatSessionCommand(ITelegramBotClient botClient, IChatService chatService, IBotStateService botStateService) : base(botClient)
        {
            _chatService = chatService;
            _botStateService = botStateService;
        }

        public async override Task<Message> ExecuteAsync(Message message, CancellationToken cancellationToken)
        {
            var user1Id = message.From!.Id;
            var user2Id = _chatService.GetConnectedUser(user1Id);

            _chatService.DisconnectUsers(user1Id, user2Id!.Value);

            var user1BotState = _botStateService.GetOrCreateBotState(user1Id);
            var user2BotState = _botStateService.GetOrCreateBotState(user2Id.Value);

            user1BotState.CurrentState = BotState.State.Default;
            user2BotState.CurrentState = BotState.State.Default;

            return await BotClient.SendTextMessageAsync(
                chatId: user2Id,
                text: "Сессия чата окончена!",
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
        }
    }
}
