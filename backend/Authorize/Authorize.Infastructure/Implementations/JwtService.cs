
using Application.Shared.Exceptions;
using Authorize.Application.Common.Intefaces;
using Authorize.Application.Contracts.JwtToken;
using Authorize.Application.Contracts.User;
using Authorize.Infastructure.Configurations;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Authorize.Infastructure.Implementations
{
    public class JwtService : IJwtService<UserResponse, TokenResponse>
    {
        private readonly IConfiguration configuration;

        public JwtService(
            IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Result<TokenResponse> DecodeToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

            var email = jwtSecurityToken.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var role = jwtSecurityToken.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
        
            if(email is null || 
                role is null)
            {
                return Result.Failure<TokenResponse>("Invalid token signature");
            }

            return Result.Success(new TokenResponse
            {
                Email = email,
                Role = role
            });
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        public string GenerateToken(UserResponse user)
        {
            var jwtConf = configuration.GetSection("JwtSettings")
                .Get<JwtConf>() ??
                throw new AppConfigurationException("Jwt token configuration");

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConf.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                audience: jwtConf.Audience,
                issuer: jwtConf.Issuer,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddMinutes(jwtConf.ExpirationTime));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
