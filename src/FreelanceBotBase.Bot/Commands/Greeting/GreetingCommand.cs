using FreelanceBotBase.Bot.Commands.Base;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBotBase.Bot.Commands.Greeting
{
    public class GreetingCommand : CommandBase
    {
        public GreetingCommand(ITelegramBotClient botClient) : base(botClient) { }

        public override async Task<Message> ExecuteAsync(Message message, CancellationToken cancellationToken)
        {
            const string greeting = "Hello!";

            return await BotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: greeting,
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
        }
    }
}
