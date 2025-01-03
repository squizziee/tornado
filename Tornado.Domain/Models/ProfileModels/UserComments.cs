using Tornado.Domain.Models.VideoModels;

namespace Tornado.Domain.Models.ProfileModels
{
    public class UserComments
    {
        public Guid Id { get; set; }
        public List<VideoComment> VideoComments { get; set; } = [];
    }
}
