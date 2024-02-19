using FreelanceBotBase.Bot.Commands.Interface;
using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Callback.Null
{
    public class NullCallbackCommand : ICallbackCommandWithInput
    {
        public Task<Message> HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Message());
        }

        public Task<Message> HandleUserInput(string userInput, Message message, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Message());
        }
    }
}
