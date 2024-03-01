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
    public class AsignDeliveryPointCallbackCommand : CallbackCommandBase, ICallbackCommandWithInput
    {
        private readonly IDeliveryPointFacade _deliveryPointFacade;
        private readonly IDeliveryPointRepository _deliveryPointRepository;
        private readonly IUserFacade _userFacade;
        private readonly IUserRepository _userRepository;
        private readonly BotState _botState;

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
