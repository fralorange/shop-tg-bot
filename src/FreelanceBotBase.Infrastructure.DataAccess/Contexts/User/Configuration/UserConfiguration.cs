using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreelanceBotBase.Infrastructure.DataAccess.Contexts.User.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<Domain.User.User>
    {
        public void Configure(EntityTypeBuilder<Domain.User.User> builder)
        {
            builder.ToTable(nameof(Domain.User.User));

            builder.HasKey(u => u.UserId);

            builder.Property(u => u.UserId).IsRequired();
            builder.Property(u => u.UserRole).IsRequired();
            builder.Property(u => u.DeliveryPointId);

            builder.HasOne(u => u.DeliveryPoint)
                .WithOne(dp => dp.Manager)
                .HasForeignKey<Domain.User.User>(u => u.DeliveryPointId);
        }
    }
}
