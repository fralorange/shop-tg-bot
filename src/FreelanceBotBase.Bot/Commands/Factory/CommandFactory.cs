using FreelanceBotBase.Bot.Commands.Greeting;
using FreelanceBotBase.Bot.Commands.Interface;
using FreelanceBotBase.Bot.Commands.Usage;
using Telegram.Bot;

namespace FreelanceBotBase.Bot.Commands.Factory
{
    public class CommandFactory
    {
        private readonly ITelegramBotClient _botClient;

        public CommandFactory(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public ICommand CreateCommand(string commandName)
        {
            return commandName switch
            {
                "/hi" => new GreetingCommand(_botClient),
                _ => new UsageCommand(_botClient)
                // _ => new NullCommand()
            };
        }
    }
}
