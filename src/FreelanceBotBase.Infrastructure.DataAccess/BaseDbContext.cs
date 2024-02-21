using Microsoft.EntityFrameworkCore;

namespace FreelanceBotBase.Infrastructure.DataAccess
{
    /// <summary>
    /// Database context.
    /// </summary>
    public class BaseDbContext : DbContext
    {
        /// <inheritdoc cref="DbContext"/>
        public BaseDbContext(DbContextOptions options) : base(options) { }

        /// <inheritdoc"/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
