using FreelanceBotBase.Infrastructure.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FreelanceBotBase.Infrastructure.DataAccess
{
    /// <inheritdoc cref="IDbInitializer"/>
    public class DbInitializer : IDbInitializer
    {
        private readonly DbContext _context;

        /// <inheritdoc cref="IDbInitializer"/>
        public DbInitializer(DbContext context)
            => _context = context;

        /// <inheritdoc/>
        public void Initialize()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }
    }
}
