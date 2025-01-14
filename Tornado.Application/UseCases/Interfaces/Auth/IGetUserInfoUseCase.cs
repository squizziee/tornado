using Tornado.Contracts.DTO;
using Tornado.Contracts.Requests.Auth;

namespace Tornado.Application.UseCases.Interfaces.Auth
{
    public interface IGetUserInfoUseCase
    {
        Task<UserDTO> ExecuteAsync(GetUserInfoRequest request, CancellationToken cancellationToken);
    }
}
