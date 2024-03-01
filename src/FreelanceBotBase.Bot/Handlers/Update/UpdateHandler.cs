using FreelanceBotBase.Bot.Commands.Factory;
using FreelanceBotBase.Bot.Commands.Interface;
using FreelanceBotBase.Bot.Services.State;
using FreelanceBotBase.Domain.State;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;

namespace FreelanceBotBase.Bot.Handlers.Update
{
    /// <inheritdoc cref="IUpdateHandler"/>
    public class UpdateHandler : IUpdateHandler
    {
        /// <summary>
        /// Telegram Bot Client.
        /// </summary>
        private readonly ITelegramBotClient _botClient;
        /// <summary>
        /// Microsoft Logger.
        /// </summary>
        private readonly ILogger<UpdateHandler> _logger;
        /// <summary>
        /// Bot state service.
        /// </summary>
        private readonly IBotStateService _botStateService;
        /// <summary>
        /// Commands factory.
        /// </summary>
        private readonly CommandFactory _commandFactory;

        /// <summary>
        /// Initializes new update handler.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="logger"></param>
        /// <param name="botStateService"></param>
        /// <param name="commandFactory"></param>
        public UpdateHandler(ITelegramBotClient botClient, ILogger<UpdateHandler> logger, IBotStateService botStateService, CommandFactory commandFactory)
        {
            _botClient = botClient;
            _logger = logger;
            _botStateService = botStateService;
            _commandFactory = commandFactory;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Telegram.Bot.Types.Update update, CancellationToken cancellationToken)
        {
            var handler = update switch
            {
                { Message: { } message } => BotOnMessageReceived(message, cancellationToken),
                { EditedMessage: { } message } => BotOnMessageReceived(message, cancellationToken),
                { CallbackQuery: { } callbackQuery } => BotOnCallbackQueryReceived(callbackQuery, cancellationToken),
                { InlineQuery: { } inlineQuery } => BotOnInlineQueryReceived(inlineQuery, cancellationToken),
                { ChosenInlineResult: { } chosenInlineResult } => BotOnChosenInlineResultReceived(chosenInlineResult, cancellationToken),
                _ => UnknownUpdateHandlerAsync(update, cancellationToken)
            };

            await handler;
        }
        #region Bot Processors
        /// <summary>
        /// Processes messages.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Receive message type: {MessageType}", message.Type);
            if (message.Text is not { } messageText)
                return;

            var botState = _botStateService.GetOrCreateBotState(message.Chat.Id);

            Message sentMessage;
            if (botState.CurrentState == BotState.State.Default)
            {
                ICommand command = _commandFactory.CreateCommand(messageText.Split(' ')[0]);
                sentMessage = await command.ExecuteAsync(message, cancellationToken);
            }
            else
            {
                ICallbackCommandWithInput command = _commandFactory.CreateCallbackCommandWithUserInput(botState.AwaitingInputState, message.Chat.Id);
                sentMessage = await command.HandleUserInput(messageText, message, cancellationToken);
            }

            _logger.LogInformation("The message was sent with id: {SentMessageId}", sentMessage.MessageId);
        }

        /// <summary>
        /// Processes callback queries.
        /// </summary>
        /// <param name="callbackQuery"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received inline keyboard callback from {CallbackQueryId}", callbackQuery.Id);

            var botState = _botStateService.GetOrCreateBotState(callbackQuery.Message!.Chat.Id);

            if (botState.CurrentState == BotState.State.Default)
            {
                ICallbackCommand callbackCommand = _commandFactory.CreateCallbackCommand(callbackQuery.Data!, callbackQuery.Message.Chat.Id);
                Message message = await callbackCommand.HandleCallbackQuery(callbackQuery, cancellationToken);
                _logger.LogInformation("The message was interacted with id: {MessageId}", message.MessageId);
            }
        }

        /// <summary>
        /// Processes inline queries.
        /// </summary>
        /// <param name="inlineQuery"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task BotOnInlineQueryReceived(InlineQuery inlineQuery, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received inline query from {InlineQueryFromId}", inlineQuery.From.Id);

            InlineQueryResult[] results =
            {
                new InlineQueryResultArticle(
                    id: "1",
                    title: "TgBots",
                    inputMessageContent: new InputTextMessageContent("hello"))
            };

            await _botClient.AnswerInlineQueryAsync(
                inlineQueryId: inlineQuery.Id,
                results: results,
                cacheTime: 0,
                isPersonal: true,
                cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Processes chosen inline results.
        /// </summary>
        /// <param name="chosenInlineResult"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task BotOnChosenInlineResultReceived(ChosenInlineResult chosenInlineResult, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received inline result: {ChosedInlineResultId}", chosenInlineResult.ResultId);

            await _botClient.SendTextMessageAsync(
                chatId: chosenInlineResult.From.Id,
                text: $"You chose result with Id: {chosenInlineResult.ResultId}",
                cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Processes unknown updates.
        /// </summary>
        /// <param name="update"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private Task UnknownUpdateHandlerAsync(Telegram.Bot.Types.Update update, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
            return Task.CompletedTask;
        }

        #endregion
        public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogInformation("HandleError: {ErrorMessage}", ErrorMessage);

            // Cooldown in case of network troubles.
            if (exception is RequestException)
                await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        }
    }
}
