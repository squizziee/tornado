using Microsoft.AspNetCore.Http;

namespace Tornado.Contracts.Requests
{
    public record UploadVideoRequest
    {
        public required Guid UserId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required IFormFile VideoData { get; set; }
        public IFormFile? PreviewData { get; set; }
    }
}
