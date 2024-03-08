using FreelanceBotBase.Bot.Commands.Interface;

namespace FreelanceBotBase.Bot.Commands.Factory.Abstract
{
    /// <summary>
    /// Factory that creates basic commands.
    /// </summary>
    public interface ITextCommandFactory
    {
        /// <summary>
        /// Creates command.
        /// </summary>
        /// <param name="commandName">Command name (with tag).</param>
        /// <returns></returns>
        ICommand CreateCommand(string commandName);
    }
}
