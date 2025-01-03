using Tornado.Domain.Enums;
using Tornado.Domain.Models.VideoModels;

namespace Tornado.Domain.Models
{
    public class CommentRating
    {
        public Guid Id { get; set; }
        public Guid CommentId { get; set; }
        public VideoComment Comment { get; set; }
        public RatingType RatingType { get; set; }
    }
}