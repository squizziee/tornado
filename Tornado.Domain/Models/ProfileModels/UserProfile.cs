﻿using Tornado.Domain.Models.Auth;
using Tornado.Domain.Models.ChannelModels;

namespace Tornado.Domain.Models.ProfileModels
{
    public class UserProfile
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public string Nickname { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public Guid UserCommentsId { get; set; }
        public UserComments UserComments { get; set; }
        public Guid UserRatingsId { get; set; }
        public UserRatings UserRatings { get; set; }
        public string? AvatarUrl { get; set; }
        public Guid? ChannelId { get; set; }
        public Channel? Channel { get; set; }
        public DateOnly CreatedAt { get; set; }
    }
}
