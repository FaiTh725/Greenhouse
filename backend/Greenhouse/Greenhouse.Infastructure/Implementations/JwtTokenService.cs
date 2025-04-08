using CSharpFunctionalExtensions;
using Greenhouse.Application.Common.Intrefaces;
using Greenhouse.Application.Contracts.Employe;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Greenhouse.Infastructure.Implementations
{
    public class JwtTokenService : IJwtTokenService<EmployeToken>
    {
        public Result<EmployeToken> DecodeToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var email = jwtToken.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var role = jwtToken.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;

            if(email is null ||
                role is null)
            {
                return Result.Failure<EmployeToken>("Invalid token signature");
            }

            return Result.Success(new EmployeToken
            {
                Email = email,
                Role = role
            });
        }
    }
}
