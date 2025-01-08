using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tornado.Contracts.Requests;

namespace Tornado.Application.UseCases.Interfaces
{
    public interface IUploadVideoUseCase
    {
        Task ExecuteAsync(UploadVideoRequest request, CancellationToken cancellationToken);
    }
}
