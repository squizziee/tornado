using Tornado.Contracts.Requests.Auth;

namespace Tornado.Application.UseCases.Interfaces.Auth
{
    public interface IRegisterUserUseCase
    {
        Task ExecuteAsync(RegisterUserRequest request, CancellationToken cancellationToken);
    }
}
