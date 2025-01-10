using Microsoft.AspNetCore.Http;

namespace Tornado.Contracts.Requests
{
    public record RegisterUserRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Nickname { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public IFormFile? Avatar { get; set; }

    }
}
