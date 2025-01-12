namespace Tornado.Contracts.DTO
{
	public record UserProfileDTO
	{
		public required Guid Id { get; set; }
		public required string Nickname { get; set; }
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
		public required Guid UserCommentsId { get; set; }
        public required Guid UserRatingsId { get; set; }
        public string? AvatarUrl { get; set; }
        public Guid? ChannelId { get; set; }
    }
}
