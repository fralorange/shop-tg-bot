﻿using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Helpers;
using FreelanceBotBase.Infrastructure.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBotBase.Bot.Commands.Callback.Reset
{
    /// <summary>
    /// Callback command that returns original sequence of pages from google sheets. 
    /// </summary>
    public class ResetCallbackCommand : CallbackCommandBase
    {
        /// <summary>
        /// Google sheets helper.
        /// </summary>
        private readonly GoogleSheetsHelper _googleSheetsHelper;
        /// <summary>
        /// Memory cache.
        /// </summary>
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Creates new reset callback command.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="googleSheetsHelper"></param>
        /// <param name="cache"></param>
        public ResetCallbackCommand(ITelegramBotClient client, GoogleSheetsHelper googleSheetsHelper, IMemoryCache cache) : base(client)
        {
            _googleSheetsHelper = googleSheetsHelper;
            _cache = cache;
        }

        public async override Task<Message> HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            var chatId = callbackQuery.Message!.Chat.Id;
            
            var cacheHelper = new CacheHelper(_cache);
            var records = cacheHelper.GetRecords(chatId, out int currentPage);

            if (records is null)
            {
                return await BotClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Время ответа истекло. Пожалуйста, сгенерируйте новый список!",
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: cancellationToken);
            }

            var paginatedRecords = PaginationHelper.SplitByPages(records, 10, currentPage);
            var output = PaginationHelper.Format(paginatedRecords);
            var inlineKeyboard = KeyboardHelper.CreateDefaultInlineKeyboard();

            return await BotClient.EditMessageTextAsync(
                chatId: chatId,
                messageId: callbackQuery.Message.MessageId,
                text: output,
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
    }
}
