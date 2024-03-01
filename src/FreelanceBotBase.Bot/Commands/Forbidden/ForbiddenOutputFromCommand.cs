using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBotBase.Bot.Commands.Forbidden
{
    /// <summary>
    /// Output for forbidden commands.
    /// </summary>
    public static class ForbiddenOutputFromCommand
    {
        /// <summary>
        /// Sends that using this command is forbidden.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Message.</returns>
        public static Task<Message> ForbidAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            return botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Нет доступа к данной команде!",
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
        }
    }
}
