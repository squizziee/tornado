using AutoMapper;
using Tornado.Contracts.DTO;
using Tornado.Domain.Models.Auth;
using Tornado.Domain.Models.ProfileModels;

namespace Tornado.Application.AutoMapperProfiles
{
    public class UserMapperProfile : Profile
    {

        public UserMapperProfile() {
            CreateMap<User, UserDTO>()
                .ForMember(
                    dest => dest.Id,
                    options => options.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest.Email,
                    options => options.MapFrom(src => src.Email))
                .ForMember(
                    dest => dest.Role,
                    options => options.MapFrom(src => src.Role.ToString()));
                //.ForMember(
                //    dest => dest.Profile, 
                //    options => options.MapFrom(src => mapper.Map<UserProfile, UserProfileDTO>(src.Profile)));
        }
    }
}
