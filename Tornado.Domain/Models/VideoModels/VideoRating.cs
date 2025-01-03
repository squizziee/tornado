using Tornado.Domain.Enums;

namespace Tornado.Domain.Models.VideoModels
{
    public class VideoRating
    {
        public Guid Id { get; set; }
        public Guid VideoId { get; set; }
        public Video Video { get; set; }
        public RatingType RatingType { get; set; }
    }
}
