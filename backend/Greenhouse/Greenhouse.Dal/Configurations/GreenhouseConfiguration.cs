using Greenhouse.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greenhouse.Dal.Configurations
{
    public class GreenhouseConfiguration : IEntityTypeConfiguration<GreenhouseEntity>
    {
        public void Configure(EntityTypeBuilder<GreenhouseEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.CropType)
                .WithOne(x => x.Greenhouse)
                .HasForeignKey<CropType>(x => x.GreenhouseId)
                .IsRequired();

            builder.HasMany(x => x.Events)
                .WithOne(x => x.Greenhouse)
                .HasForeignKey(x => x.GreenhouseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Area)
                .IsRequired();

            builder.Property(x => x.Location)
                .IsRequired();

            builder.ToTable(x => x
            .HasCheckConstraint(
                "CK_Greenhouses_Area", "\"Area\" > 0"));

        }
    }
}
