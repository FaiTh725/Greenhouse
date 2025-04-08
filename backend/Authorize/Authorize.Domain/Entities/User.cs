using Authorize.Domain.Events;
using Authorize.Domain.Primitives;
using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace Authorize.Domain.Entities
{
    public sealed class User : DomainEventEntity
    {
        public const int MAX_PASSWORD_LENGHT = 30;
        public const int MIN_PASSWORD_LENGHT = 5;

        public string Email { get; private set; } = string.Empty;

        public string? Name { get; private set; } = string.Empty;

        public string PasswordHash { get; private set; } = string.Empty;

        public bool IsActive { get; private set; } = false;

        public Role Role { get; private set; }

        // For EF
        public User() { }

        private User(
            string email,
            string passwordHash,
            Role role,
            string? name = null)
        {
            Email = email;
            Name = name;
            PasswordHash = passwordHash;
            Role = role;
        }

        public static Result<User> Initialize(
            string email,
            string passwordHash,
            Role role,
            string? name = null)
        {
            if(!IsValidEmail(email))
            {
                return Result.Failure<User>("Invalid email");
            }

            if(role is null)
            {
                return Result.Failure<User>("Role is Required");
            }
            
            return Result.Success(new User(
                email, passwordHash, role, name));
        }

        public void Active()
        {
            IsActive = true;

            RaiseDomainEvent(new ActiveUserEvent
            {
                UserEmail = Email
            });
        }

        public static bool IsValidEmail(string checkEmail)
        {
            Regex emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            return emailRegex.IsMatch(checkEmail);
        }

        public static bool IsValidPassword(string password)
        {
            Regex passwordRegex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d).+$");

            return passwordRegex.IsMatch(password) && 
                password.Length >= MIN_PASSWORD_LENGHT &&
                password.Length <= MAX_PASSWORD_LENGHT;
        }
    }
}
