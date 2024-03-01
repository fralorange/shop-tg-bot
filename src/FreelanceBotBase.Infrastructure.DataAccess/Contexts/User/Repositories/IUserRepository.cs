using UserEntity = FreelanceBotBase.Domain.User.User;

namespace FreelanceBotBase.Infrastructure.DataAccess.Contexts.User.Repositories
{
    /// <summary>
    /// User repository.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns></returns>
        Task<IReadOnlyCollection<UserEntity>> GetAll();
        /// <summary>
        /// Gets user by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserEntity> GetByIdAsync(long id);
        /// <summary>
        /// Adds user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddAsync(UserEntity user, CancellationToken cancellationToken);
        /// <summary>
        /// Deletes user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(UserEntity user, CancellationToken cancellationToken);
    }
}
