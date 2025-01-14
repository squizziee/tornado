using Tornado.Contracts.Requests;

namespace Tornado.Application.UseCases.Interfaces.Auth
{
    public interface IRefreshTokensUseCase
    {
        Task<(string, string)> ExecuteAsync(string refreshToken, CancellationToken cancellationToken);
    }
}
