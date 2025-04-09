namespace Authorize.Application.Contracts.User
{
    public class UserDataResponse
    {
        public long Id { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
    }
}
