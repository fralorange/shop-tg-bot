using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Helpers;
using FreelanceBotBase.Bot.Services.Cart;
using FreelanceBotBase.Domain.Product;
using Microsoft.Extensions.Caching.Memory;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBotBase.Bot.Commands.Callback.AddProduct
{
    public class AddProductCallbackCommand : CallbackCommandBase
    {
        private readonly ICartService _cartService;
        private readonly IMemoryCache _cache;

        public AddProductCallbackCommand(ITelegramBotClient botClient, ICartService cartSerivce, IMemoryCache cache) : base(botClient)
        {
            _cartService = cartSerivce;
            _cache = cache;
        }

        public async override Task<Message> HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            //FIX DRY.
            var records = _cache.Get<IEnumerable<ProductRecord>>($"{callbackQuery.Message!.Chat.Id}_records");

            if (records is null)
            {
                return await BotClient.SendTextMessageAsync(
                    chatId: callbackQuery.Message.Chat.Id,
                    text: "Время ответа истекло. Пожалуйста, сгенерируйте новый список!",
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: cancellationToken);
            }

            string productName = string.Empty;

            string pattern = "\"([^\"]*)\"";
            Match match = Regex.Match(callbackQuery.Message!.Text!, pattern);
            if (match.Success) 
            { 
                productName = match.Groups[1].Value;
            }

            var productRecord = records!.FirstOrDefault(p => p.Product.Equals(productName));
            _cartService.Add(callbackQuery.From.Id, productRecord!);

            return await BotClient.EditMessageTextAsync(
                chatId: callbackQuery.Message.Chat.Id,
                messageId: callbackQuery.Message.MessageId,
                text: $"{productName} успешно добавлен в вашу корзину!",
                replyMarkup: InlineKeyboardHelper.CreateSelectNotFoundInlineKeyboard(),
                cancellationToken: cancellationToken);
        }
    }
}
