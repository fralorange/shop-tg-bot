using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Domain.DeliveryPoint;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.DeliveryPoint.Repositories;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBotBase.Bot.Commands.Callback.CreateDP
{
    public class ConfirmDpConfigurationCallbackCommand : CallbackCommandBase
    {
        private readonly IDeliveryPointRepository _deliveryPointRepository;

        public ConfirmDpConfigurationCallbackCommand(ITelegramBotClient botClient, IDeliveryPointRepository deliveryPointRepository)
            : base(botClient)
            => _deliveryPointRepository = deliveryPointRepository;

        public async override Task<Message> HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            var text = callbackQuery.Message!.Text!;

            var name = Regex.Match(text, @"(Название: )(.*)(\n)").Groups[2].ToString();
            var location = Regex.Match(text, "(Локация: )(.*)").Groups[2].ToString();

            var dp = new DeliveryPoint { Name = name, Location = location };
            
            await _deliveryPointRepository.AddAsync(dp, cancellationToken);

            return await BotClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: $"{name} успешно добавлен в БД",
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
        }
    }
}
