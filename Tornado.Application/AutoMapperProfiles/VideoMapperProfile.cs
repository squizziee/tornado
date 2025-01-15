using AutoMapper;
using Tornado.Contracts.DTO;
using Tornado.Domain.Models.VideoModels;

namespace Tornado.Application.AutoMapperProfiles
{
    public class VideoMapperProfile : Profile
    {
        public VideoMapperProfile()
        {
            CreateMap<Video, VideoShortDTO>()
                .ForMember(
                    dest => dest.ChannelName,
                    options => options.MapFrom(src => src.Channel.Name));
        }
    }
}
