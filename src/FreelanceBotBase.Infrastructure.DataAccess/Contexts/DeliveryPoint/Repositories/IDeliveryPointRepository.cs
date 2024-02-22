using DeliveryPointEntity = FreelanceBotBase.Domain.DeliveryPoint.DeliveryPoint;

namespace FreelanceBotBase.Infrastructure.DataAccess.Contexts.DeliveryPoint.Repositories
{
    public interface IDeliveryPointRepository
    {
        Task<IReadOnlyCollection<DeliveryPointEntity>> GetAll();
        Task<DeliveryPointEntity> GetByIdAsync(long id);
        Task AddAsync(DeliveryPointEntity deliveryPoint, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(DeliveryPointEntity deliveryPoint, CancellationToken cancellationToken);
    }
}
