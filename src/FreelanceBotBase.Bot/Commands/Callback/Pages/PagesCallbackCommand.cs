﻿using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Helpers;
using FreelanceBotBase.Domain.Product;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBotBase.Bot.Commands.Callback.Pages
{
    public class PagesCallbackCommand : CallbackCommandBase
    {
        private readonly IMemoryCache _cache;

        public PagesCallbackCommand(ITelegramBotClient client, IMemoryCache cache) : base(client)
            => _cache = cache;

        public async override Task<Message> HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            var currentPage = _cache.Get<int>($"{callbackQuery.Message!.Chat.Id}_currentPage");
            var records = _cache.Get<IEnumerable<ProductRecord>>($"{callbackQuery.Message.Chat.Id}_records");

            if (records is null)
            {
                return await BotClient.SendTextMessageAsync(
                    chatId: callbackQuery.Message.Chat.Id,
                    text: "Время ответа истекло. Пожалуйста, сгенерируйте новый список!",
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: cancellationToken);
            }

            int maxPage = (records.Count() + 9) / 10;
            currentPage = callbackQuery.Data == "next_page" ? Math.Min(currentPage + 1, maxPage) : Math.Max(currentPage - 1, 1);

            _cache.Set($"{callbackQuery.Message.Chat.Id}_currentPage", currentPage);

            var paginatedRecords = PaginationHelper.SplitByPages(records, 10, currentPage);
            var output = PaginationHelper.FormatProductRecords(paginatedRecords);
            var inlineKeyboard = PaginationHelper.CreateInlineKeyboard();

            return await BotClient.EditMessageTextAsync(
                chatId: callbackQuery.Message.Chat.Id,
                messageId: callbackQuery.Message.MessageId,
                text: output,
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
    }
}
