using Greenhouse.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greenhouse.Dal.Configurations
{
    public class ResourceConfiguration :
        IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired();

            builder.Property(x => x.Unit)
                .IsRequired();

            builder.Property(x => x.ResourceType)
                .IsRequired();

            builder.HasOne(x => x.AgricultiralEventResource)
                .WithOne(x => x.Resource)
                .HasForeignKey<Resource>(x => x.AgricultiralEventResourceId);
        }
    }
}
