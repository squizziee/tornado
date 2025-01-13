using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tornado.API.Attributes;
using Tornado.Application.UseCases.Interfaces;
using Tornado.Contracts.Requests;
using Tornado.Contracts.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tornado.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRegisterUserUseCase _registerUserUseCase;
        private readonly ILoginWithEmailAndPasswordUseCase _loginWithEmailAndPasswordUseCase;
        private readonly IRefreshTokensUseCase _refreshTokensUseCase;

        private readonly CookieOptions _accessTokenCookieOptions;
        private readonly CookieOptions _refreshTokenCookieOptions;

        public UserController(
            IRegisterUserUseCase registerUserUseCase,
            ILoginWithEmailAndPasswordUseCase loginWithEmailAndPasswordUseCase,
            IRefreshTokensUseCase refreshTokensUseCase,
            IConfiguration configuration) { 
            _registerUserUseCase = registerUserUseCase;
            _loginWithEmailAndPasswordUseCase = loginWithEmailAndPasswordUseCase;
            _refreshTokensUseCase = refreshTokensUseCase;

            _accessTokenCookieOptions = new()
            {
                Expires = DateTimeOffset.UtcNow.AddMinutes(
                    double.Parse(configuration["Jwt:AccessToken:ExpirationTimeInMinutes"]!)),
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.None
            };

            _refreshTokenCookieOptions = new()
            {
                Expires = DateTimeOffset.UtcNow.AddDays(
                    double.Parse(configuration["Jwt:RefreshToken:ExpirationTimeInDays"]!)),
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.None
            };
        }

        

        // POST api/user/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _registerUserUseCase.ExecuteAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }

            return Ok();           
        }

        // POST api/user/login
        [HttpPost("login")]
        public async Task<IActionResult> LoginWithEmailAndPassword([FromForm] LoginWithEmailAndPasswordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var tokens = await _loginWithEmailAndPasswordUseCase.ExecuteAsync(request, cancellationToken);

                if (tokens.Item1 == "" && tokens.Item2 == "")
                {
                    return Unauthorized();
                }

                Response.Cookies.Append("accessToken", tokens.Item1, _accessTokenCookieOptions);
                Response.Cookies.Append("refreshToken", tokens.Item2, _refreshTokenCookieOptions);

                return Ok();
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
            
        }

        [HttpPost("refresh")]
        [AuthorizeWithRefreshToken]
        public async Task<IActionResult> TryRefreshTokens([FromForm] CancellationToken cancellationToken)
        {
            try
            {
                var tokens = await _refreshTokensUseCase.ExecuteAsync(Request.Cookies["refreshToken"]!, cancellationToken);

                if (tokens.Item1 == "" && tokens.Item2 == "")
                {
                    return Unauthorized();
                }

                Response.Cookies.Append("accessToken", tokens.Item1, _accessTokenCookieOptions);
                Response.Cookies.Append("refreshToken", tokens.Item2, _refreshTokenCookieOptions);

                return Ok();
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("test")]
        [Authorize(Roles = "Viewer")]
        public async Task<IActionResult> Test()
        {
            return Ok();
        }
    }
}
