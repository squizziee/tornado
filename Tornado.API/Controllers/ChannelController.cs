using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tornado.Contracts.Requests.Auth;

namespace Tornado.API.Controllers
{
    [Route("api/channel")]
    [ApiController]
    public class ChannelController : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateChannel([FromForm] RegisterUserRequest request, CancellationToken cancellationToken)
        {
            

            return Ok();
        }
    }
}
