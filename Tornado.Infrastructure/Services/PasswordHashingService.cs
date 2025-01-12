using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using Tornado.Infrastructure.Services.Interfaces;

namespace Tornado.Infrastructure.Services
{
	public class PasswordHashingService : IPasswordHashingService
	{
		private IConfiguration _configuration;

		public PasswordHashingService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string GenerateHash(string password)
		{
			var hash = BCrypt.Net.BCrypt.EnhancedHashPassword(password);

			return hash;
        }

		public bool Verify(string providedPassword, string validPasswordHash)
		{
			return BCrypt.Net.BCrypt.EnhancedVerify(providedPassword, validPasswordHash);

        }
	}
}
