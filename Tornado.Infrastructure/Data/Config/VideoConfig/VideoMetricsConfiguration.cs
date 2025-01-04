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
    public class VideoMetricsConfiguration : IEntityTypeConfiguration<VideoMetrics>
    {
        public void Configure(EntityTypeBuilder<VideoMetrics> builder)
        {
            builder.ToTable("VideoMetrics");

            builder.HasKey(metrics => metrics.Id);

            builder
                .Property(metrics => metrics.TotalViews)
                .IsRequired();

            builder
                .Property(metrics => metrics.TotalLikes)
                .IsRequired();

            builder
                .Property(metrics => metrics.AverageWatchTime)
                .IsRequired();

            builder
                .Property(metrics => metrics.AverageWatchPercentage)
                .IsRequired();

            builder
                .Property(metrics => metrics.UniqueViewersCount)
                .IsRequired();

            builder
                .Property(metrics => metrics.AverageViewsPerUniqueViewer)
                .IsRequired();
        }
    }
}
