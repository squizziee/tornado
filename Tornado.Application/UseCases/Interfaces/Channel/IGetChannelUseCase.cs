using Tornado.Contracts.DTO;
using Tornado.Contracts.Requests.Channel;

namespace Tornado.Application.UseCases.Interfaces.Channel
{
    public interface IGetChannelUseCase
    {
        Task<ChannelDTO> ExecuteAsync(GetChannelRequest request, CancellationToken cancellationToken);
    }
}
