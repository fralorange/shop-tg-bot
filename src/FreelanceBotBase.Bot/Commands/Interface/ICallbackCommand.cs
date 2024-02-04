using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Interface
{
    public interface ICallbackCommand
    {
        Task<Message> HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken);
    }
}
