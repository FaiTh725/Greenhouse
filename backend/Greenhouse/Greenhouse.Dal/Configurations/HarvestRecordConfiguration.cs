using Greenhouse.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greenhouse.Dal.Configurations
{
    public class HarvestRecordConfiguration :
        IEntityTypeConfiguration<HarvestRecord>
    {
        public void Configure(EntityTypeBuilder<HarvestRecord> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ResourceName)
                .IsRequired();

            builder.Property(x => x.ActualYield)
                .IsRequired();

            builder.Property(x => x.YieldUnit)
                .IsRequired();

            builder.HasOne(x => x.Event)
                .WithOne()
                .HasForeignKey<HarvestRecord>(x => x.EventId)
                .IsRequired();
        }
    }
}
