using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Tornado.Domain.Models.VideoModels;
using Tornado.Infrastructure.Services.Interfaces;
using Tornado.Infrastructure.Services.Settings;

namespace Tornado.Infrastructure.Services
{
	public class VideoPreviewService : IVideoPreviewService
	{
		private readonly IFFmpegAccessService _ffmpegAccessService;
		private readonly IImageUploadService _imageUploadService;
		private readonly VideoUploadDirectorySettings _directorySettings;
		private readonly IWebHostEnvironment _environment;


		public VideoPreviewService(
			IFFmpegAccessService fFmpegAccessService,
			IImageUploadService imageUploadService,
			IOptions<VideoUploadDirectorySettings> options,
			IWebHostEnvironment environment)
		{
			_ffmpegAccessService = fFmpegAccessService;
			_imageUploadService = imageUploadService;
			_directorySettings = options.Value;
			_environment = environment;
		}

		public async Task<string> GeneratePreviewUrlFromProvidedData(IFormFile? providedPreviewData, Video video)
		{
			if (providedPreviewData != null)
			{
				return await _imageUploadService.UploadImage(providedPreviewData, ImageType.VideoPreview);
			}

			var stubUrl = await _imageUploadService.UploadEmptyImage(ImageType.VideoPreview);

			var highestQualitySourceUrl = await GetHighestQualityVideoSourceUrlAvailable(video);

			if (highestQualitySourceUrl == "")
			{
				throw new Exception("Error occured while searching for source files");
			}

			await _ffmpegAccessService.ExecuteWithArguments(
				$"-y -ss 00:00:00 -i {highestQualitySourceUrl} -frames:v 1 -q:v 2 {stubUrl.Item1}" 
			);

			return stubUrl.Item2;
		}

		// finds the file with highest quality available for given video
		private Task<string> GetHighestQualityVideoSourceUrlAvailable(Video video)
		{
			var videoFileName = video.SourceFileName;

			var dirs = _directorySettings
				.GetType()
				.GetProperties()
				.Select(prop => prop.GetValue(_directorySettings) as string)
				.Skip(2)
				.ToList();

			// to start from best quality
			dirs.Reverse();

			foreach (var dir in dirs)
			{
				var theoreticalUploadPath = Path.Combine(
					_environment.WebRootPath,
					_directorySettings.Root,
					dir!,
					video.SourceFileName + ".mov");

				if (File.Exists(theoreticalUploadPath))
				{
					return Task.FromResult(theoreticalUploadPath);
				}
			}

			return Task.FromResult("");
		}
	}
}
