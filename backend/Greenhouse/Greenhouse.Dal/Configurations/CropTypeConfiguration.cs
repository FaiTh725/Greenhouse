using Greenhouse.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greenhouse.Dal.Configurations
{
    public class CropTypeConfiguration :
        IEntityTypeConfiguration<CropType>
    {
        public void Configure(EntityTypeBuilder<CropType> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired();

            builder.HasOne(x => x.Greenhouse)
                .WithOne(x => x.CropType)
                .HasForeignKey<CropType>(x => x.GreenhouseId);
        }
    }
}
