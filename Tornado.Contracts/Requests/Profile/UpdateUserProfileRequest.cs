using Microsoft.AspNetCore.Http;

namespace Tornado.Contracts.Requests.Profile
{
    public record UpdateUserProfileRequest
    {
        public required Guid UserId {  get; set; }
        public required string Nickname { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public IFormFile? AvatarData { get; set; }
    }
}
