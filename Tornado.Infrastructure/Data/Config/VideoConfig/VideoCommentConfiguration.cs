using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tornado.Domain.Models.VideoModels;

namespace Tornado.Infrastructure.Data.Config.VideoConfig
{
    public class VideoCommentConfiguration : IEntityTypeConfiguration<VideoComment>
    {
        public void Configure(EntityTypeBuilder<VideoComment> builder)
        {
            builder.ToTable("VideoComments");

            builder.HasKey(comment => comment.Id);

            builder
                .Property(comment => comment.Text)
                .HasMaxLength(65535)
                .IsRequired();

            builder
                .Property(comment => comment.IsReply)
                .IsRequired();

            builder
                .Property(comment => comment.RepliesTo)
                .IsRequired(false);

            builder
                .HasMany(comment => comment.CommentRatings)
                .WithOne(rating => rating.Comment)
                .HasForeignKey(rating => rating.CommentId)
                .IsRequired();
        }
    }
}
