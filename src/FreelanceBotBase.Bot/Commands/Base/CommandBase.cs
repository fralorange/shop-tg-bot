using FreelanceBotBase.Bot.Commands.Interface;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Base
{
    /// <summary>
    /// Base command.
    /// </summary>
    public abstract class CommandBase : ICommand
    {
        /// <summary>
        /// Telegram Bot Client.
        /// </summary>
        protected readonly ITelegramBotClient BotClient;
        /// <summary>
        /// Creates new callback command.
        /// </summary>
        /// <param name="botClient">Telegram Bot Client.</param>
        public CommandBase(ITelegramBotClient botClient) 
            => BotClient = botClient;

        /// <summary>
        /// Handles Telegram message.
        /// </summary>
        /// <param name="message">Query.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Message.</returns>
        public abstract Task<Message> ExecuteAsync(Message message, CancellationToken cancellationToken);
    }
}
