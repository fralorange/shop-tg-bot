using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Commands.Text.GetCart;
using FreelanceBotBase.Bot.Helpers;
using FreelanceBotBase.Bot.Services.Cart;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Callback.Back
{
    public class BackCartCallbackCommand : CallbackCommandBase
    {
        private readonly ICartService _cartService;

        public BackCartCallbackCommand(ITelegramBotClient botClient, ICartService cartService) : base(botClient)
            => _cartService = cartService;

        public override async Task<Message> HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            // return await new GetCartCommand(BotClient, _cartService).ExecuteAsync(callbackQuery.Message!, cancellationToken);
            // keep in mind maybe it will come useful one day ^
            var cart = _cartService.Get(callbackQuery.From.Id)!;

            string output = cart == null
                ? "Корзина пустая!"
                : "Корзина:\n" + PaginationHelper.FormatProductRecords(cart);

            var inlineKeyboard = (cart == null) ? null : InlineKeyboardHelper.CreateCartInlineKeyboard();

            return await BotClient.EditMessageTextAsync(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message.MessageId,
                text: output,
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
    }
}
