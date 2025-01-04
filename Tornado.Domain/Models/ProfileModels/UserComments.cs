using Tornado.Domain.Models.VideoModels;

namespace Tornado.Domain.Models.ProfileModels
{
    // container for UserProfile that stores comments for different content
    public class UserComments
    {
        public Guid Id { get; set; }
        public UserProfile Profile { get; set; }
        public IEnumerable<VideoComment> VideoComments { get; set; } = [];
    }
}
