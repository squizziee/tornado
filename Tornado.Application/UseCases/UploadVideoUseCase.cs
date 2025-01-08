using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tornado.Application.UseCases.Interfaces;
using Tornado.Contracts.Requests;
using Tornado.Infrastructure.Services.Interfaces;

namespace Tornado.Application.UseCases
{
    public class UploadVideoUseCase : IUploadVideoUseCase
    {
        private readonly ILogger<UploadVideoUseCase> logger;
        private readonly IVideoUploadService videoUploadService;

        public UploadVideoUseCase(
            ILogger<UploadVideoUseCase> logger, 
            IVideoUploadService videoUploadService)
        {
            this.logger = logger;
            this.videoUploadService = videoUploadService;
        }

        public async Task ExecuteAsync(UploadVideoRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var file = request.VideoData;
                await videoUploadService.Upload(file, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while trying to upload file: " +  ex.Message);
            }
        }
    }
}
