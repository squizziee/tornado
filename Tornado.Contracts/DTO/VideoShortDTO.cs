namespace Tornado.Contracts.DTO
{
    public record VideoShortDTO
    {
        public required Guid Id { get; set; }
        public required string Description { get; set; } = string.Empty;
        public required string ChannelName { get; set; } = string.Empty;
        public required Guid ChannelId { get; set; }
        public required string PreviewUrl { get; set; }
        public required string Duration { get; set; }
    }
}
