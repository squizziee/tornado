using AutoMapper;
using Tornado.Application.UseCases.Interfaces.Channel;
using Tornado.Contracts.DTO;
using Tornado.Contracts.Requests.Channel;
using Tornado.Infrastructure.Interfaces;

namespace Tornado.Application.UseCases.Channel
{
    public class GetChannelUseCase : IGetChannelUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Mapper _mapper;

        public GetChannelUseCase(IUnitOfWork unitOfWork, Mapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ChannelDTO> ExecuteAsync(GetChannelRequest request, CancellationToken cancellationToken)
        {
            var tryFindChannel = await _unitOfWork.ChannelRepository.GetByIdAsync(request.Id, cancellationToken);

            if (tryFindChannel == null)
            {
                throw new Exception($"Channel with id of {request.Id} was not found");
            }

            return _mapper.Map<Domain.Models.ChannelModels.Channel, ChannelDTO>(tryFindChannel);
        }
    }
}
