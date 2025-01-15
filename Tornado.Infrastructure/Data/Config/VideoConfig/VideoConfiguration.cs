using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tornado.Domain.Models.VideoModels;

namespace Tornado.Infrastructure.Data.Config.VideoConfig
{
    public class VideoConfiguration : IEntityTypeConfiguration<Video>
    {
        public void Configure(EntityTypeBuilder<Video> builder)
        {
            builder.ToTable("Videos");

            builder.HasKey(video => video.Id);

            builder
                .Property(video => video.Title)
                .HasMaxLength(65535)
                .IsRequired();

            builder
                .Property(video => video.Description)
                .HasMaxLength(65535)
                .IsRequired();

            builder
                .Property(video => video.SourceFileName)
                .HasMaxLength(65535)
                .IsRequired();

            builder
                .Property(video => video.PreviewSourceUrl)
                .HasMaxLength(65535)
                .IsRequired();

            builder
                .Property(video => video.Duration)
                .IsRequired();

            //builder
            //    .HasOne(video => video.Metrics)
            //    .WithOne(metrics => metrics.Video)
            //    .HasForeignKey<VideoMetrics>(metrics => metrics.VideoId);

            builder
                .HasMany(video => video.VideoCommentChains)
                .WithOne(commentChain => commentChain.Video)
                .HasForeignKey(commentChain => commentChain.VideoId)
                .IsRequired();

            builder
                .HasMany(video => video.VideoRatings)
                .WithOne(videoRating => videoRating.Video)
                .HasForeignKey(videoRating => videoRating.VideoId)
                .IsRequired();
        }
    }
}
