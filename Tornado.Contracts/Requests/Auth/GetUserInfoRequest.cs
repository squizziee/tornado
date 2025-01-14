namespace Tornado.Contracts.Requests.Auth
{
    public record GetUserInfoRequest
    {
        public required Guid Id { get; set; }
    }
}
