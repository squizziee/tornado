using AutoMapper;
using Tornado.Contracts.DTO;
using Tornado.Domain.Models.ProfileModels;

namespace Tornado.Application.AutoMapperProfiles
{
	public class UserProfileMapperProfile : Profile
	{
		public UserProfileMapperProfile() {
			CreateMap<UserProfile, UserProfileDTO>();
		}
	}
}
