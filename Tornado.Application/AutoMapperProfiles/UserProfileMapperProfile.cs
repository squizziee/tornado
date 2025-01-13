using AutoMapper;
using Tornado.Contracts.DTO;
using Tornado.Domain.Models.ProfileModels;

namespace Tornado.Application.AutoMapperProfiles
{
	public class UserProfileMapperProfile : Profile
	{
		public UserProfileMapperProfile() {
			CreateMap<UserProfile, UserProfileDTO>()
				.ForMember(
					dest => dest.Id,
					options => options.MapFrom(src => src.Id))
				.ForMember(
					dest => dest.Nickname,
					options => options.MapFrom(src => src.Nickname))
				.ForMember(
					dest => dest.FirstName,
					options => options.MapFrom(src => src.FirstName.ToString()))
				.ForMember(
					dest => dest.LastName,
					options => options.MapFrom(src => src.LastName.ToString()))
				.ForMember(
					dest => dest.UserCommentsId,
					options => options.MapFrom(src => src.UserCommentsId))
				.ForMember(
					dest => dest.UserRatingsId,
					options => options.MapFrom(src => src.UserRatingsId))
				.ForMember(
					dest => dest.AvatarUrl,
					options => options.MapFrom(src => src.AvatarUrl))
				.ForMember(
					dest => dest.ChannelId,
					options => options.MapFrom(src => src.ChannelId));
		}
	}
}
