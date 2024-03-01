using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Helpers;
using FreelanceBotBase.Bot.Services.Cart;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Callback.Back
{
    /// <summary>
    /// Callback command that returns user back.
    /// </summary>
    public class BackCartCallbackCommand : CallbackCommandBase
    {
        /// <summary>
        /// Cart service to manage user carts.
        /// </summary>
        private readonly ICartService _cartService;

        /// <summary>
        /// Creates back callback command.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="cartService"></param>
        public BackCartCallbackCommand(ITelegramBotClient botClient, ICartService cartService) : base(botClient)
            => _cartService = cartService;

        /// <inheritdoc/>
        public override async Task<Message> HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            // return await new GetCartCommand(BotClient, _cartService).ExecuteAsync(callbackQuery.Message!, cancellationToken);
            // keep in mind maybe it will come useful one day ^
            var cart = _cartService.Get(callbackQuery.From.Id)!;

            string output = cart == null
                ? "Корзина пустая!"
                : "Корзина:\n" + PaginationHelper.Format(cart);

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
