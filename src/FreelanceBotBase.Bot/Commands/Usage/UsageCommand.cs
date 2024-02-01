using FreelanceBotBase.Bot.Commands.Base;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBotBase.Bot.Commands.Usage
{
    public class UsageCommand : CommandBase
    {
        public UsageCommand(ITelegramBotClient botClient) : base(botClient) { }

        public override async Task<Message> ExecuteAsync(Message message, CancellationToken cancellationToken)
        {
            const string usage = "Usage:\n" +
                                 "/hi - Greet bot.";

            return await BotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: usage,
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
        }
    }
}
