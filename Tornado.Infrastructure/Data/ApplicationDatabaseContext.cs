using Microsoft.EntityFrameworkCore;
using Tornado.Domain.Models;
using Tornado.Domain.Models.Auth;
using Tornado.Domain.Models.ChannelModels;
using Tornado.Domain.Models.ProfileModels;
using Tornado.Domain.Models.VideoModels;
using Tornado.Infrastructure.Data.Config;
using Tornado.Infrastructure.Data.Config.AuthConfig;
using Tornado.Infrastructure.Data.Config.ChannelConfig;
using Tornado.Infrastructure.Data.Config.ProfileConfig;
using Tornado.Infrastructure.Data.Config.VideoConfig;

namespace Tornado.Infrastructure.Data
{
    public class ApplicationDatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserRatings> UserRatings { get; set; }
        public DbSet<UserComments> UserComments { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<ChannelMetrics> ChannelMetrics { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<VideoComment> VideoComments { get; set; }
        public DbSet<VideoCommentChain> VideoCommentChains { get; set; }
        public DbSet<CommentRating> CommentRatings { get; set; }
        public DbSet<VideoRating> VideoRatings { get; set; }
        public DbSet<VideoMetrics> VideoMetrics { get; set; }

        public ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            modelBuilder.ApplyConfiguration(new ChannelConfiguration());
            modelBuilder.ApplyConfiguration(new ChannelMetricsConfiguration());

            modelBuilder.ApplyConfiguration(new UserProfileConfiguration());
            modelBuilder.ApplyConfiguration(new UserCommentsConfiguration());
            modelBuilder.ApplyConfiguration(new UserRatingsConfiguration());

            modelBuilder.ApplyConfiguration(new VideoConfiguration());
            modelBuilder.ApplyConfiguration(new VideoMetricsConfiguration());
            modelBuilder.ApplyConfiguration(new VideoRatingConfiguration());
            modelBuilder.ApplyConfiguration(new VideoCommentChainConfiguration());
            modelBuilder.ApplyConfiguration(new VideoCommentConfiguration());

            modelBuilder.ApplyConfiguration(new CommentRatingConfiguration());
        }
    }
}
