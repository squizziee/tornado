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
                    dest => dest.Role,
                    options => options.MapFrom(src => src.Role.ToString()));
        }
    }
}
