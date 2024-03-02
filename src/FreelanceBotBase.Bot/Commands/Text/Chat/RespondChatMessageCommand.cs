using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Services.Chat;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Text.Chat
{
    /// <summary>
    /// Command that forwards one users message to another.
    /// </summary>
    public class RespondChatMessageCommand : CommandBase
    {
        private readonly IChatService _chatService;

        /// <summary>
        /// Creates new respond chat message command.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="chatService"></param>
        public RespondChatMessageCommand(ITelegramBotClient botClient, IChatService chatService) : base(botClient)
            => _chatService = chatService;

        public override async Task<Message> ExecuteAsync(Message message, CancellationToken cancellationToken)
        {
            var userId = message.From!.Id;
            var targetId = _chatService.GetConnectedUser(userId);

            return await BotClient.SendTextMessageAsync(
                chatId: targetId!,
                text: message.Text!,
                cancellationToken: cancellationToken);
        }
    }
}
