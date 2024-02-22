using UserEntity = FreelanceBotBase.Domain.User.User;

namespace FreelanceBotBase.Infrastructure.DataAccess.Contexts.User.Repositories
{
    public interface IUserRepository
    {
        Task<IReadOnlyCollection<UserEntity>> GetAll();
        Task<UserEntity> GetByIdAsync(long id);
        Task AddAsync(UserEntity user, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(UserEntity user, CancellationToken cancellationToken);
    }
}
