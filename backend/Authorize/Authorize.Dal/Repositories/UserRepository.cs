using Authorize.Domain.Entities;
using Authorize.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Authorize.Dal.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext context;

        public UserRepository(
            AppDbContext context)
        {
            this.context = context;
        }

        public async Task ActiveUsers(List<User> users)
        {
            var userEmailsQuery = users
                .Select(x => x.Email);

            await context.Users
                .Where(x => userEmailsQuery.Any(email => email == x.Email))
                .ExecuteUpdateAsync(x => x
                    .SetProperty(p => p.IsActive, true));
        }

        public async Task<User> AddUser(User user)
        {
            var userEntity = await context.Users
                .AddAsync(user);

            return userEntity.Entity;
        }

        public async Task<User?> GetActiveUser(string email)
        {
            return await context.Users
                .Include(user => user.Role)
                .FirstOrDefaultAsync(x => x.Email == email && 
                    x.IsActive == true);
        }

        public async Task<IEnumerable<User>> GetUnconfirmedUsers()
        {
            return await context.Users
                .Where(x => x.IsActive == false)
                .Include(user => user.Role)
                .ToListAsync();
        }

        public async Task<User?> GetUser(string email)
        {
            return await context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUser(long id)
        {
            return await context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
