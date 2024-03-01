using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Interface
{
    /// <summary>
    /// Telegram callback command.
    /// </summary>
    public interface ICallbackCommand
    {
        /// <summary>
        /// Handles callback query.
        /// </summary>
        /// <param name="callbackQuery">Query.</param>
        /// <param name="cancellationToken">Canellation Token.</param>
        /// <returns>Message</returns>
        Task<Message> HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken);
    }
}
