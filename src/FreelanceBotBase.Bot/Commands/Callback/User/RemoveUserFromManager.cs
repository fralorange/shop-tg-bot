using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Commands.Interface;
using FreelanceBotBase.Domain.State;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.User.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

using UserEntity = FreelanceBotBase.Domain.User.User;

namespace FreelanceBotBase.Bot.Commands.Callback.User
{
    /// <summary>
    /// Callback command that allows to remove user from manager spot.
    /// </summary>
    public class RemoveUserFromManager : CallbackCommandBase, ICallbackCommandWithInput
    {
        /// <summary>
        /// User repository.
        /// </summary>
        private readonly IUserRepository _userRepository;
        /// <summary>
        /// Bot state.
        /// </summary>
        private readonly BotState _botState;

        /// <summary>
        /// Creates callback command that allows to remove user from manager spot.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="userRepository"></param>
        /// <param name="botState"></param>
        public RemoveUserFromManager(ITelegramBotClient botClient, IUserRepository userRepository, BotState botState) : base(botClient)
        {
            _userRepository = userRepository;
            _botState = botState;
        }

        public async override Task<Message> HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            _botState.CurrentState = BotState.State.AwaitingInput;
            _botState.AwaitingInputState = BotState.InputState.RemovingUser;

            await BotClient.EditMessageTextAsync(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message.MessageId,
                text: callbackQuery.Message.Text!,
                replyMarkup: null,
                cancellationToken: cancellationToken);

            return await BotClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: "Введите ID пользователя, которого желаете удалить из системы.",
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
        }

        public async Task<Message> HandleUserInput(string userInput, Message message, CancellationToken cancellationToken)
        {
            _botState.CurrentState = BotState.State.Default;
            _botState.AwaitingInputState = BotState.InputState.None;

            if (long.TryParse(userInput, out long userId))
            {
                var user = new UserEntity { UserId = userId, UserRole = UserEntity.Role.Manager };
                var deleted = await _userRepository.DeleteAsync(user, cancellationToken);
                
                if (!deleted)
                {
                    return await BotClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: $"Данный ID: {userId}, не был найден в системе!",
                        replyMarkup: new ReplyKeyboardRemove(),
                        cancellationToken: cancellationToken);
                }

                return await BotClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"Пользователь по ID: {userId} был успешно удалён из системы!",
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: cancellationToken);
            } else
            {
                return await BotClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"ID не является правильным, попытайтесь ещё раз!",
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: cancellationToken);
            }
        }
    }
}
