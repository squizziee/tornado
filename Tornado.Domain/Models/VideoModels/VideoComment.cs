using Tornado.Domain.Models.ProfileModels;

namespace Tornado.Domain.Models.VideoModels
{
    public class VideoComment
    {
        public Guid Id { get; set; }
        public Guid UserCommentsId { get; set; }
        public UserComments UserComments { get; set; }
        public Guid CommentChainId { get; set; }
        public VideoCommentChain CommentChain { get; set; }
        public string Text { get; set; } = string.Empty;
        public IEnumerable<CommentRating> CommentRatings { get; set; } = [];
        public bool IsReply { get; set; }
        public Guid? RepliesTo { get; set; }
        public DateOnly CreatedAt { get; set; }
    }
}
