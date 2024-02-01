using FreelanceBotBase.Bot.Handlers.Update;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace FreelanceBotBase.Bot.Services.Receiver
{
    public class ReceiverService : ReceiverServiceBase<UpdateHandler>
    {
        public ReceiverService(
            ITelegramBotClient botClient,
            UpdateHandler updateHandler,
            ILogger<ReceiverServiceBase<UpdateHandler>> logger,
            IOptions<ReceiverOptions> receiverOptions)
            : base(botClient, updateHandler, logger, receiverOptions)
        {
        }
    }
}
