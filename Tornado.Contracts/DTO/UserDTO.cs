﻿namespace Tornado.Contracts.DTO
{
    public record UserDTO
    {
        public required Guid Id { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
        public required UserProfileDTO Profile { get; set; }
    }
}
