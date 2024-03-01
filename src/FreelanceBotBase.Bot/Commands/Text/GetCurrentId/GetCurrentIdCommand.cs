using FreelanceBotBase.Bot.Commands.Base;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBotBase.Bot.Commands.Text.GetCurrentId
{
    /// <summary>
    /// Command to get current user's id.
    /// </summary>
    public class GetCurrentIdCommand : CommandBase
    {
        /// <summary>
        /// Creates get current user id command.
        /// </summary>
        /// <param name="botClient"></param>
        public GetCurrentIdCommand(ITelegramBotClient botClient) : base(botClient) { } 

        public override Task<Message> ExecuteAsync(Message message, CancellationToken cancellationToken)
        {
            return BotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Ваш ID: {message.From!.Id}",
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
        }
    }
}
