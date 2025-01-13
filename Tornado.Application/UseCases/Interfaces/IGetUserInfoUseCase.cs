using Tornado.Contracts.DTO;
using Tornado.Contracts.Requests;

namespace Tornado.Application.UseCases.Interfaces
{
    public interface IGetUserInfoUseCase
    {
        Task<UserDTO> ExecuteAsync(GetUserInfoRequest request, CancellationToken cancellationToken);
    }
}
