using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Helpers;
using FreelanceBotBase.Domain.Product;
using FreelanceBotBase.Infrastructure.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBotBase.Bot.Commands.Text.GetProducts
{
    public class GetProductsCommand : CommandBase
    {
        private readonly GoogleSheetsHelper _googleSheetsHelper;
        private readonly IMemoryCache _cache;

        public GetProductsCommand(ITelegramBotClient client, GoogleSheetsHelper googleSheetsHelper, IMemoryCache cache) : base(client)
        {
            _googleSheetsHelper = googleSheetsHelper;
            _cache = cache;
        }

        public async override Task<Message> ExecuteAsync(Message message, CancellationToken cancellationToken)
        {
            var records = await _googleSheetsHelper.GetRecordsAsync();

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60),
                SlidingExpiration = TimeSpan.FromMinutes(15),
            };

            _cache.Set($"{message.Chat.Id}_records", records, cacheOptions.SetSize(10));

            int currentPage = 1;

            _cache.Set($"{message.Chat.Id}_currentPage", currentPage, cacheOptions);

            var paginatedRecords = PaginationHelper.SplitByPages(records, 10, currentPage);
            var output = PaginationHelper.FormatProductRecords(paginatedRecords);
            var inlineKeyboard = PaginationHelper.CreateInlineKeyboard();

            return await BotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: output,
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
    }
}
