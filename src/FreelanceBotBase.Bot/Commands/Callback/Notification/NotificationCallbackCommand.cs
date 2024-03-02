using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Helpers;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.DeliveryPoint.Repositories;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBotBase.Bot.Commands.Callback.Notification
{
    /// <summary>
    /// Callback command that sends notification to delivery point manager.
    /// </summary>
    public class NotificationCallbackCommand : CallbackCommandBase
    {
        private readonly IDeliveryPointRepository _deliveryPointRepository;
        /// <summary>
        /// Creates notification callback command.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="deliveryPointRepository"></param>
        public NotificationCallbackCommand(ITelegramBotClient botClient, IDeliveryPointRepository deliveryPointRepository) : base(botClient)
            => _deliveryPointRepository = deliveryPointRepository;

        public override async Task<Message> HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            var text = callbackQuery.Message!.Text!;
            var dpId = Convert.ToInt64(Regex.Match(text, "(№)(.*)(,)").Groups[2].Value);
            var deliveryPoint = await _deliveryPointRepository.GetByIdAsync(dpId);

            var managerId = deliveryPoint.ManagerId;
            var chatId = callbackQuery.Message.Chat.Id;

            await BotClient.EditMessageTextAsync(
                chatId: chatId,
                messageId: callbackQuery.Message.MessageId,
                text: text,
                replyMarkup: null,
                cancellationToken: cancellationToken);

            await BotClient.SendTextMessageAsync(
                chatId: managerId!,
                text: $"Пользователь по ID: {callbackQuery.From.Id}, отправил вам запрос на покупку",
                replyMarkup: KeyboardHelper.CreateNotificationInlineKeyboard(),
                cancellationToken: cancellationToken);

            return await BotClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Уведомление менеджеру успешно отправлено!",
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
        }
    }
}
