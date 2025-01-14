namespace Tornado.Contracts.Requests.Auth
{
    public record GetVideoMetadataRequest
    {
        public required Guid Guid { get; set; }
    }
}
