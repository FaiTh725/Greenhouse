using Greenhouse.Domain.Entities;

namespace Greenhouse.Domain.Repositorires
{
    public interface IEmployeRepository
    {
        Task<Employee> AddEmploye(Employee employee);

        Task<Employee?> GetEmploye(long id);

        Task<Employee?> GetEmploye(string email);

        Task<IEnumerable<Employee>> GetEmployes();
    }
}
