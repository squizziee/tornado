using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tornado.Domain.Models.Auth;
using Tornado.Infrastructure.Services.Interfaces;

namespace Tornado.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        // Generates access token with user's Id, Email and Role
        public string GenerateNewJwtToken(User user)
        {
            var claims = new[]
            {    
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:AccessToken:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:AccessToken:Issuer"],
                audience: _configuration["Jwt:AccessToken:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:AccessToken:ExpirationTimeInMinutes"]!)),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Generates refresh token with user's Id, Email and Role
        public string GenerateNewRefreshToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:RefreshToken:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:RefreshToken:Issuer"],
                audience: _configuration["JWT:RefreshToken:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(double.Parse(_configuration["Jwt:RefreshToken:ExpirationTimeInDays"]!)),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public bool VerifyRefreshToken(string refreshToken)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:RefreshToken:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var validator = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidIssuer = _configuration["JWT:RefreshToken:Issuer"],
                ValidAudience = _configuration["JWT:RefreshToken:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:RefreshToken:Key"]!))
            };

            if (validator.CanReadToken(refreshToken))
            {
                ClaimsPrincipal principal;
                try
                {
                    // This line throws if invalid
                    principal = validator.ValidateToken(refreshToken, validationParameters, out var validatedToken);

                    // If we got here then the token is valid
                    if (principal.HasClaim(c => c.Type == ClaimTypes.Email))
                    {
                        return true;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }
    }
}
