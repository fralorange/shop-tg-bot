using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Commands.Interface;
using FreelanceBotBase.Domain.State;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.DeliveryPoint.Facades;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.DeliveryPoint.Repositories;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.User.Facades;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.User.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBotBase.Bot.Commands.Callback.AsignDp
{
    /// <summary>
    /// Asign delivery point to manager callback command.
    /// </summary>
    public class AsignDeliveryPointCallbackCommand : CallbackCommandBase, ICallbackCommandWithInput
    {
        /// <summary>
        /// Delivery point facade.
        /// </summary>
        private readonly IDeliveryPointFacade _deliveryPointFacade;
        /// <summary>
        /// Delivery point repository.
        /// </summary>
        private readonly IDeliveryPointRepository _deliveryPointRepository;
        /// <summary>
        /// User facade.
        /// </summary>
        private readonly IUserFacade _userFacade;
        /// <summary>
        /// User repository.
        /// </summary>
        private readonly IUserRepository _userRepository;
        /// <summary>
        /// Bot state.
        /// </summary>
        private readonly BotState _botState;

        /// <summary>
        /// Creates callback command that asigns delivery point to a new manager.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="deliveryPointFacade"></param>
        /// <param name="deliveryPointRepository"></param>
        /// <param name="userFacade"></param>
        /// <param name="userRepository"></param>
        /// <param name="botState"></param>
        public AsignDeliveryPointCallbackCommand(
            ITelegramBotClient botClient,
            IDeliveryPointFacade deliveryPointFacade,
            IDeliveryPointRepository deliveryPointRepository,
            IUserFacade userFacade,
            IUserRepository userRepository,
            BotState botState)
            : base(botClient)
        {
            _deliveryPointFacade = deliveryPointFacade;
            _deliveryPointRepository = deliveryPointRepository;
            _userFacade = userFacade;
            _userRepository = userRepository;
            _botState = botState;
        }

        /// <inheritdoc/>
        public override async Task<Message> HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        { 
            _botState.CurrentState = BotState.State.AwaitingInput;
            _botState.AwaitingInputState = BotState.InputState.AsigningDpManager;

            return await BotClient.SendTextMessageAsync(
                chatId: callbackQuery.Message!.Chat.Id,
                text: "Введите ID пункта выдачи.",
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<Message> HandleUserInput(string userInput, Message message, CancellationToken cancellationToken)
        {
            _botState.CurrentState = BotState.State.Default;
            _botState.AwaitingInputState = BotState.InputState.None;

            if (long.TryParse(userInput, out var dpId))
            {
                var dp = await _deliveryPointRepository.GetByIdAsync(dpId);
                if (dp == null)
                    return await BotClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Данный ID не найден в системе!",
                        replyMarkup: new ReplyKeyboardRemove(),
                        cancellationToken: cancellationToken);
                else if (dp.ManagerId != null)
                    return await BotClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Данный пункт выдачи уже занят другим менеджером!",
                        replyMarkup: new ReplyKeyboardRemove(),
                        cancellationToken: cancellationToken);

                var userId = message.From!.Id;
                var user = await _userRepository.GetByIdAsync(userId);
                if (user.DeliveryPointId != null)
                    return await BotClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Текущий пользователь уже представлен к пункту выдачи!",
                            replyMarkup: new ReplyKeyboardRemove(),
                            cancellationToken: cancellationToken);

                await _userFacade.AssignDeliveryPointAsync(userId, dpId, cancellationToken);
                await _deliveryPointFacade.SetManagerAsync(dpId, userId, cancellationToken);

                return await BotClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"Пользователь <{userId}> успешно привязан к пункту по номеру: {dpId}",
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
