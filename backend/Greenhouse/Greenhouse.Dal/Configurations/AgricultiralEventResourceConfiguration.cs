using Greenhouse.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greenhouse.Dal.Configurations
{
    public class AgricultiralEventResourceConfiguration :
        IEntityTypeConfiguration<AgricultiralEventResource>
    {
        public void Configure(
            EntityTypeBuilder<AgricultiralEventResource> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Event)
                .WithMany(x => x.Resources)
                .HasForeignKey(x => x.EventId);

            builder.HasOne(x => x.Resource)
                .WithOne(x => x.AgricultiralEventResource)
                .HasForeignKey<Resource>(x => x.AgricultiralEventResourceId);

            builder.Property(x => x.PlannedAmount)
                .IsRequired();

            builder.ToTable(x => x
            .HasCheckConstraint(
                "CK_EventResources_PlannedAmount",
                "\"PlannedAmount\" >= 0"));

            builder.ToTable(x => x
            .HasCheckConstraint(
                "CK_EventResources_ActualAmount",
                "\"ActualAmount\" >= 0"));
        }
    }
}
