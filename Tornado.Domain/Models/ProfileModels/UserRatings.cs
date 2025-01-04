using Tornado.Domain.Models.VideoModels;

namespace Tornado.Domain.Models.ProfileModels
{
    public class UserRatings
    {
        public Guid Id { get; set; }
        public List<VideoRating> VideoRatings { get; set; } = [];
        public List<CommentRating> CommentRatings { get; set; } = [];
    }
}