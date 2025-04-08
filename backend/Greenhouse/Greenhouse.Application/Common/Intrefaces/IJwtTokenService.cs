using CSharpFunctionalExtensions;

namespace Greenhouse.Application.Common.Intrefaces
{
    public interface IJwtTokenService<TResponse>
    {
        Result<TResponse> DecodeToken(string token);
    }
}
