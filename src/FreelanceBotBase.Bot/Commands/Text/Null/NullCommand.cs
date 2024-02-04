using FreelanceBotBase.Bot.Commands.Interface;
using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Text.Null
{
    public class NullCommand : ICommand
    {
        public Task<Message> ExecuteAsync(Message message, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Message());
        }
    }
}
