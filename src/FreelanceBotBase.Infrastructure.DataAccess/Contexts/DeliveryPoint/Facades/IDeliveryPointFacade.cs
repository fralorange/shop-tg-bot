namespace FreelanceBotBase.Infrastructure.DataAccess.Contexts.DeliveryPoint.Facades
{
    /// <summary>
    /// Facade to interpet external system methods.
    /// </summary>
    public interface IDeliveryPointFacade
    {
        /// <summary>
        /// Sets manager to delivery point.
        /// </summary>
        /// <param name="deliveryPointId"></param>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SetManagerAsync(long deliveryPointId, long? userId, CancellationToken cancellationToken);
    }
}
