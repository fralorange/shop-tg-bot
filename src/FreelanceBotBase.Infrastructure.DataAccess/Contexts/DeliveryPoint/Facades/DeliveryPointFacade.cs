﻿using FreelanceBotBase.Infrastructure.Repository;
using DeliveryPointEntity = FreelanceBotBase.Domain.DeliveryPoint.DeliveryPoint;

namespace FreelanceBotBase.Infrastructure.DataAccess.Contexts.DeliveryPoint.Facades
{
    /// <inheritdoc cref="IDeliveryPointFacade"/>
    public class DeliveryPointFacade : IDeliveryPointFacade
    {
        private readonly IRepository<DeliveryPointEntity> _repository;

        /// <summary>
        /// Initializes delivery point facade.
        /// </summary>
        /// <param name="repository"></param>
        public DeliveryPointFacade(IRepository<DeliveryPointEntity> repository)
            => _repository = repository;

        public async Task SetManagerAsync(long deliveryPointId, long? userId, CancellationToken cancellationToken)
        {
            var deliveryPoint = await _repository.GetByIdAsync(deliveryPointId);
            ArgumentNullException.ThrowIfNull(deliveryPoint, nameof(deliveryPoint));

            deliveryPoint.ManagerId = userId;
            await _repository.UpdateAsync(deliveryPoint, cancellationToken);
        }
    }
}
