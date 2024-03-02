using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Commands.Interface;
using FreelanceBotBase.Bot.Helpers;
using FreelanceBotBase.Contracts.Product;
using FreelanceBotBase.Domain.State;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBotBase.Bot.Commands.Callback.Select
{
    /// <summary>
    /// Callback command that selects products.
    /// </summary>
    public class SelectCallbackCommand : CallbackCommandBase, ICallbackCommandWithInput
    {
        /// <summary>
        /// Memory cache.
        /// </summary>
        private readonly IMemoryCache _cache;
        /// <summary>
        /// Bot state.
        /// </summary>
        private readonly BotState _botState;

        /// <summary>
        /// Creates new select callback command.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="cache"></param>
        /// <param name="botState"></param>
        public SelectCallbackCommand(ITelegramBotClient client, IMemoryCache cache, BotState botState) : base(client)
        {
            _cache = cache;
            _botState = botState;
        }

        public async override Task<Message> HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            _botState.CurrentState = BotState.State.AwaitingInput;
            _botState.AwaitingInputState = BotState.InputState.Selection;

            return await BotClient.EditMessageTextAsync(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message.MessageId,
                text: "Введите название продукта, который желаете выбрать...",
                replyMarkup: null,
                cancellationToken: cancellationToken);
        }

        public async Task<Message> HandleUserInput(string userInput, Message message, CancellationToken cancellationToken)
        {
            _botState.CurrentState = BotState.State.Default;
            _botState.AwaitingInputState = BotState.InputState.None;

            var chatId = message.Chat.Id;

            var records = _cache.Get<IEnumerable<ProductDto>>($"{chatId}_records");

            if (records is null)
            {
                return await BotClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Время ответа истекло. Пожалуйста, сгенерируйте новый список!",
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: cancellationToken);
            }

            var product = records.FirstOrDefault(r => r.Product.Contains(userInput));

            var keyboard = product != null
                ? KeyboardHelper.CreateSelectInlineKeyboard()
                : KeyboardHelper.CreateResetInlineKeyboard();
            var output = product != null ?
                $"Вы выбрали: \"{product.Product}\"\nПодтвердите свой выбор!"
                : "Не найдено!";

            return await BotClient.SendTextMessageAsync(
                chatId: chatId,
                text: output,
                replyMarkup: keyboard,
                cancellationToken: cancellationToken);
        }
    }
}
