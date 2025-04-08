using Authorize.Domain.Entities;
using Authorize.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Authorize.Dal.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext context;

        public RoleRepository(
            AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Role> AddRole(Role role)
        {
            var roleEntity = await context.Roles
                .AddAsync(role);
        
            return roleEntity.Entity;
        }

        public async Task<Role?> GetRole(string roleName)
        {
            return await context.Roles
                .FirstOrDefaultAsync(r => r.Name == roleName);
        }

        public IEnumerable<Role> GetRoles()
        {
            return context
                .Roles
                .AsNoTracking()
                .AsEnumerable();
        }
    }
}
