using Microsoft.Extensions.Logging;
using Tornado.Application.UseCases.Interfaces;
using Tornado.Contracts.Requests;
using Tornado.Domain.Models.VideoModels;
using Tornado.Infrastructure.Interfaces;
using Tornado.Infrastructure.Services.Interfaces;

namespace Tornado.Application.UseCases.Video
{
    public class UploadVideoUseCase : IUploadVideoUseCase
    {
        private readonly ILogger<UploadVideoUseCase> _logger;
        private readonly IVideoUploadService _videoUploadService;
        private readonly IVideoPreviewService _videoPreviewService;
        private readonly IUnitOfWork _unitOfWork;

        public UploadVideoUseCase(
            ILogger<UploadVideoUseCase> logger,
            IVideoUploadService videoUploadService,
            IVideoPreviewService videoPreviewService,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _videoUploadService = videoUploadService;
            _videoPreviewService = videoPreviewService;
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(UploadVideoRequest request, MemoryStream videoData, CancellationToken cancellationToken)
        {
            var tryFindUser = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (tryFindUser == null)
            {
                throw new Exception($"No user with id of {request.UserId} was found");
            }

            var tryFindProfile = await _unitOfWork.UserProfileRepository
                .GetByIdAsync(tryFindUser.ProfileId, cancellationToken);

            if (tryFindProfile == null)
            {
                throw new Exception($"No profile for user with id of {tryFindUser.ProfileId} was found");
            }

            if (tryFindProfile.ChannelId == null)
            {
                throw new Exception($"Channel for user with id of {request.UserId} was not created");
            }

            try
            {
                var newVideoid = Guid.NewGuid();

                var newVideo = new Domain.Models.VideoModels.Video
                {
                    Id = newVideoid,
                    ChannelId = (Guid) tryFindProfile.ChannelId,
                    Title = request.Title,
                    Description = request.Description,
                    CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                };

                var items = await _videoUploadService.Upload(videoData, cancellationToken);

                newVideo.SourceFileName = items.Item1;
                newVideo.Duration = items.Item2;

                // important to do this after video upload if preview was not provided in request
                newVideo.PreviewSourceUrl = await _videoPreviewService
                    .GeneratePreviewUrlFromProvidedData(request.PreviewData, newVideo);

                await _unitOfWork.VideoRepository.AddAsync(newVideo, cancellationToken);
                _unitOfWork.Save();
                
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while trying to upload file: " + ex.Message);
            }
        }
    }
}
