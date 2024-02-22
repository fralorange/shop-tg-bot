using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Commands.Interface;
using FreelanceBotBase.Bot.Helpers;
using FreelanceBotBase.Bot.Services.Cart;
using FreelanceBotBase.Domain.State;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBotBase.Bot.Commands.Callback.Delete
{
    public class DeleteCallbackCommand : CallbackCommandBase, ICallbackCommandWithInput
    {
        private readonly ICartService _cartService;
        private readonly BotState _botState;

        public DeleteCallbackCommand(ITelegramBotClient botClient, ICartService cartService, BotState botState) : base(botClient)
        {
            _cartService = cartService;
            _botState = botState;
        }

        public override async Task<Message> HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            _botState.CurrentState = BotState.State.AwaitingInput;
            _botState.AwaitingInputState = BotState.InputState.Delete;

            return await BotClient.EditMessageTextAsync(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message.MessageId,
                text: "Введите полное название продукта, который желаете удалить...",
                replyMarkup: null,
                cancellationToken: cancellationToken);
        }

        public async Task<Message> HandleUserInput(string userInput, Message message, CancellationToken cancellationToken)
        {
            _botState.CurrentState = BotState.State.Default;
            _botState.AwaitingInputState = BotState.InputState.None;

            var deleted = _cartService.Delete(message.From!.Id, userInput);
            var response = (deleted) ? $"{userInput} был успешно удалён из корзины!" : "Неправильно набрано название удаляемого объекта!";
            var inlineKeyboard = InlineKeyboardHelper.CreateBackInlineKeyboard();

            return await BotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: response,
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
    }
}
