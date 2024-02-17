using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Helpers;
using FreelanceBotBase.Bot.Services.Cart;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBotBase.Bot.Commands.Text.GetCart
{
    public class GetCartCommand : CommandBase
    {
        private readonly ICartService _cartService;

        public GetCartCommand(ITelegramBotClient botClient, ICartService cartService) : base(botClient)
        {
            _cartService = cartService;
        }

        public override Task<Message> ExecuteAsync(Message message, CancellationToken cancellationToken)
        {
            var cart = _cartService.Get(message.From!.Id)!;
            
            string output = cart == null
                ? "Корзина пустая!"
                : "Корзина:\n" + PaginationHelper.FormatProductRecords(cart);

            return BotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: output,
                replyMarkup: new ReplyKeyboardRemove(), // TO-DO: add controls l8r
                cancellationToken: cancellationToken);
        }
    }
}
