using FreelanceBotBase.Bot.Commands.Interface;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Base
{
    /// <summary>
    /// Base callback command.
    /// </summary>
    public abstract class CallbackCommandBase : ICallbackCommand
    {
        /// <summary>
        /// Telegram Bot Client.
        /// </summary>
        protected readonly ITelegramBotClient BotClient;
        /// <summary>
        /// Creates new callback command.
        /// </summary>
        /// <param name="botClient">Telegram Bot Client.</param>
        public CallbackCommandBase(ITelegramBotClient botClient)
            => BotClient = botClient;

        /// <summary>
        /// Handles callback query.
        /// </summary>
        /// <param name="callbackQuery">Query.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Message.</returns>
        public abstract Task<Message> HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken);
    }
}
