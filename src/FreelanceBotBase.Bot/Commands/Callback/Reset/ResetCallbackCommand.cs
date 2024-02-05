using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Helpers;
using FreelanceBotBase.Infrastructure.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Callback.Reset
{
    public class ResetCallbackCommand : CallbackCommandBase
    {
        private readonly GoogleSheetsHelper _googleSheetsHelper;
        private readonly IMemoryCache _cache;

        public ResetCallbackCommand(ITelegramBotClient client, GoogleSheetsHelper googleSheetsHelper, IMemoryCache cache) : base(client)
        {
            _googleSheetsHelper = googleSheetsHelper;
            _cache = cache;
        }

        public async override Task<Message> HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            var records = await _googleSheetsHelper.GetRecordsAsync();

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60),
                SlidingExpiration = TimeSpan.FromMinutes(15),
            };

            _cache.Set($"{callbackQuery.Message!.Chat.Id}_records", records, cacheOptions.SetSize(10));

            int currentPage = 1;

            _cache.Set($"{callbackQuery.Message.Chat.Id}_currentPage", currentPage, cacheOptions);

            var paginatedRecords = PaginationHelper.SplitByPages(records, 10, currentPage);
            var output = PaginationHelper.FormatProductRecords(paginatedRecords);
            var inlineKeyboard = InlineKeyboardHelper.CreateDefaultInlineKeyboard();

            return await BotClient.EditMessageTextAsync(
                chatId: callbackQuery.Message.Chat.Id,
                messageId: callbackQuery.Message.MessageId,
                text: output,
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
    }
}
