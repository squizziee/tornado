using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tornado.Domain.Models.VideoModels
{
    // Probably needs an entire microservice for proper functionality
    public class VideoMetrics
    {
        public Guid Id { get; set; }
        public Guid VideoId { get; set; }
        public Video Video { get; set; }
        public long TotalViews { get; set; }
        public long TotalLikes { get; set; }
        public TimeSpan AverageWatchTime { get; set; }
        public double AverageWatchPercentage { get; set; }
        public long UniqueViewersCount { get; set; }
        public long AverageViewsPerUniqueViewer { get; set; }
    }
}
