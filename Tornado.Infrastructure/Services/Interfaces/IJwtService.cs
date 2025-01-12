using Tornado.Domain.Models.Auth;

namespace Tornado.Infrastructure.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateNewJwtToken(User user);
        string GenerateNewRefreshToken(User user);
        bool VerifyRefreshToken(string refreshToken);
    }
}
