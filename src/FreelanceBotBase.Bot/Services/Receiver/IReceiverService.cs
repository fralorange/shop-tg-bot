namespace FreelanceBotBase.Bot.Services.Receiver
{
    /// <summary>
    /// Provides a service for receiving updates from Telegram.
    /// </summary>
    public interface IReceiverService
    {
        /// <summary>
        /// Receives updates from Telegram asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ReceiveAsync(CancellationToken cancellationToken);
    }
}
