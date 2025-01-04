using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tornado.Domain.Models.ChannelModels;
using Tornado.Domain.Models.VideoModels;

namespace Tornado.Infrastructure.Data.Config.ChannelConfig
{
    public class ChannelMetricsConfiguration : IEntityTypeConfiguration<ChannelMetrics>
    {
        public void Configure(EntityTypeBuilder<ChannelMetrics> builder)
        {
            builder.ToTable("ChannelMetrics");

            builder.HasKey(metrics => metrics.Id);
        }
    }
}
