using Greenhouse.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greenhouse.Dal.Configurations
{
    public class AgricultiralEventConfiguration :
        IEntityTypeConfiguration<AgricultiralEvent>
    {
        public void Configure(EntityTypeBuilder<AgricultiralEvent> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Greenhouse)
                .WithMany(x => x.Events)
                .HasForeignKey(x =>  x.GreenhouseId)
                .IsRequired();

            builder.HasOne(x => x.Employee)
                .WithMany(x => x.Events)
                .HasForeignKey(x => x.EmploeeId)
                .IsRequired();

            builder.HasMany(x => x.Resources)
                .WithOne(x => x.Event)
                .HasForeignKey(x => x.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Name)
                .IsRequired();

            builder.Property(x => x.EventType)
                .IsRequired();

            builder.Property(x => x.PlannedDate)
                .IsRequired();
        }
    }
}
