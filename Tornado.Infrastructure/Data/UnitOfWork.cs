using Microsoft.EntityFrameworkCore.Storage;
using Tornado.Infrastructure.Interfaces;
using Tornado.Infrastructure.Interfaces.Repositories;

namespace Tornado.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
	{
		private ApplicationDatabaseContext _databaseContext;
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
		private IDbContextTransaction dbContextTransaction { get; set; }

        public UnitOfWork(
			ApplicationDatabaseContext databaseContext,
			IUserRepository userRepository,
			IUserProfileRepository userProfileRepository,
			IUserRatingsRepository userRatingsRepository,
			IUserCommentRepository userCommentRepository//,
			//IChannelRepository channelRepository,
			//IChannelMetricsRepository channelMetricsRepository,
			//IVideoRepository videoRepository,
			//IVideoRatingRepository videoRatingRepository,
			//IVideoCommentChainRepository videoCommentChainRepository,
			//IVideoCommentRepository videoCommentRepository,
			//IVideoMetricsRepository videoMetricsRepository
			)
		{
			_databaseContext = databaseContext;
			this.UserRepository = userRepository;
			this.UserProfileRepository = userProfileRepository;
			this.UserRatingsRepository = userRatingsRepository;
			this.UserCommentRepository = userCommentRepository;
			//this.ChannelRepository = channelRepository;
			//this.ChannelMetricsRepository = channelMetricsRepository;
			//this.VideoRepository = videoRepository;
			//this.VideoRatingRepository = videoRatingRepository;
			//this.VideoMetricsRepository = videoMetricsRepository;
			//this.VideoCommentChainRepository = videoCommentChainRepository;
			//this.VideoCommentRepository = videoCommentRepository;
			//this.VideoMetricsRepository = videoMetricsRepository;
		}
		public void Commit()
		{
			_databaseContext.Database.CommitTransaction();
		}

		public void CreateTransaction()
		{
			this.dbContextTransaction = _databaseContext.Database.BeginTransaction();
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		public void Rollback()
		{
			_databaseContext.Database.RollbackTransaction();
		}

		public void Save()
		{
			_databaseContext.SaveChanges();
		}
	}
}
