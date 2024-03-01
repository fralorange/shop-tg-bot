namespace FreelanceBotBase.Infrastructure.DataAccess.Contexts.DeliveryPoint.Facades
{
    public interface IDeliveryPointFacade
    {
        Task SetManagerAsync(long deliveryPointId, long? userId, CancellationToken cancellationToken);
    }
}
