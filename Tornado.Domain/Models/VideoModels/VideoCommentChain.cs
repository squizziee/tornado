namespace Tornado.Domain.Models.VideoModels
{
    public class VideoCommentChain
    {
        public Guid Id { get; set; }
        public List<VideoComment> VideoComments { get; set; } = [];
    }
}
