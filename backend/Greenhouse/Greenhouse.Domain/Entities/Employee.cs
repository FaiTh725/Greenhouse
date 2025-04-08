using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace Greenhouse.Domain.Entities
{
    public class Employee : Entity
    {
        public string Email { get; private set; } = string.Empty;
    
        public string? Name { get; private set; }

        public List<AgricultiralEvent> Events { get; private set; } = new List<AgricultiralEvent>();

        private Employee(
            string email,
            string? name)
        {
            Email = email;
            Name = name;
        }

        public static Result<Employee> Initialize(
            string email,
            string? name)
        {
            if(!IsValidEmail(email))
            {
                return Result.Failure<Employee>("Incorrect email");
            }

            return Result.Success(new Employee(
                email, name));
        }

        public static bool IsValidEmail(string checkEmail)
        {
            Regex emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            return emailRegex.IsMatch(checkEmail);
        }
    }
}
