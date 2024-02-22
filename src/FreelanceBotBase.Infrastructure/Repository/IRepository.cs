using System.Linq.Expressions;

namespace FreelanceBotBase.Infrastructure.Repository
{
    /// <summary>
    /// Base repository.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// Gets all filtered entities.
        /// </summary>
        /// <param name="predicate">Predicate.</param>
        /// <returns></returns>
        IQueryable<TEntity> GetAllFiltered(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets entity by id.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(long id);

        /// <summary>
        /// Gets entity by predicate.
        /// </summary>
        /// <param name="predicate">Predicate.</param>
        /// <returns></returns>
        Task<TEntity> GetByPredicateAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Adds model.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddAsync(TEntity model, CancellationToken cancellationToken);

        /// <summary>
        /// Edits model.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateAsync(TEntity model, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(TEntity model, CancellationToken cancellationToken);
    }
}
