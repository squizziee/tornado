using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tornado.Application.UseCases.Interfaces.Auth;
using Tornado.Application.UseCases.Interfaces.Profile;
using Tornado.Contracts.Requests.Profile;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tornado.API.Controllers
{
    [Route("api/profile")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IGetUserProfileUseCase _getUserProfileUseCase;
        private readonly IUpdateUserProfileUseCase _updateProfileUseCase;
        public UserProfileController(
            IGetUserProfileUseCase getUserProfileUseCase,
            IUpdateUserProfileUseCase updateProfileUseCase) {
            _getUserProfileUseCase = getUserProfileUseCase;
            _updateProfileUseCase = updateProfileUseCase;
        }

        // GET api/profile?id=...
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetUserProfileRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var profile = await _getUserProfileUseCase.ExecuteAsync(request, cancellationToken);
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // PUT api/profile?id=...
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put([FromForm] UpdateUserProfileRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _updateProfileUseCase.ExecuteAsync(request, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
