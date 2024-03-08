using FreelanceBotBase.Bot.Commands.Interface;

namespace FreelanceBotBase.Bot.Commands.Factory.Abstract
{
    /// <summary>
    /// Factory that creates commands for chatting through chatbot.
    /// </summary>
    public interface IChatCommandFactory
    {
        /// <summary>
        /// Creates chat command.
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        ICommand CreateChatCommand(string commandName);
    }
}
