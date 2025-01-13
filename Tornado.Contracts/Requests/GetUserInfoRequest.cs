namespace Tornado.Contracts.Requests
{
    public record GetUserInfoRequest
    {
        public required Guid Id { get; set; }
    }
}
