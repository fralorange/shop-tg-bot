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
    public class AddUserAsManager : CallbackCommandBase, ICallbackCommandWithInput
    {
        private readonly IUserRepository _userRepository;
        private readonly BotState _botState;

        public AddUserAsManager(ITelegramBotClient botClient, IUserRepository userRepository, BotState botState) : base(botClient)
        {
            _userRepository = userRepository;
            _botState = botState;
        }

        public async override Task<Message> HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            _botState.CurrentState = BotState.State.AwaitingInput;
            _botState.AwaitingInputState = BotState.InputState.AddingUser;

            await BotClient.EditMessageTextAsync(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message.MessageId,
                text: callbackQuery.Message.Text!,
                replyMarkup: null,
                cancellationToken: cancellationToken);

            return await BotClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: "Введите ID пользователя, сообщенное им вам лично.",
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
                await _userRepository.AddAsync(user, cancellationToken);

                return await BotClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"Пользователь по ID: {userId} был успешно добавлен, рекомендуется записать кто это, поскольку База записывает лишь его ID!",
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: cancellationToken);
            } else
            {
                return await BotClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "ID не является правильным, попытайтесь ещё раз!",
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: cancellationToken);
            }
        }
    }
}
