namespace Tornado.Infrastructure.Services.Settings
{
    public record ImageUploadSettings
    {
        public required string Root { get; set; }
        public required string Avatars { get; set; }
        public required string VideoPreviews { get; set; }
        public required string ChannelHeaders { get; set; }
        public required string ChannelAvatars { get; set; }
        public required string[] SupportedExtensions { get; set; }
    }
}
