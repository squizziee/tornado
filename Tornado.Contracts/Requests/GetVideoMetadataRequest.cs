namespace Tornado.Contracts.Requests
{
    public record GetVideoMetadataRequest
    {
        public required Guid Guid { get; set; }
    }
}
