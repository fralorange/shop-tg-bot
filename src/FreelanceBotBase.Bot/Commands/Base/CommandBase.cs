using FreelanceBotBase.Bot.Commands.Interface;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Base
{
    public abstract class CommandBase : ICommand
    {
        protected readonly ITelegramBotClient BotClient;
        public CommandBase(ITelegramBotClient botClient) => BotClient = botClient;

        public abstract Task<Message> ExecuteAsync(Message message, CancellationToken cancellationToken);
    }
}
