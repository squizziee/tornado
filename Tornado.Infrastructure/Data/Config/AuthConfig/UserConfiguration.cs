using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tornado.Domain.Models.Auth;
using Tornado.Domain.Models.ProfileModels;

namespace Tornado.Infrastructure.Data.Config.AuthConfig
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(user => user.Id);
            builder
                .HasOne(user => user.Profile)
                .WithOne(profile => profile.User)
                .HasForeignKey<User>(user => user.ProfileId)
                .IsRequired();

            builder
                .Property(user => user.Email)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(user => user.PasswordHash)
                .IsRequired();
        }
    }
}
