using Microsoft.AspNetCore.Http;

namespace Tornado.Contracts.Requests.Channel
{
    public record CreateChannelRequest
    {
        public required Guid UserId { get; set; }
        public required string ChannelName { get; set; }
        public string? ChannelDescription { get; set; }
        public IFormFile? ChannelAvatarData { get; set; }
        public IFormFile? ChannelHeaderData { get; set; }
    }
}
