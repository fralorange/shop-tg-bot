using FreelanceBotBase.Bot.Commands.Interface;

namespace FreelanceBotBase.Bot.Commands.Factory.Abstract
{
    /// <summary>
    /// Factory that creates callback commands.
    /// </summary>
    public interface ICallbackCommandFactory
    {
        /// <summary>
        /// Creates callback command.
        /// </summary>
        /// <param name="commandParam">Command parameter.</param>
        /// <param name="chatId">Chat id.</param>
        /// <returns></returns>
        ICallbackCommand CreateCallbackCommand(string commandParam, long chatId);
    }
}
