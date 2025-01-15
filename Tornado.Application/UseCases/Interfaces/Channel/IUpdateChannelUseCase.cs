using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tornado.Contracts.Requests.Channel;

namespace Tornado.Application.UseCases.Interfaces.Channel
{
    public interface IUpdateChannelUseCase
    {
        Task ExecuteAsync(UpdateChannelRequest request, CancellationToken cancellationToken);
    }
}
