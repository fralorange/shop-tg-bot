using AutoMapper;
using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Helpers;
using FreelanceBotBase.Contracts.Product;
using FreelanceBotBase.Infrastructure.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Text.GetProducts
{
    public class GetProductsCommand : CommandBase
    {
        private readonly GoogleSheetsHelper _googleSheetsHelper;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;

        public GetProductsCommand(ITelegramBotClient client, GoogleSheetsHelper googleSheetsHelper, IMemoryCache cache, IMapper mapper) : base(client)
        {
            _googleSheetsHelper = googleSheetsHelper;
            _cache = cache;
            _mapper = mapper;
        }

        public async override Task<Message> ExecuteAsync(Message message, CancellationToken cancellationToken)
        {
            var records = await _googleSheetsHelper.GetRecordsAsync();
            var recordsDto = _mapper.Map<IEnumerable<ProductDto>>(records);

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60),
                SlidingExpiration = TimeSpan.FromMinutes(15),
            };

            _cache.Set($"{message.Chat.Id}_records", recordsDto, cacheOptions.SetSize(10));

            int currentPage = 1;

            _cache.Set($"{message.Chat.Id}_currentPage", currentPage, cacheOptions);

            var paginatedRecords = PaginationHelper.SplitByPages(recordsDto, 10, currentPage);
            var output = PaginationHelper.Format(paginatedRecords);
            var inlineKeyboard = InlineKeyboardHelper.CreateDefaultInlineKeyboard();

            return await BotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: output,
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
    }
}
