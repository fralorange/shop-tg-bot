using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Helpers;
using FreelanceBotBase.Infrastructure.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

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
            var output = PaginationHelper.FormatProductRecords(paginatedRecords);
            var inlineKeyboard = InlineKeyboardHelper.CreateDefaultInlineKeyboard();

            return await BotClient.EditMessageTextAsync(
                chatId: chatId,
                messageId: callbackQuery.Message.MessageId,
                text: output,
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
    }
}
