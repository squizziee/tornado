using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tornado.Infrastructure.Services.Interfaces
{
    public interface IVideoUploadService
    {
        Task<string> Upload(IFormFile videoData, CancellationToken cancellationToken);
    }
}
