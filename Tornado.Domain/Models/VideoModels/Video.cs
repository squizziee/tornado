using Tornado.Domain.Models.ChannelModels;

namespace Tornado.Domain.Models.VideoModels
{
    #pragma warning disable CS8618
    public class Video
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid ChannelId { get; set; }
        public Channel Channel { get; set; }
        public IEnumerable<VideoCommentChain> VideoCommentChains { get; set; }
        public IEnumerable<VideoRating> VideoRatings { get; set; }
        public string SourceFileName { get; set; } = string.Empty;
        public string PreviewSourceUrl { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        //public VideoMetrics Metrics { get; set; }
        public DateOnly CreatedAt { get; set; }
    }
}