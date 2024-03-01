using AutoMapper;
using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Commands.Interface;
using FreelanceBotBase.Bot.Helpers;
using FreelanceBotBase.Contracts.DeliveryPoint;
using FreelanceBotBase.Domain.State;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.DeliveryPoint.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Callback.Checkout
{
    /// <summary>
    /// Checkout callback command.
    /// </summary>
    public class CheckoutCallbackCommand : CallbackCommandBase, ICallbackCommandWithInput
    {
        /// <summary>
        /// Delivery point repository.
        /// </summary>
        private readonly IDeliveryPointRepository _deliveryPointRepository;
        /// <summary>
        /// Mapper.
        /// </summary>
        private readonly IMapper _mapper;
        /// <summary>
        /// Bot state.
        /// </summary>
        private readonly BotState _botState;

        /// <summary>
        /// Creates checkout callback command.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="deliveryPointRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="botState"></param>
        public CheckoutCallbackCommand(
            ITelegramBotClient botClient,
            IDeliveryPointRepository deliveryPointRepository,
            IMapper mapper,
            BotState botState) : base(botClient)
        {
            _deliveryPointRepository = deliveryPointRepository;
            _mapper = mapper;
            _botState = botState;
        }

        /// <inheritdoc/>
        public async override Task<Message> HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            var deliveryPoints = await _deliveryPointRepository.GetAll();
            var deliveryPointsDto = _mapper.Map<List<DeliveryPointDto>>(deliveryPoints);

            var output = PaginationHelper.Format(deliveryPointsDto);

            _botState.CurrentState = BotState.State.AwaitingInput;
            _botState.AwaitingInputState = BotState.InputState.ChoosingDeliveryPoint;

            return await BotClient.EditMessageTextAsync(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message.MessageId,
                text: "Введите в текстовое поле номер пункта выдачи из предложенных ниже\n" + output,
                replyMarkup: null,
                cancellationToken: cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<Message> HandleUserInput(string userInput, Message message, CancellationToken cancellationToken)
        {
            _botState.CurrentState = BotState.State.Default; // change to Chatting l8r when Admin responds
            _botState.AwaitingInputState = BotState.InputState.None;

            var chatId = message.Chat.Id;

            if (!int.TryParse(userInput, out var dpId))
                return await BotClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Ошибка: ожидалось число, но получено другое значение. Пожалуйста введите число!",
                    replyMarkup: InlineKeyboardHelper.CreateBackInlineKeyboard(),
                    cancellationToken: cancellationToken);

            var deliveryPoints = await _deliveryPointRepository.GetAll();
            var deliveryPoint = deliveryPoints.First(dp => dp.Id.Equals(dpId));

            var output = $"Вы выбрали пункт выдачи \"{deliveryPoint.Name}\" находящийся по адресу: \"{deliveryPoint.Location}\"\n";
            var inlineKeyboard = InlineKeyboardHelper.CreateConfirmChattingInlineKeyabord();

            return await BotClient.SendTextMessageAsync(
                chatId: chatId,
                text: output + "Подтвердив свой выбор вас перенаправит в чат с администратором данного пункта выдачи, как только он примет заявку",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);

            // it'll need to send information of products in cart of certain user to delivery point admin
        }
    }
}
