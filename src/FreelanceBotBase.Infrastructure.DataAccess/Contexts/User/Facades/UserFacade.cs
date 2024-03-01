using FreelanceBotBase.Infrastructure.Repository;

using UserEntity = FreelanceBotBase.Domain.User.User;

namespace FreelanceBotBase.Infrastructure.DataAccess.Contexts.User.Facades
{
    /// <inheritdoc cref="IUserFacade"/>
    public class UserFacade : IUserFacade
    {
        private readonly IRepository<UserEntity> _repository;

        /// <summary>
        /// Initializes user facade.
        /// </summary>
        /// <param name="repository"></param>
        public UserFacade(IRepository<UserEntity> repository)
            => _repository = repository;

        public async Task AssignDeliveryPointAsync(long userId, long? deliveryPointId, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(userId);
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            if (user.UserRole != UserEntity.Role.Manager)
                throw new ArgumentException("User not a manager");

            user.DeliveryPointId = deliveryPointId;
            await _repository.UpdateAsync(user, cancellationToken);
        }
    }
}
