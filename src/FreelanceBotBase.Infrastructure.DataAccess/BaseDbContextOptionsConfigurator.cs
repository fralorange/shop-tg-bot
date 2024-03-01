using FreelanceBotBase.Infrastructure.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FreelanceBotBase.Infrastructure.DataAccess
{
    /// <inheritdoc cref="IDbContextOptionsConfigurator{TContext}"/>
    public class BaseDbContextOptionsConfigurator : IDbContextOptionsConfigurator<BaseDbContext>
    {
        private const string ConnectionStringName = "POSTGRES_DB";
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Creates new base db context configuration.
        /// </summary>
        /// <param name="configuration"></param>
        public BaseDbContextOptionsConfigurator(IConfiguration configuration)
            => _configuration = configuration;

        public void Configure(DbContextOptionsBuilder<BaseDbContext> options)
        {
            var connectionString = _configuration.GetConnectionString(ConnectionStringName);

            options
                .UseNpgsql(connectionString)
                .UseLazyLoadingProxies();
        }
    }
}
