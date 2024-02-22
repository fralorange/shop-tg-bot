using FreelanceBotBase.Infrastructure.DataAccess.Contexts.DeliveryPoint.Configuration;
using FreelanceBotBase.Infrastructure.DataAccess.Contexts.User.Configuration;
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
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new DeliveryPointConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
