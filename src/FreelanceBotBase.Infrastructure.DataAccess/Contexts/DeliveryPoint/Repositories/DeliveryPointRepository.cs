using FreelanceBotBase.Infrastructure.Repository;

using DeliveryPointEntity = FreelanceBotBase.Domain.DeliveryPoint.DeliveryPoint;

namespace FreelanceBotBase.Infrastructure.DataAccess.Contexts.DeliveryPoint.Repositories
{
    /// <inheritdoc cref="IDeliveryPointRepository"/>
    public class DeliveryPointRepository : IDeliveryPointRepository
    {
        private readonly IRepository<DeliveryPointEntity> _repository;

        /// <summary>
        /// Initializes. new delivery point repository.
        /// </summary>
        /// <param name="repository"></param>
        public DeliveryPointRepository(IRepository<DeliveryPointEntity> repository)
            => _repository = repository;


        public Task<IReadOnlyCollection<DeliveryPointEntity>> GetAll()
        {
            IReadOnlyCollection<DeliveryPointEntity> collection = _repository.GetAll().ToList().AsReadOnly();
            return Task.FromResult(collection);
        }

        public Task<DeliveryPointEntity> GetByIdAsync(long id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task AddAsync(DeliveryPointEntity deliveryPoint, CancellationToken cancellationToken)
        {
            return _repository.AddAsync(deliveryPoint, cancellationToken);
        }

        public Task<bool> DeleteAsync(DeliveryPointEntity deliveryPoint, CancellationToken cancellationToken)
        {
            return _repository.DeleteAsync(deliveryPoint, cancellationToken);
        }
    }
}
