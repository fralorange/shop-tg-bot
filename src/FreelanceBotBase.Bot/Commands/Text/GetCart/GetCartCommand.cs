using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Helpers;
using FreelanceBotBase.Bot.Services.Cart;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Text.GetCart
{
    /// <summary>
    /// Command to get user's cart.
    /// </summary>
    public class GetCartCommand : CommandBase
    {
        /// <summary>
        /// Cart service.
        /// </summary>
        private readonly ICartService _cartService;

        /// <summary>
        /// Creates new get cart command.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="cartService"></param>
        public GetCartCommand(ITelegramBotClient botClient, ICartService cartService) : base(botClient)
        {
            _cartService = cartService;
        }

        public async override Task<Message> ExecuteAsync(Message message, CancellationToken cancellationToken)
        {
            var cart = _cartService.Get(message.From!.Id)!;

            string output = cart == null
                ? "Корзина пустая!"
                : "Корзина:\n" + PaginationHelper.Format(cart);

            var inlineKeyboard = (cart == null ) ? null : InlineKeyboardHelper.CreateCartInlineKeyboard();

            return await BotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: output,
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
    }
}
