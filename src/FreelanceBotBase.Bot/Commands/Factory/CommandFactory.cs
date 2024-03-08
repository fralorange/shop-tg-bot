using FreelanceBotBase.Bot.Commands.Callback.Null;
using FreelanceBotBase.Bot.Commands.Factory.Abstract;
using FreelanceBotBase.Bot.Commands.Factory.Factories;
using FreelanceBotBase.Bot.Commands.Interface;
using FreelanceBotBase.Bot.Commands.Text.Null;
using FreelanceBotBase.Domain.State;

namespace FreelanceBotBase.Bot.Commands.Factory
{
    /// <summary>
    /// Command factory<br/>
    /// One factory to rule them all!
    /// </summary>
    public class CommandFactory : ITextCommandFactory, ICallbackCommandFactory, ICallbackCommandWithInputFactory, IChatCommandFactory
    {
        private readonly UserCommandFactory _userCommandFactory;
        private readonly ProductCommandFactory _productCommandFactory;
        private readonly ChatCommandFactory _chatCommandFactory;

        /// <summary>
        /// Creates new united factory.
        /// </summary>
        /// <param name="userCommandFactory"></param>
        /// <param name="productCommandFactory"></param>
        /// <param name="chatCommandFactory"></param>
        public CommandFactory(UserCommandFactory userCommandFactory,
            ProductCommandFactory productCommandFactory,
            ChatCommandFactory chatCommandFactory)
        {
            _userCommandFactory = userCommandFactory;
            _productCommandFactory = productCommandFactory;
            _chatCommandFactory = chatCommandFactory;
        }
        public ICommand CreateCommand(string commandName)
        {
            ICommand command = _userCommandFactory.CreateCommand(commandName);
            command = command is NullCommand ? _productCommandFactory.CreateCommand(commandName) : command;

            return command;
        }

        public ICallbackCommand CreateCallbackCommand(string commandParam, long chatId)
        {
            ICallbackCommand callbackCommand = _userCommandFactory.CreateCallbackCommand(commandParam, chatId);
            callbackCommand = callbackCommand is NullCallbackCommand 
                ? _productCommandFactory.CreateCallbackCommand(commandParam, chatId) 
                : callbackCommand;

            return callbackCommand;
        }

        public ICallbackCommandWithInput CreateCallbackCommandWithUserInput(BotState.InputState inputState, long chatId)
        {
            ICallbackCommandWithInput callbackCommandWithInput = _userCommandFactory.CreateCallbackCommandWithUserInput(inputState, chatId);
            callbackCommandWithInput = callbackCommandWithInput is NullCallbackCommand 
                ? _productCommandFactory.CreateCallbackCommandWithUserInput(inputState, chatId) 
                : callbackCommandWithInput;

            return callbackCommandWithInput;
        }

        public ICommand CreateChatCommand(string commandName)
        {
            return _chatCommandFactory.CreateChatCommand(commandName);
        }
    }
}
