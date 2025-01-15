using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tornado.Contracts.Requests
{
    public record UploadVideoRequest
    {
        public required IFormFile VideoData { get; set; }
        public IFormFile? PreviewData { get; set; }
    }
}
