using Tornado.Application.UseCases.Interfaces;
using Tornado.Contracts.Requests;
using Tornado.Infrastructure.Interfaces;
using Tornado.Infrastructure.Services.Interfaces;

namespace Tornado.Application.UseCases
{
    public class LoginWithEmailAndPasswordUseCase : ILoginWithEmailAndPasswordUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHashingService _passwordHashingService;

        public LoginWithEmailAndPasswordUseCase(
            IUnitOfWork unitOfWork, 
            IJwtService jwtService, 
            IPasswordHashingService passwordHashingService)
        {
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
            _passwordHashingService = passwordHashingService;
        }

        public async Task<(string, string)> ExecuteAsync(LoginWithEmailAndPasswordRequest request, CancellationToken cancellationToken)
        {
            var tryFind = await _unitOfWork.UserRepository.FindByEmailAsync(request.Email, cancellationToken);

            if (tryFind == null)
            {
                throw new Exception($"No user with email of {request.Email} was found");
            }

            if (!_passwordHashingService.Verify(request.Password, tryFind.PasswordHash))
            {
                throw new Exception($"No valid password for {request.Email} was provided");
            }

            var newAccessToken = _jwtService.GenerateNewJwtToken(tryFind);
            var newRefreshToken = _jwtService.GenerateNewRefreshToken(tryFind);

            try
            {
                tryFind.RefreshToken = newRefreshToken;

                _unitOfWork.Save();
            }
            catch (Exception)
            {
                return ("", "");
            }
            
            return (newAccessToken, newRefreshToken);
        }
    }
}
