using DeliveryPointEntity = FreelanceBotBase.Domain.DeliveryPoint.DeliveryPoint;

namespace FreelanceBotBase.Infrastructure.DataAccess.Contexts.DeliveryPoint.Repositories
{
    /// <summary>
    /// Delivery point repository.
    /// </summary>
    public interface IDeliveryPointRepository
    {
        /// <summary>
        /// Gets all delivery point entities.
        /// </summary>
        /// <returns></returns>
        Task<IReadOnlyCollection<DeliveryPointEntity>> GetAll();
        /// <summary>
        /// Gets delivery point by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DeliveryPointEntity> GetByIdAsync(long id);
        /// <summary>
        /// Adds new delivery point.
        /// </summary>
        /// <param name="deliveryPoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddAsync(DeliveryPointEntity deliveryPoint, CancellationToken cancellationToken);
        /// <summary>
        /// Deletes delivery point.
        /// </summary>
        /// <param name="deliveryPoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(DeliveryPointEntity deliveryPoint, CancellationToken cancellationToken);
    }
}
