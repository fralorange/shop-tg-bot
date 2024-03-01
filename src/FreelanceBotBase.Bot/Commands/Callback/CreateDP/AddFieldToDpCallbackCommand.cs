using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Commands.Interface;
using FreelanceBotBase.Bot.Helpers;
using FreelanceBotBase.Domain.State;
using Microsoft.Extensions.Caching.Memory;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBotBase.Bot.Commands.Callback.CreateDP
{
    public class AddFieldToDpCallbackCommand : CallbackCommandBase, ICallbackCommandWithInput
    {
        private readonly BotState _botState;
        private readonly IMemoryCache _cache;
        private readonly string _cacheKey;
        private readonly string _field;

        public AddFieldToDpCallbackCommand(ITelegramBotClient botClient, BotState botState, IMemoryCache cache, string cacheKey, string field) 
            : base(botClient)
        {
            _botState = botState;
            _cache = cache;
            _cacheKey = cacheKey;
            _field = field;
        }

        public override Task<Message> HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            _botState.CurrentState = BotState.State.AwaitingInput;
            _botState.AwaitingInputState = (_field == "Название") ? BotState.InputState.EditingDpName : BotState.InputState.EditingDpLocation;

            _cache.Set($"{callbackQuery.From.Id}_{_cacheKey}", callbackQuery.Message!.Text, TimeSpan.FromMinutes(5));

            BotClient.EditMessageTextAsync(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message.MessageId,
                text: callbackQuery.Message.Text!,
                replyMarkup: null,
                cancellationToken: cancellationToken);

            return BotClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: $"Введите новое поле \"{_field}\" Пункта выдачи.",
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
        }

        public Task<Message> HandleUserInput(string userInput, Message message, CancellationToken cancellationToken)
        {
            _botState.CurrentState = BotState.State.Default;
            _botState.AwaitingInputState = BotState.InputState.None;

            var text = _cache.Get<string>($"{message.From!.Id}_{_cacheKey}")!;

            if (text is null)
            {
                return BotClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Время создания Пункта выдачи истекло!",
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: cancellationToken);
            }

            var inlineKeyboard = InlineKeyboardHelper.CreateAddNewDpInlineKeyboard();

            if (!Regex.IsMatch(userInput, @"^[а-яА-Яa-zA-Z0-9\s.,]+$") || userInput.Contains('\n'))
            {
                BotClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Не используйте специальные символы и пишите всё на одной строке!",
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: cancellationToken);

                return BotClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: text,
                    replyMarkup: inlineKeyboard,
                    cancellationToken: cancellationToken);
            }

            string newText = Regex.Replace(text, $"(?m)^({_field}: )(.*)(\n)?", $"$1{userInput}$3");

            return BotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: newText,
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
    }
}
