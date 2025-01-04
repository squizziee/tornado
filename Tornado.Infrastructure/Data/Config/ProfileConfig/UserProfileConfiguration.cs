using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tornado.Domain.Models.ProfileModels;

namespace Tornado.Infrastructure.Data.Config.ProfileConfig
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.ToTable("UserProfiles");

            builder.HasKey(profile => profile.Id);

            builder
                .Property(profile => profile.Nickname)
                .IsRequired()
                .HasMaxLength(255);

            builder
                .Property(profile => profile.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(profile => profile.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(profile => profile.AvatarUrl)
                .HasMaxLength(255);

            builder
                .HasOne(profile => profile.UserComments)
                .WithOne(userComments => userComments.Profile)
                .HasForeignKey<UserProfile>(profile => profile.UserCommentsId)
                .IsRequired();

            builder
                .HasOne(profile => profile.UserRatings)
                .WithOne(userRatings => userRatings.Profile)
                .HasForeignKey<UserProfile>(profile => profile.UserRatingsId)
                .IsRequired();

            builder
                .HasOne(profile => profile.Channel)
                .WithOne(channel => channel.Profile)
                .HasForeignKey<UserProfile>(profile => profile.ChannelId)
                .IsRequired(false);
        }
    }
}
