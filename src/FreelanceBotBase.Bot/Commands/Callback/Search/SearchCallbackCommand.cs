using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Commands.Interface;
using FreelanceBotBase.Bot.Helpers;
using FreelanceBotBase.Domain.Product;
using FreelanceBotBase.Domain.States;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBotBase.Bot.Commands.Callback.Search
{
    public class SearchCallbackCommand : CallbackCommandBase, ICallbackCommandWithInput
    {
        private readonly IMemoryCache _cache;
        private readonly BotState _botState;

        public SearchCallbackCommand(ITelegramBotClient client, IMemoryCache cache, BotState botState) : base(client)
        {
            _cache = cache;
            _botState = botState;
        }

        public async override Task<Message> HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            _botState.CurrentState = BotState.State.AwaitingInput;
            _botState.AwaitingInputState = BotState.InputState.Search;

            return await BotClient.EditMessageTextAsync(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message.MessageId,
                text: "Введите в поле для сообщений название продукта...",
                replyMarkup: null,
                cancellationToken: cancellationToken);
        }

        public async Task<Message> HandleUserInput(string userInput, long chatId, CancellationToken cancellationToken)
        {
            _botState.CurrentState = BotState.State.Default;
            _botState.AwaitingInputState = BotState.InputState.None;

            //FIX DRY!!!
            var records = _cache.Get<IEnumerable<ProductRecord>>($"{chatId}_records");

            if (records is null)
            {
                return await BotClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Время ответа истекло. Пожалуйста, сгенерируйте новый список!",
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: cancellationToken);
            }

            var filteredRecords = records.Where(r => r.Product.StartsWith(userInput)).ToList();

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60),
                SlidingExpiration = TimeSpan.FromMinutes(15),
            };

            _cache.Set($"{chatId}_records", filteredRecords, cacheOptions.SetSize(10));

            int currentPage = 1;

            _cache.Set($"{chatId}_currentPage", currentPage, cacheOptions);

            var paginatedRecords = PaginationHelper.SplitByPages(filteredRecords, 10, currentPage);
            var output = PaginationHelper.FormatProductRecords(paginatedRecords);
            var inlineKeyboard = InlineKeyboardHelper.CreateSearchInlineKeyboard();

            return await BotClient.SendTextMessageAsync(
                chatId: chatId,
                text: output,
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
    }
}
