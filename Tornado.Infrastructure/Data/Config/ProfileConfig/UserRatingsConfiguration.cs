using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tornado.Domain.Models.ProfileModels;

namespace Tornado.Infrastructure.Data.Config.ProfileConfig
{
    public class UserRatingsConfiguration : IEntityTypeConfiguration<UserRatings>
    {
        public void Configure(EntityTypeBuilder<UserRatings> builder)
        {
            builder.ToTable("UserRatings");

            builder.HasKey(userRatings => userRatings.Id);

            builder
                .HasMany(userRatings => userRatings.VideoRatings)
                .WithOne(videoRatings => videoRatings.UserRatings)
                .HasForeignKey(videoRatings => videoRatings.UserRatingsId)
                .IsRequired();

            builder
                .HasMany(userRatings => userRatings.CommentRatings)
                .WithOne(commentRatings => commentRatings.UserRatings)
                .HasForeignKey(commentRatings => commentRatings.UserRatingsId)
                .IsRequired();
        }
    }
}
