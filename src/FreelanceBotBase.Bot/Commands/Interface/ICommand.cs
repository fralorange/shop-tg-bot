using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Interface
{
    /// <summary>
    /// Telegram command.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Executes Telegram command.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="cancellationToken">Cancalletion Token.</param>
        /// <returns>Message.</returns>
        Task<Message> ExecuteAsync(Message message, CancellationToken cancellationToken);
    }
}
