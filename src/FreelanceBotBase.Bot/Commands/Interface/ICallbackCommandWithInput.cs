using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Interface
{
    public interface ICallbackCommandWithInput : ICallbackCommand
    {
        Task<Message> HandleUserInput(string userInput, long chatId, CancellationToken cancellationToken);
    }
}
