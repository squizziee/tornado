using Tornado.Domain.Models.ProfileModels;
using Tornado.Domain.Models.VideoModels;

namespace Tornado.Domain.Models.ChannelModels
{
    public class Channel
    {
        public Guid Id { get; set; }
        public UserProfile Profile { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ChannelHeaderSourceUrl { get; set; } = string.Empty;
        public string ChannelAvatarSourceUrl { get; set; } = string.Empty;
        public IEnumerable<Video> Videos { get; set; } = [];
        public ChannelMetrics Metrics { get; set; }
    }
}
