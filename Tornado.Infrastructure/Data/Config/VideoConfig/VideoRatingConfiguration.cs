using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tornado.Domain.Models.VideoModels;

namespace Tornado.Infrastructure.Data.Config.VideoConfig
{
    public class VideoRatingConfiguration : IEntityTypeConfiguration<VideoRating>
    {
        public void Configure(EntityTypeBuilder<VideoRating> builder)
        {
            builder.ToTable("VideoRatings");

            builder.HasKey(rating => rating.Id);

            builder
                .Property(rating => rating.RatingType)
                .IsRequired();
        }
    }
}
