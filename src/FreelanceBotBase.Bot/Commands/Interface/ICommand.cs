using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Interface
{
    public interface ICommand
    {
        Task<Message> ExecuteAsync(Message message, CancellationToken cancellationToken);
    }
}
