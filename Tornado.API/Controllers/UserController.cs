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
        private IRegisterUserUseCase _registerUserUseCase;
        private ILoginWithEmailAndPasswordUseCase _loginWithEmailAndPasswordUseCase;

        private CookieOptions _accessTokenCookieOptions;
        private CookieOptions _refreshTokenCookieOptions;

        public UserController(
            IRegisterUserUseCase registerUserUseCase,
            ILoginWithEmailAndPasswordUseCase loginWithEmailAndPasswordUseCase,
            IConfiguration configuration) { 
            _registerUserUseCase = registerUserUseCase;
            _loginWithEmailAndPasswordUseCase = loginWithEmailAndPasswordUseCase;

            _accessTokenCookieOptions = new()
            {
                Expires = DateTimeOffset.UtcNow.AddMinutes(double.Parse(configuration["Jwt:AccessToken:ExpirationTimeInMinutes"]!)),
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.None
            };

            _refreshTokenCookieOptions = new()
            {
                Expires = DateTimeOffset.UtcNow.AddMinutes(double.Parse(configuration["Jwt:RefreshToken:ExpirationTimeInDays"]!)),
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

                //var response = new SuccessfulLoginResponse
                //{
                //    AccessToken = tokens.Item1,
                //    RefreshToken = tokens.Item2
                //};

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
            
            //try
            //{
            //    var tokens = await _loginWithEmailAndPasswordUseCase.ExecuteAsync(request, cancellationToken);

            //    if (tokens.Item1 == "" && tokens.Item2 == "")
            //    {
            //        return Unauthorized();
            //    }

            //    var response = new SuccessfulLoginResponse
            //    {
            //        AccessToken = tokens.Item1,
            //        RefreshToken = tokens.Item2
            //    };

            //    return Ok(response);
            //}
            //catch (Exception ex)
            //{
            //    return Unauthorized(ex.Message);
            //}

            return Ok();

        }
    }
}
