using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tornado.Domain.Models.ChannelModels;
using Tornado.Domain.Models.ProfileModels;

namespace Tornado.Infrastructure.Data.Config.ChannelConfig
{
    public class ChannelConfiguration : IEntityTypeConfiguration<Channel>
    {
        public void Configure(EntityTypeBuilder<Channel> builder)
        {
            builder.ToTable("Channels");

            builder.HasKey(channel => channel.Id);

            builder
                .Property(profile => profile.Name)
                .HasMaxLength(255)
                .IsRequired();

            builder
                .Property(profile => profile.Description)
                .HasMaxLength(65535)
                .IsRequired();

            builder
                .Property(profile => profile.ChannelAvatarSourceUrl)
                .HasMaxLength(200)
                .IsRequired();

            builder
                .Property(profile => profile.ChannelHeaderSourceUrl)
                .HasMaxLength(200)
                .IsRequired();

            builder
                .HasMany(channel => channel.Videos)
                .WithOne(video => video.Channel)
                .HasForeignKey(video => video.ChannelId);

            //builder
            //   .HasOne(channel => channel.Metrics)
            //   .WithOne(metrics => metrics.Channel)
            //   .HasForeignKey<ChannelMetrics>(metrics => metrics.ChannelId)
            //   .IsRequired();
        }
    }
}
