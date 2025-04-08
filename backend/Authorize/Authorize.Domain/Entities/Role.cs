using CSharpFunctionalExtensions;

namespace Authorize.Domain.Entities
{
    public sealed class Role
    {
        public string Name { get; private set; } = string.Empty;

        public List<User> Users { get; private set; } = new List<User>();

        // For EF
        public Role()
        {}

        private Role(string name)
        {
            Name = name;
        }

        public static Result<Role> Initialize(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                return Result.Failure<Role>("Name is empty");
            }

            return Result.Success(new Role(name));
        }
    }
}
