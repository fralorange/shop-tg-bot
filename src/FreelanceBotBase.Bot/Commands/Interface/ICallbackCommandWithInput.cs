using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Interface
{
    /// <summary>
    /// <inheritdoc/> [with input]
    /// </summary>
    public interface ICallbackCommandWithInput : ICallbackCommand
    {
        /// <summary>
        /// Handles user input.
        /// </summary>
        /// <param name="userInput">User input.</param>
        /// <param name="message">Message.</param>
        /// <param name="cancellationToken">Token.</param>
        /// <returns>Message.</returns>
        Task<Message> HandleUserInput(string userInput, Message message, CancellationToken cancellationToken);
    }
}
