namespace Tornado.Domain.Models.VideoModels
{
    public class VideoComment
    {
        public Guid Id { get; set; }
        public Guid CommentChainId { get; set; }
        public string Text { get; set; } = string.Empty;
        public List<CommentRating> CommentRatings { get; set; } = [];
        public bool IsReply { get; set; }
        public Guid? RepliesTo { get; set; }
    }
}
