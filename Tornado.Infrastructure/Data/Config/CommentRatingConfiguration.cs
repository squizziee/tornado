using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tornado.Domain.Models;

namespace Tornado.Infrastructure.Data.Config
{
    public class CommentRatingConfiguration : IEntityTypeConfiguration<CommentRating>
    {
        public void Configure(EntityTypeBuilder<CommentRating> builder)
        {
            builder.ToTable("CommentRatings");

            builder.HasKey(rating => rating.Id);

            builder
                .Property(rating => rating.RatingType)
                .IsRequired();
        }
    }
}
