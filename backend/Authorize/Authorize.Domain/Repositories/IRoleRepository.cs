
using Authorize.Domain.Entities;

namespace Authorize.Domain.Repositories
{
    public interface IRoleRepository
    {
        Task<Role> AddRole(Role role);

        Task<Role?> GetRole(string roleName);

        IEnumerable<Role> GetRoles();
    }
}
