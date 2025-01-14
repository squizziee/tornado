using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tornado.Application.UseCases.Interfaces;
using Tornado.Contracts.Requests;
using Tornado.Contracts.Requests.Auth;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tornado.API.Controllers
{
	[Route("api/video")]
	[ApiController]
	public class VideoController : ControllerBase
	{
		private readonly IUploadVideoUseCase uploadVideoUseCase;

		public VideoController(IUploadVideoUseCase uploadVideoUseCase)
		{
			this.uploadVideoUseCase = uploadVideoUseCase;
		}

		// POST api/video/playback/init
		[HttpGet("playback/init")]
		public async Task<IActionResult> GetVideoMetadata([FromQuery] GetVideoMetadataRequest request, CancellationToken cancellationToken)
		{

			//await uploadVideoUseCase.ExecuteAsync(request, cancellationToken);

			return Ok("File uploaded");
		}

		// POST api/video/upload
		[HttpPost("upload")]
		[Consumes("multipart/form-data")]
		public async Task<IActionResult> UploadVideo([FromForm] UploadVideoRequest request, CancellationToken cancellationToken)
		{

			#pragma warning disable CS4014
			var memoryStream = new MemoryStream();
			request.VideoData.CopyTo(memoryStream);
			memoryStream.Position = 0;

			uploadVideoUseCase.ExecuteAsync(memoryStream, cancellationToken);
			#pragma warning restore CS4014

			return Ok("File uploaded");
		}


	}
}
