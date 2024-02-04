using FreelanceBotBase.Bot.Commands.Text.Base;
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
            _cache.Set("Records", records, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60),
                SlidingExpiration = TimeSpan.FromMinutes(15),
                Size = 10,
            });

            var paginatedRecords = PaginationHelper<ProductRecord>.SplitByPages(records, 10, 1);

            string separator = new('-', 90);
            string output = string.Join("\n" + separator + "\n", paginatedRecords.Select(r => $"Продукт: {r.Product}\nЦена: {r.Cost}"));

            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Предыдущая страница", "prev_page"),
                    InlineKeyboardButton.WithCallbackData("Следующая страница", "next_page"),
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Выбрать", "select"),
                    InlineKeyboardButton.WithCallbackData("Поиск", "search"),
                }
            });

            return await BotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: output,
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
    }
}
