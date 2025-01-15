using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tornado.Application.UseCases.Interfaces.Channel;
using Tornado.Contracts.Requests.Auth;
using Tornado.Contracts.Requests.Channel;

namespace Tornado.API.Controllers
{
	[Route("api/channel")]
	[ApiController]
	public class ChannelController : ControllerBase
	{
		private readonly ICreateChannelUseCase _createChannelUseCase;
		private readonly IUpdateChannelUseCase _updateChannelUseCase;
		private readonly IDeleteChannelUseCase _deleteChannelUseCase;
		private readonly IGetChannelUseCase _getChannelUseCase;
		public ChannelController(
			ICreateChannelUseCase createChannelUseCase, 
			IUpdateChannelUseCase updateChannelUseCase, 
			IDeleteChannelUseCase deleteChannelUseCase,
			IGetChannelUseCase getChannelUseCase) { 
			_createChannelUseCase = createChannelUseCase;
			_updateChannelUseCase = updateChannelUseCase;
			_deleteChannelUseCase = deleteChannelUseCase;
			_getChannelUseCase = getChannelUseCase;
		}

        [HttpGet]
        public async Task<IActionResult> GetChannel(
            [FromQuery] GetChannelRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                var channel = await _getChannelUseCase.ExecuteAsync(request, cancellationToken);
                return Ok(channel);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("create")]
		//[Authorize]
		public async Task<IActionResult> CreateChannel(
			[FromForm] CreateChannelRequest request, 
			CancellationToken cancellationToken)
		{
			try
			{
				await _createChannelUseCase.ExecuteAsync(request, cancellationToken);
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPut]
		[Authorize]
		public async Task<IActionResult> UpdateChannel(
			[FromForm] UpdateChannelRequest request,
			CancellationToken cancellationToken)
		{
			try
			{
				await _updateChannelUseCase.ExecuteAsync(request, cancellationToken);
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpDelete]
		[Authorize]
		public async Task<IActionResult> DeleteChannel(
			[FromQuery] DeleteChannelRequest request,
			CancellationToken cancellationToken)
		{
			try
			{
				#pragma warning disable CS4014
				_deleteChannelUseCase.ExecuteAsync(request, cancellationToken);
				#pragma warning restore CS4014
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
