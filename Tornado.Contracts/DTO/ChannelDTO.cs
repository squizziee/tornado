namespace Tornado.Contracts.DTO
{
    public record ChannelDTO
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public string? HeaderUrl { get; set; }
        public string? AvatarUrl { get; set; }
        public required VideoShortDTO[] Videos { get; set; }
    }
}
