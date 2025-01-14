using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tornado.Application.UseCases.Interfaces.Profile;
using Tornado.Contracts.Requests.Profile;
using Tornado.Domain.Models.ProfileModels;
using Tornado.Infrastructure.Interfaces;
using Tornado.Infrastructure.Services.Interfaces;

namespace Tornado.Application.UseCases.Profile
{
    public class UpdateUserProfileUseCase : IUpdateUserProfileUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageUploadService _imageUploadService;

        public UpdateUserProfileUseCase(
            IUnitOfWork unitOfWork, 
            IImageUploadService imageUploadService)
        {
            _unitOfWork = unitOfWork;
            _imageUploadService = imageUploadService;
        }

        public async Task ExecuteAsync(
            UpdateUserProfileRequest request, 
            CancellationToken cancellationToken)
        {
            var tryFindUser = await _unitOfWork.UserRepository
                .GetByIdAsync(request.UserId, cancellationToken);

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

            var newProfile = new UserProfile
            {
                Id = tryFindProfile.Id,
                Nickname = request.Nickname,
                FirstName = request.FirstName,
                LastName = request.LastName,
            };

            if (request.AvatarData != null)
            {
                // if avatar already set, delete avatar file
                if (tryFindProfile.AvatarUrl != null)
                {
                    await _imageUploadService.DeleteUploadedImage(tryFindProfile.AvatarUrl);
                }

                newProfile.AvatarUrl =  await _imageUploadService
                    .UploadImage(request.AvatarData, ImageType.Avatar);
            }
            else
            {
                newProfile.AvatarUrl = null;
            }

            await _unitOfWork.UserProfileRepository.UpdateAsync(newProfile, cancellationToken);
            _unitOfWork.Save();
        }
    }
}
