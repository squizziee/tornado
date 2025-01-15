﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tornado.Contracts.Requests.Channel;

namespace Tornado.Application.UseCases.Interfaces.Channel
{
    public interface ICreateChannelUseCase
    {
        Task ExecuteAsync(CreateChannelRequest request, CancellationToken cancellationToken);
    }
}
