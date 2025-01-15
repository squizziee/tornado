using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tornado.Application.UseCases.Interfaces.Channel;
using Tornado.Contracts.Requests.Channel;
using Tornado.Infrastructure.Interfaces;
using Tornado.Infrastructure.Services.Interfaces;

namespace Tornado.Application.UseCases.Channel
{
	public class DeleteChannelUseCase : IDeleteChannelUseCase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IImageUploadService _imageUploadService;
		private readonly ILogger<DeleteChannelUseCase> _logger;

		public DeleteChannelUseCase(
			IUnitOfWork unitOfWork, 
			IImageUploadService imageUploadService, 
			ILogger<DeleteChannelUseCase> logger)
		{
			_unitOfWork = unitOfWork;
			_imageUploadService = imageUploadService;
			_logger = logger;
		}

		public async Task ExecuteAsync(DeleteChannelRequest request, CancellationToken cancellationToken)
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
				.GetByIdAsync((Guid)tryFindProfile.ChannelId, cancellationToken))!;

			_logger.LogInformation($"Started channel deletion for user of id {tryFindUser.Id}");

			tryFindProfile.ChannelId = null;

            await _unitOfWork.ChannelRepository.DeleteAsync(existingChannel, cancellationToken);
            _unitOfWork.Save();

            _logger.LogInformation($"Ended channel deletion for user of id {tryFindUser.Id}");

            _logger.LogInformation($"Started channel data (images/videos) deletion for user of id {tryFindUser.Id}");
            // delete pictures
            if (existingChannel.ChannelHeaderSourceUrl != null)
			{
				await _imageUploadService.DeleteUploadedImage(existingChannel.ChannelHeaderSourceUrl);
			}

            if (existingChannel.ChannelAvatarSourceUrl != null)
            {
                await _imageUploadService.DeleteUploadedImage(existingChannel.ChannelAvatarSourceUrl);
            }

			#pragma warning disable CS4014
            DeleteChannelVideoFiles(existingChannel);
			#pragma warning restore CS4014
        }

		private async Task DeleteChannelVideoFiles(Domain.Models.ChannelModels.Channel channel)
		{
			foreach (var video in channel.Videos)
			{
				await _imageUploadService.DeleteUploadedImage(video.PreviewSourceUrl);
			}
            _logger.LogInformation($"Ended channel data (images/videos) deletion");
        }
	}
}
