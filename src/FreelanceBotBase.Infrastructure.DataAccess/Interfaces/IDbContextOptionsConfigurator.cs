using Microsoft.EntityFrameworkCore;

namespace FreelanceBotBase.Infrastructure.DataAccess.Interfaces
{
    /// <summary>
    /// <see cref="DbContext"/> configurator.
    /// </summary>
    /// <typeparam name="TContext">Context.</typeparam>
    public interface IDbContextOptionsConfigurator<TContext> where TContext : DbContext
    {
        /// <summary>
        /// Configures database context.
        /// </summary>
        /// <param name="options">Database context options.</param>
        void Configure(DbContextOptionsBuilder<TContext> options);
    }
}
