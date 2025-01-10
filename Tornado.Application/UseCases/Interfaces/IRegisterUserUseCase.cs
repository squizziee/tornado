using Tornado.Contracts.Requests;

namespace Tornado.Application.UseCases.Interfaces
{
    public interface IRegisterUserUseCase
    {
        Task ExecuteAsync(RegisterUserRequest request, CancellationToken cancellationToken);
    }
}
