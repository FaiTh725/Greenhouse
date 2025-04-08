using Authorize.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorize.Dal.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(x => x.Name);

            builder.HasMany(x => x.Users)
                .WithOne(x => x.Role)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
