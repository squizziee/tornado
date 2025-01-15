using Tornado.Application.UseCases.Interfaces.Channel;
using Tornado.Contracts.Requests.Channel;
using Tornado.Infrastructure.Interfaces;
using Tornado.Infrastructure.Services.Interfaces;

namespace Tornado.Application.UseCases.Channel
{
    public class UpdateChannelUseCase : IUpdateChannelUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageUploadService _imageUploadService;

        public UpdateChannelUseCase(IUnitOfWork unitOfWork, IImageUploadService imageUploadService)
        {
            _unitOfWork = unitOfWork;
            _imageUploadService = imageUploadService;
        }

        public async Task ExecuteAsync(UpdateChannelRequest request, CancellationToken cancellationToken)
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

            var existingChannel = (await _unitOfWork.ChannelRepository
                .GetByIdAsync((Guid) tryFindProfile.ChannelId, cancellationToken))!;

            var updatedChannel = new Domain.Models.ChannelModels.Channel
            {
                Id = existingChannel.Id,
                Name = request.ChannelName,
                Description = request.ChannelDescription,
            };

            // upload/replace pictures
            if (request.ChannelHeaderData != null)
            {
                if (existingChannel.ChannelHeaderSourceUrl != null)
                {
                    await _imageUploadService.DeleteUploadedImage(existingChannel.ChannelHeaderSourceUrl);
                }

                updatedChannel.ChannelHeaderSourceUrl =
                    await _imageUploadService.UploadImage(request.ChannelHeaderData, ImageType.ChannelHeader);
            }

            if (request.ChannelAvatarData != null)
            {
                if (existingChannel.ChannelAvatarSourceUrl != null)
                {
                    await _imageUploadService.DeleteUploadedImage(existingChannel.ChannelAvatarSourceUrl);
                }

                updatedChannel.ChannelAvatarSourceUrl =
                    await _imageUploadService.UploadImage(request.ChannelAvatarData, ImageType.ChannelAvatar);
            }

            await _unitOfWork.ChannelRepository.UpdateAsync(updatedChannel, cancellationToken);
            _unitOfWork.Save();
        }
    }
}
