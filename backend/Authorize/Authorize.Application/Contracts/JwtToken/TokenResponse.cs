
namespace Authorize.Application.Contracts.JwtToken
{
    public class TokenResponse
    {
        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}
