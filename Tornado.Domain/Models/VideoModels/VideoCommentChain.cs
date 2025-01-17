namespace Tornado.Domain.Models.VideoModels
{
    public class VideoCommentChain
    {
        public Guid Id { get; set; }
        public Guid VideoId { get; set; }
        public Video Video { get; set; }
        public IEnumerable<VideoComment> VideoComments { get; set; } = [];
        public DateOnly CreatedAt { get; set; }
    }
}
