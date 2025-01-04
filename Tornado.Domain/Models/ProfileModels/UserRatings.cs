using Tornado.Domain.Models.VideoModels;

namespace Tornado.Domain.Models.ProfileModels
{
    // container for UserProfile that stores ratings for different content
    public class UserRatings
    {
        public Guid Id { get; set; }
        public UserProfile Profile { get; set; }
        public IEnumerable<VideoRating> VideoRatings { get; set; } = [];
        public IEnumerable<CommentRating> CommentRatings { get; set; } = [];
    }
}