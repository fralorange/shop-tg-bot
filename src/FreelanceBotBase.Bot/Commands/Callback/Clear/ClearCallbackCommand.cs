using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Helpers;
using FreelanceBotBase.Bot.Services.Cart;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Callback.Clear
{
    public class ClearCallbackCommand : CallbackCommandBase
    {
        private readonly ICartService _cartService;

        public ClearCallbackCommand(ITelegramBotClient botClient, ICartService cartService) : base(botClient)
            => _cartService = cartService;

        public override async Task<Message> HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            var clearState = _cartService.Delete(callbackQuery.From.Id);
            var response = (clearState) ? "Корзина успешно очищена!" : "Очистка корзины не удалась по неизвестной причине!";

            return await BotClient.EditMessageTextAsync(
                 chatId: callbackQuery.Message!.Chat.Id,
                 messageId: callbackQuery.Message.MessageId,
                 text: response,
                 replyMarkup: null,
                 cancellationToken: cancellationToken);
        }
    }
}
