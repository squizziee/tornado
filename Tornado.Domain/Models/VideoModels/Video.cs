using Tornado.Domain.Models.VideoModels;
using Tornado.Domain.Models.ProfileModels;

namespace Tornado.Domain.Models.VideoModels
{
    public class Video
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid AuthorId { get; set; }
        public UserProfile Author { get; set; }
        public List<VideoComment> VideoComments { get; set; }
        public List<VideoRating> VideoRatings { get; set; }
        public string SourceUrl { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public VideoMetrics Metrics { get; set; }
    }
}