namespace Tornado.Contracts.Requests.Auth
{
    public record LoginWithEmailAndPasswordRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
