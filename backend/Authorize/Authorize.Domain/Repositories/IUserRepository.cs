using Authorize.Domain.Entities;

namespace Authorize.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> AddUser(User user);

        Task<User?> GetUser(string email);

        Task<User?> GetUser(long id);

        Task<User?> GetActiveUser(string email);

        Task ActiveUsers(List<User> users);
    }
}
