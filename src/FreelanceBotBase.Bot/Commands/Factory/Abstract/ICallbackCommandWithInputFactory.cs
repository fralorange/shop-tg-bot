using FreelanceBotBase.Bot.Commands.Interface;
using FreelanceBotBase.Domain.State;

namespace FreelanceBotBase.Bot.Commands.Factory.Abstract
{
    /// <summary>
    /// Factory that creates callback commands and awaits user input.
    /// </summary>
    public interface ICallbackCommandWithInputFactory
    {
        /// <summary>
        /// Creates callback command with input.
        /// </summary>
        /// <param name="inputState">Input state.</param>
        /// <param name="chatId">Chat id.</param>
        /// <returns></returns>
        ICallbackCommandWithInput CreateCallbackCommandWithUserInput(BotState.InputState inputState, long chatId);
    }
}
