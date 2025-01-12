namespace Tornado.Infrastructure.Services.Interfaces
{
    public interface IPasswordHashingService
    {
        string GenerateHash(string password);
        bool Verify(string providedPassword, string validPasswordHash);
    }
}
