using Tornado.Domain.Models.ProfileModels;

namespace Tornado.Domain.Models.Auth
{
    public class User
    {
        public Guid Id { get; set; }
        public Guid ProfileId { get; set; }
        public UserProfile Profile { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}
