using Tornado.Contracts.Requests.Profile;

namespace Tornado.Application.UseCases.Interfaces.Profile
{
    public interface IUpdateUserProfileUseCase
    {
        Task ExecuteAsync(UpdateUserProfileRequest request, CancellationToken cancellationToken);
    }
}
