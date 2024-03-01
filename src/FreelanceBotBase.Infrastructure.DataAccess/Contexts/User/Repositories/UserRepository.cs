using FreelanceBotBase.Infrastructure.Repository;
using UserEntity = FreelanceBotBase.Domain.User.User;

namespace FreelanceBotBase.Infrastructure.DataAccess.Contexts.User.Repositories
{
    /// <inheritdoc cref="IUserRepository"/>
    public class UserRepository : IUserRepository
    {
        private readonly IRepository<UserEntity> _repository;

        /// <summary>
        /// Initializes user repository.
        /// </summary>
        /// <param name="repository"></param>
        public UserRepository(IRepository<UserEntity> repository)
            => _repository = repository;

        public Task<IReadOnlyCollection<UserEntity>> GetAll()
        {
            IReadOnlyCollection<UserEntity> collection = _repository.GetAll().ToList().AsReadOnly();
            return Task.FromResult(collection);
        }

        public Task<UserEntity> GetByIdAsync(long id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task AddAsync(UserEntity user, CancellationToken cancellationToken)
        {
            return _repository.AddAsync(user, cancellationToken);
        }

        public Task<bool> DeleteAsync(UserEntity user, CancellationToken cancellationToken)
        {
            return _repository.DeleteAsync(user, cancellationToken);
        }
    }
}
