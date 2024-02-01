using FreelanceBotBase.Bot.Services.Receiver;
using Microsoft.Extensions.Logging;

namespace FreelanceBotBase.Bot.Services.Polling
{
    /// <inheritdoc/>
    public class PollingService : PollingServiceBase<ReceiverService>
    {
        public PollingService(IServiceProvider serviceProvider, ILogger<PollingService> logger)
            :base(serviceProvider, logger)
        {
        }
    }
}
