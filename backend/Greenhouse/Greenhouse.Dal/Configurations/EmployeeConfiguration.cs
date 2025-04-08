using Greenhouse.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Greenhouse.Dal.Configurations
{
    public class EmployeeConfiguration :
        IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Email)
                .IsUnique();

            builder.HasMany(x => x.Events)
                .WithOne(x => x.Employee)
                .HasForeignKey(x => x.EmploeeId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
