using AutoMapper;
using Tornado.Contracts.DTO;

namespace Tornado.Application.AutoMapperProfiles
{
    public class ChannelMapperProfile : Profile
    {
        public ChannelMapperProfile()
        {
            CreateMap<Domain.Models.ChannelModels.Channel, ChannelDTO>()
                .ForMember(
                    dest => dest.AvatarUrl,
                    options => options.MapFrom(src => src.ChannelAvatarSourceUrl))
                .ForMember(
                    dest => dest.HeaderUrl,
                    options => options.MapFrom(src => src.ChannelAvatarSourceUrl));
        }
    }
}
