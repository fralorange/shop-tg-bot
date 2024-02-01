using FreelanceBotBase.Bot.Services.Receiver;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FreelanceBotBase.Bot.Services.Polling
{
    /// <summary>
    /// A class to compose Polling backgrounds service and Receiver service classes.
    /// </summary>
    /// <typeparam name="TReceiverService">Receiver implementation.</typeparam>
    public abstract class PollingServiceBase<TReceiverService> : BackgroundService
        where TReceiverService : IReceiverService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        /// <summary>
        /// Creates new instance of Polling service.
        /// </summary>
        /// <param name="serviceProvider">Service Provider.</param>
        /// <param name="logger">Logger.</param>
        internal PollingServiceBase(IServiceProvider serviceProvider, ILogger logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting polling service");

            await DoWork(cancellationToken);
        }

        private async Task DoWork(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var receiver = scope.ServiceProvider.GetService<TReceiverService>();

                    await receiver!.ReceiveAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Polling failed with exception {Exception}", ex);
                    
                    await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                }
            }
        }
    }
}
