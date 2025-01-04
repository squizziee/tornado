using Tornado.Domain.Enums;
using Tornado.Domain.Models.ProfileModels;

namespace Tornado.Domain.Models.VideoModels
{
    public class VideoRating
    {
        public Guid Id { get; set; }
        public Guid VideoId { get; set; }
        public Video Video { get; set; }
        public Guid UserRatingsId { get; set; }
        public UserRatings UserRatings { get; set; }    
        public RatingType RatingType { get; set; }
    }
}
