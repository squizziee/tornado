﻿namespace Tornado.Domain.Models.ChannelModels
{
    public class ChannelMetrics
    {
        public Guid Id { get; set; }
        public Guid ChannelId { get; set; }
        public Channel Channel { get; set; }
    }
}