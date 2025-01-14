using AutoMapper;
using Tornado.Application.UseCases.Interfaces.Profile;
using Tornado.Contracts.DTO;
using Tornado.Contracts.Requests.Profile;
using Tornado.Domain.Models.ProfileModels;
using Tornado.Infrastructure.Data;
using Tornado.Infrastructure.Interfaces;

namespace Tornado.Application.UseCases.Profile
{
    public class GetUserProfileUseCase : IGetUserProfileUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Mapper _mapper;
        public GetUserProfileUseCase(IUnitOfWork unitOfWork, Mapper mapper) { 
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserProfileDTO> ExecuteAsync(
            GetUserProfileRequest request, 
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

            return _mapper.Map<UserProfile, UserProfileDTO>(tryFindProfile);
        }
    }
}
