using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FreelanceBotBase.Infrastructure.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbContext DbContext { get; }
        protected DbSet<TEntity> DbSet { get; }

        public Repository(DbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = DbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll()
        {
            return DbSet;
        }

        public IQueryable<TEntity> GetAllFiltered(Expression<Func<TEntity, bool>> predicate)
        {
            ArgumentNullException.ThrowIfNull(predicate);
            
            return DbSet.Where(predicate);
        }

        public async Task<TEntity> GetByIdAsync(long id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<TEntity> GetByPredicateAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public async Task AddAsync(TEntity model, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(model);

            await DbSet.AddAsync(model, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(TEntity model, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(model);

            DbSet.Update(model);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> DeleteAsync(TEntity model, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(model);
            DbSet.Remove(model);
            int affectedRows = await DbContext.SaveChangesAsync(cancellationToken);
            return affectedRows > 0;
        }
    }
}
