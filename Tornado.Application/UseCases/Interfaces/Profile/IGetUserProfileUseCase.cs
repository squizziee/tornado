using Tornado.Contracts.DTO;
using Tornado.Contracts.Requests.Auth;
using Tornado.Contracts.Requests.Profile;

namespace Tornado.Application.UseCases.Interfaces.Profile
{
    public interface IGetUserProfileUseCase
    {
        Task<UserProfileDTO> ExecuteAsync(GetUserProfileRequest request, CancellationToken cancellationToken);
    }
}
