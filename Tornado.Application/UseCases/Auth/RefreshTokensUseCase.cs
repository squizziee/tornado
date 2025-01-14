using Microsoft.Extensions.Logging;
using Tornado.Application.UseCases.Interfaces.Auth;
using Tornado.Contracts.Requests;
using Tornado.Infrastructure.Interfaces;
using Tornado.Infrastructure.Services.Interfaces;

namespace Tornado.Application.UseCases.Auth
{
    public class RefreshTokensUseCase : IRefreshTokensUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly ILogger<RefreshTokensUseCase> _logger;

        public RefreshTokensUseCase(
            IUnitOfWork unitOfWork,
            IJwtService jwtService,
            ILogger<RefreshTokensUseCase> logger)
        {
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<(string, string)> ExecuteAsync(string refreshToken, CancellationToken cancellationToken)
        {
            var tryFind = await _unitOfWork.UserRepository.FindByRefreshTokenAsync(refreshToken, cancellationToken);

            if (tryFind == null)
            {
                throw new Exception("No user for refresh token was found");
            }

            try
            {
                _unitOfWork.CreateTransaction();

                var newAccessToken = _jwtService.GenerateNewJwtToken(tryFind);
                var newRefreshToken = _jwtService.GenerateNewRefreshToken(tryFind);

                tryFind.RefreshToken = newRefreshToken;

                _unitOfWork.Save();
                _unitOfWork.Commit();

                return (newAccessToken, newRefreshToken);
            }
            catch (Exception)
            {
                _logger.LogError($"Failed to create new tokens for {tryFind.Email}");
                _logger.LogInformation($"Performing transaction rollback...");
            }

            return ("", "");
        }
    }
}
