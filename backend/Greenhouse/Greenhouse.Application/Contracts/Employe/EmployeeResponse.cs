namespace Greenhouse.Application.Contracts.Employe
{
    public class EmployeeResponse
    {
        public long Id { get; set; }

        public string Email { get; set; } = string.Empty;

        public string? Name { get; set; }
    }
}
