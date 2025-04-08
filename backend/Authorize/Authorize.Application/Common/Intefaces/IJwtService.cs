using CSharpFunctionalExtensions;

namespace Authorize.Application.Common.Intefaces
{
    public interface IJwtService<TokenObj, TokenResponse>
        where TokenObj : class
        where TokenResponse : class
    {
        string GenerateToken(TokenObj obj);

        Result<TokenResponse> DecodeToken(string token);

        string GenerateRefreshToken();
    }
}
