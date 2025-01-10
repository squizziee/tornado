using Microsoft.EntityFrameworkCore.Storage;
using Tornado.Infrastructure.Interfaces.Repositories;

namespace Tornado.Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IUserRepository UserRepository { get; set; }
        public IUserProfileRepository UserProfileRepository { get; set; }
        public IUserRatingsRepository UserRatingsRepository { get; set; }
        public IUserCommentRepository UserCommentRepository { get; set; }
        public IChannelRepository ChannelRepository { get; set; }
        public IChannelMetricsRepository ChannelMetricsRepository { get; set; }
        public IVideoRepository VideoRepository { get; set; }
        public IVideoRatingRepository VideoRatingRepository { get; set; }
        public IVideoCommentChainRepository VideoCommentChainRepository { get; set; }
        public IVideoCommentRepository VideoCommentRepository { get; set; }
        public IVideoMetricsRepository VideoMetricsRepository { get; set; }
        void CreateTransaction();
        void Commit();
        void Rollback();
        void Save();
    }
}
