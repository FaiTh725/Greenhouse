using Greenhouse.Domain.Entities;
using Greenhouse.Domain.Repositorires;
using Microsoft.EntityFrameworkCore;

namespace Greenhouse.Dal.Repositories
{
    public class EmployeRepository : IEmployeRepository
    {
        private readonly AppDbContext context;

        public EmployeRepository(
            AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Employee> AddEmploye(Employee employee)
        {
            var employeEntity = await context
                .AddAsync(employee);
        
            return employeEntity.Entity;
        }

        public async Task<Employee?> GetEmploye(long id)
        {
            return await context.Employees
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Employee?> GetEmploye(string email)
        {
            return await context.Employees
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<IEnumerable<Employee>> GetEmployes()
        {
            return await context.Employees
                .ToListAsync();
        }
    }
}
