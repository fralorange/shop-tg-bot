﻿using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Bot.Commands.Interface;
using FreelanceBotBase.Bot.Helpers;
using FreelanceBotBase.Domain.State;
using FreelanceBotBase.Infrastructure.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FreelanceBotBase.Bot.Commands.Callback.Search
{
    /// <summary>
    /// Callback command that allows filter products records.
    /// </summary>
    public class SearchCallbackCommand : CallbackCommandBase, ICallbackCommandWithInput
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
        /// Creates new search callback command.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="cache"></param>
        /// <param name="botState"></param>
        public SearchCallbackCommand(ITelegramBotClient client, IMemoryCache cache, BotState botState) : base(client)
        {
            _cache = cache;
            _botState = botState;
        }

        public async override Task<Message> HandleCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            _botState.CurrentState = BotState.State.AwaitingInput;
            _botState.AwaitingInputState = BotState.InputState.Search;

            return await BotClient.EditMessageTextAsync(
                chatId: callbackQuery.Message!.Chat.Id,
                messageId: callbackQuery.Message.MessageId,
                text: "Введите в поле для сообщений название продукта...",
                replyMarkup: null,
                cancellationToken: cancellationToken);
        }

        public async Task<Message> HandleUserInput(string userInput, Message message, CancellationToken cancellationToken)
        {
            _botState.CurrentState = BotState.State.Default;
            _botState.AwaitingInputState = BotState.InputState.None;

            var chatId = message.Chat.Id;

            var cacheHelper = new CacheHelper(_cache);
            var records = cacheHelper.GetRecords(chatId, out int currentPage, userInput);

            if (records is null)
            {
                return await BotClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Время ответа истекло. Пожалуйста, сгенерируйте новый список!",
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: cancellationToken);
            }

            var paginatedRecords = PaginationHelper.SplitByPages(records, 10, currentPage);
            var output = PaginationHelper.Format(paginatedRecords);
            var inlineKeyboard = KeyboardHelper.CreateSearchInlineKeyboard();

            return await BotClient.SendTextMessageAsync(
                chatId: chatId,
                text: output,
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }
    }
}
