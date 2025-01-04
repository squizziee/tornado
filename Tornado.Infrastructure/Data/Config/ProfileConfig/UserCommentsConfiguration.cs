using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tornado.Domain.Models.ProfileModels;

namespace Tornado.Infrastructure.Data.Config.ProfileConfig
{
    public class UserCommentsConfiguration : IEntityTypeConfiguration<UserComments>
    {
        public void Configure(EntityTypeBuilder<UserComments> builder)
        {
            builder.ToTable("UserComments");

            builder.HasKey(userComments => userComments.Id);

            builder
                .HasMany(userComments => userComments.VideoComments)
                .WithOne(videoComments => videoComments.UserComments)
                .HasForeignKey(videoComments => videoComments.UserCommentsId)
                .IsRequired();
        }
    }
}
