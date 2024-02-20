using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Commands.Interface;
using FreelanceBotBase.Bot.Helpers;
using FreelanceBotBase.Bot.Services.Cart;
using FreelanceBotBase.Domain.DeliveryPoint;
using FreelanceBotBase.Domain.States;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Callback.Checkout
{
    public class CheckoutCallbackCommand : CallbackCommandBase, ICallbackCommandWithInput
    {
        private readonly ICartService _cartService;
        private readonly BotState _botState;
        private readonly IEnumerable<DeliveryPoint> _deliveryPoints;

        public CheckoutCallbackCommand(ITelegramBotClient botClient, ICartService cartService, BotState botState) : base(botClient)
        {
            _cartService = cartService;
            _botState = botState;

            _deliveryPoints = new List<DeliveryPoint>
            {
                new() { Id = 1, Name = "Пункт 1", Location = "Ул. Ульянова 1" },
                new() { Id = 2, Name = "Пункт 2", Location = "Ул. Пожарова 2" },
                new() { Id = 3, Name = "Пункт 3", Location = "Ул. Петрова 3" },
                new() { Id = 4, Name = "Пункт 4", Location = "Ул. Демидова 4" },
                new() { Id = 5, Name = "Пункт 5", Location = "Ул. Нахимова 5" },
            };
        }

        public override Task<Message> HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        { 
            var separator = new string('-', 90);
            var output = string
                .Join('\n' + separator + '\n', _deliveryPoints.Select(dp => $"Номер: {dp.Id}\nНазвание пункта: {dp.Name}\nЛокация: {dp.Location}"));

            _botState.CurrentState = BotState.State.AwaitingInput;
            _botState.AwaitingInputState = BotState.InputState.ChoosingDeliveryPoint;

            return BotClient.EditMessageTextAsync(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message.MessageId,
                text: "Введите в текстовое поле номер пункта выдачи из предложенных ниже\n" + output,
                replyMarkup: null,
                cancellationToken: cancellationToken);
        }

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

            var deliveryPoint = _deliveryPoints.First(dp => dp.Id.Equals(dpId));

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
