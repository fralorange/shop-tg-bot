﻿using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Domain.DeliveryPoint;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.DeliveryPoint.Repositories;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBotBase.Bot.Commands.Callback.CreateDP
{
    /// <summary>
    /// Callback command to confirm configuration for new delivery point.
    /// </summary>
    public class ConfirmDpConfigurationCallbackCommand : CallbackCommandBase
    {
        /// <summary>
        /// Delivery point repository.
        /// </summary>
        private readonly IDeliveryPointRepository _deliveryPointRepository;

        /// <summary>
        /// Creates new confirm delivery point configuration callback command.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="deliveryPointRepository"></param>
        public ConfirmDpConfigurationCallbackCommand(ITelegramBotClient botClient, IDeliveryPointRepository deliveryPointRepository)
            : base(botClient)
            => _deliveryPointRepository = deliveryPointRepository;

        public async override Task<Message> HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            var text = callbackQuery.Message!.Text!;

            var name = Regex.Match(text, @"(Название: )(.*)(\n)").Groups[2].Value;
            var location = Regex.Match(text, "(Локация: )(.*)").Groups[2].Value;

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
