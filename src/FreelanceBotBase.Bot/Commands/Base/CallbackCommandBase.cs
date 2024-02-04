using FreelanceBotBase.Bot.Commands.Interface;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Base
{
    public abstract class CallbackCommandBase : ICallbackCommand
    {
        protected readonly ITelegramBotClient BotClient;
        public CallbackCommandBase(ITelegramBotClient botClient)
            => BotClient = botClient;

        public abstract Task<Message> HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken);
    }
}
