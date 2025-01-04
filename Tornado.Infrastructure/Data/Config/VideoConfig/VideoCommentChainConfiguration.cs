using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tornado.Domain.Models.VideoModels;

namespace Tornado.Infrastructure.Data.Config.VideoConfig
{
    public class VideoCommentChainConfiguration : IEntityTypeConfiguration<VideoCommentChain>
    {
        public void Configure(EntityTypeBuilder<VideoCommentChain> builder)
        {
            builder.ToTable("VideoCommentChains");

            builder.HasKey(chain => chain.Id);

            builder
                .HasMany(chain => chain.VideoComments)
                .WithOne(comment => comment.CommentChain)
                .HasForeignKey(comment => comment.CommentChainId)
                .IsRequired();
        }
    }
}
