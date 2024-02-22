using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Helpers;
using FreelanceBotBase.Contracts.Product;
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
            var chatId = callbackQuery.Message!.Chat.Id;

            var currentPage = _cache.Get<int>($"{callbackQuery.Message!.Chat.Id}_currentPage");
            var records = _cache.Get<IEnumerable<ProductDto>>($"{callbackQuery.Message.Chat.Id}_records");

            if (records is null)
            {
                return await BotClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Время ответа истекло. Пожалуйста, сгенерируйте новый список!",
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: cancellationToken);
            }

            int maxPage = (records.Count() + 9) / 10;
            int newPage = callbackQuery.Data == "next_page" ? Math.Min(currentPage + 1, maxPage) : Math.Max(currentPage - 1, 1);

            if (newPage == currentPage)
            {
                await BotClient.AnswerCallbackQueryAsync(
                    callbackQueryId: callbackQuery.Id,
                    text: newPage == maxPage ? "Вы уже находитесь на последней странице!" : "Вы уже находитесь на первой странице!",
                    showAlert: true,
                    cancellationToken: cancellationToken);

                return callbackQuery.Message;
            }

            currentPage = newPage;
            _cache.Set($"{chatId}_currentPage", currentPage);

            var paginatedRecords = PaginationHelper.SplitByPages(records, 10, currentPage);
            var output = PaginationHelper.Format(paginatedRecords);

            return await BotClient.EditMessageTextAsync(
                chatId: chatId,
                messageId: callbackQuery.Message.MessageId,
                text: output,
                replyMarkup: callbackQuery.Message.ReplyMarkup,
                cancellationToken: cancellationToken);
        }
    }
}
