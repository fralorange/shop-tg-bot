using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreelanceBotBase.Infrastructure.DataAccess.Contexts.DeliveryPoint.Configuration
{
    /// <summary>
    /// Delivery point EF Core configuration.
    /// </summary>
    public class DeliveryPointConfiguration : IEntityTypeConfiguration<Domain.DeliveryPoint.DeliveryPoint>
    {
        public void Configure(EntityTypeBuilder<Domain.DeliveryPoint.DeliveryPoint> builder)
        {
            builder.ToTable(nameof(Domain.DeliveryPoint.DeliveryPoint));

            builder.HasKey(dp => dp.Id);

            builder.Property(dp => dp.Id).ValueGeneratedOnAdd();
            builder.Property(dp => dp.Name).IsRequired();
            builder.Property(dp => dp.Location).IsRequired();
            builder.Property(dp => dp.ManagerId);

            builder.HasOne(dp => dp.Manager)
                .WithOne(u => u.DeliveryPoint)
                .HasForeignKey<Domain.DeliveryPoint.DeliveryPoint>(dp => dp.ManagerId);
        }
    }
}
