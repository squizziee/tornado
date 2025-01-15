using Tornado.Application.UseCases.Interfaces.Channel;
using Tornado.Contracts.Requests.Channel;
using Tornado.Infrastructure.Interfaces;
using Tornado.Infrastructure.Services.Interfaces;

namespace Tornado.Application.UseCases.Channel
{
    public class CreateChannelUseCase : ICreateChannelUseCase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IImageUploadService _imageUploadService;

		public CreateChannelUseCase(IUnitOfWork unitOfWork, IImageUploadService imageUploadService)
		{
			_unitOfWork = unitOfWork;
			_imageUploadService = imageUploadService;
		}

		public async Task ExecuteAsync(CreateChannelRequest request, CancellationToken cancellationToken)
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

			if (tryFindProfile.ChannelId != null)
			{
				throw new Exception($"Channel for user with id of {request.UserId} was already created");
			}

			var newChannelId = Guid.NewGuid();
			var newChannel = new Domain.Models.ChannelModels.Channel
			{
				Id = newChannelId,
				Name = request.ChannelName,
				Description = request.ChannelDescription,
			};

			// upload pictures
			if (request.ChannelHeaderData != null)
			{
				newChannel.ChannelHeaderSourceUrl = 
					await _imageUploadService.UploadImage(request.ChannelHeaderData, ImageType.ChannelHeader);
			}

            if (request.ChannelAvatarData != null)
            {
                newChannel.ChannelAvatarSourceUrl =
                    await _imageUploadService.UploadImage(request.ChannelAvatarData, ImageType.ChannelAvatar);
            }

			// bind to user profile
			tryFindProfile.ChannelId = newChannelId;

			await _unitOfWork.ChannelRepository.AddAsync(newChannel, cancellationToken);
			_unitOfWork.Save();
        }
	}
}
