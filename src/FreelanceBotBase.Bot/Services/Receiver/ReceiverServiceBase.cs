using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace FreelanceBotBase.Bot.Services.Receiver
{
    /// <summary>
    /// A class to compose Receiver Service and Update Handler classes.
    /// </summary>
    /// <typeparam name="TUpdateHandler">Update Handler to use in Update Receiver.</typeparam>
    public abstract class ReceiverServiceBase<TUpdateHandler> : IReceiverService
        where TUpdateHandler : IUpdateHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IUpdateHandler _updateHandler;
        private readonly ILogger<ReceiverServiceBase<TUpdateHandler>> _logger;
        private readonly ReceiverOptions _receiverOptions;

        /// <summary>
        /// Creates new instance of Receiver Service.
        /// </summary>
        /// <param name="botClient">Telegram bot client instance.</param>
        /// <param name="updateHandler">Update handler.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="receiverOptions">Options used to properly set receiver.</param>
        internal ReceiverServiceBase(
            ITelegramBotClient botClient,
            IUpdateHandler updateHandler,
            ILogger<ReceiverServiceBase<TUpdateHandler>> logger,
            IOptions<ReceiverOptions> receiverOptions)
        {
            _botClient = botClient;
            _updateHandler = updateHandler;
            _logger = logger;
            _receiverOptions = receiverOptions.Value;
        }

        /// <inheritdoc/>
        public async Task ReceiveAsync(CancellationToken cancellationToken)
        {
            var me = await _botClient.GetMeAsync(cancellationToken);
            _logger.LogInformation("Start receiving updates for {BotName}", me.Username ?? "My Awesome Bot");

            await _botClient.ReceiveAsync(
                updateHandler: _updateHandler,
                receiverOptions: _receiverOptions,
                cancellationToken: cancellationToken);
        }
    }
}
