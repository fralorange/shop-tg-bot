using FreelanceBotBase.Bot.Commands.Interface;
using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Callback.Null
{
    public class NullCallbackCommand : ICallbackCommand
    {
        public Task<Message> HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Message());
        }
    }
}
