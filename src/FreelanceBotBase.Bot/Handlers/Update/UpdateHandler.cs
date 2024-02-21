using FreelanceBotBase.Bot.Commands.Factory;
using FreelanceBotBase.Bot.Commands.Interface;
using FreelanceBotBase.Bot.Services.BotState;
using FreelanceBotBase.Domain.States;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;

namespace FreelanceBotBase.Bot.Handlers.Update
{
    public class UpdateHandler : IUpdateHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<UpdateHandler> _logger;
        private readonly IBotStateService _botStateService;
        private readonly CommandFactory _commandFactory;

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

        private async Task BotOnChosenInlineResultReceived(ChosenInlineResult chosenInlineResult, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received inline result: {ChosedInlineResultId}", chosenInlineResult.ResultId);

            await _botClient.SendTextMessageAsync(
                chatId: chosenInlineResult.From.Id,
                text: $"You chose result with Id: {chosenInlineResult.ResultId}",
                cancellationToken: cancellationToken);
        }

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
