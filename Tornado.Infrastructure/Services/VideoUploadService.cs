using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Tornado.Infrastructure.Services.Interfaces;
using FFMpegCore;
using Microsoft.Extensions.Configuration;
using FFMpegCore.Enums;

namespace Tornado.Infrastructure.Services
{
	public class VideoUploadService : IVideoUploadService
	{
		private readonly ILogger<VideoUploadService> logger;
		private readonly IConfiguration configuration;
		private static readonly (int, int)[] dimensions = [
			(256, 144),
			(426, 240),
			(480, 360),
			(640, 480),
			(1280, 720),
			(1920, 1080),
			(3440, 1440),
			(3840, 2160),
		];

		public VideoUploadService(
			ILogger<VideoUploadService> logger,
			IConfiguration configuration)
		{
			this.logger = logger;
			this.configuration = configuration;
        }

		public async Task<string> Upload(
			IFormFile videoData,
			CancellationToken cancellationToken)
		{

			// paths for different video formats
			var dirs = configuration
				.GetSection("VideoUpload")
				.Get<UploadDirectories>();

			var fileName = Guid
				.NewGuid()
				.ToString();

			var uploadPath = Path.Combine(
				Directory.GetCurrentDirectory(),
				dirs!.Root,
				dirs!.Source,
				fileName);

			if (videoData.Length <= 0)
			{
                throw new Exception("Uploaded file size was zero");
			}

			using (var stream = File.Create(uploadPath))
			{
				logger.LogInformation($"Started new source file upload on {uploadPath}");

				await videoData.CopyToAsync(stream, cancellationToken);

				logger.LogInformation($"Ended source file upload on {uploadPath}");
			}

			var mediaInfo = FFProbe.Analyse(uploadPath);

			var videoHeight = mediaInfo.VideoStreams.First().Height;
			var videoWidth = mediaInfo.VideoStreams.First().Width;

            var closestPossibleDimensions = FindClosestDimensions(videoHeight);

			if (closestPossibleDimensions == -1)
			{
                throw new Exception($"Unsupported resolution was provided: {videoWidth}x{videoHeight}");
            }

			await ProcessWithFFMpeg(uploadPath, fileName, closestPossibleDimensions, dirs);	

			return fileName;
		}

		// processing with ffmpegcore
		private async Task ProcessWithFFMpeg(
			string srcFilePath,
			string fileName,
			int maxDimensionsIndex, 
			UploadDirectories uploadDirectories)
		{
			// map UploadDirectories as List sorted from the smallest resolution to the biggest
			// skipping Root and Source
			var mappedDirs = uploadDirectories
				.GetType()
				.GetProperties()
				.Select(prop => prop.GetValue(uploadDirectories) as string)
				.Skip(2)
				.ToList();

			// encode video in every format equal and below the original dimensions
            for (int i = maxDimensionsIndex; i >= 0; i--)
			{
				var outputWidth = dimensions[i].Item1;
				var outputHeight = dimensions[i].Item2;

                var currentDimensionUploadPath = Path.Combine(
					Directory.GetCurrentDirectory(),
                    uploadDirectories.Root,
                    mappedDirs[i]!,
					fileName + ".mov"
                );

                logger.LogInformation($"Started uploading {outputWidth}x{outputHeight} for {fileName}");

                await FFMpegArguments
					.FromFileInput(srcFilePath)
					.OutputToFile(
                        currentDimensionUploadPath,
						false,
						options => options
							.WithVideoCodec(VideoCodec.LibX264)
							.WithAudioCodec(AudioCodec.Aac)
							.WithVideoFilters(filterOptions => filterOptions
								.Scale(outputWidth, outputHeight)
							)
							.WithFastStart()
					//.WithVideoFilters($"scale={outputWidth}:{outputHeight}:force_original_aspect_ratio=decrease,pad={outputWidth}:{outputHeight}:(ow-iw)/2:(oh-ih)/2")
					)
					.ProcessAsynchronously();
					;

                logger.LogInformation($"Ended uploading {outputWidth}x{outputHeight} for {fileName}");
            }
		}

		private int FindClosestDimensions(int height)
		{
			var counter = 0;
			foreach (var dimension in dimensions)
			{
				if (dimension.Item1 >= height) return counter;
				++counter;
			}

			return -1;
		}
	}

	internal record UploadDirectories
	{
		public string Root { get; set; }
		public string Source { get; set; }
		public string Q144p { get; set; }
		public string Q240p { get; set; }
		public string Q360p { get; set; }
		public string Q480p { get; set; }
		public string Q720p { get; set; }
		public string Q1080p { get; set; }
		public string Q1440p { get; set; }
		public string Q2160p { get; set; }
	}

	//internal sealed class VideoQualityStandartDimensions
	//{
	//	public static (int, int) Q144p = (256, 144);
	//	public static (int, int) Q240p = (426, 240);
	//	public static (int, int) Q360p = (480, 360);
	//	public static (int, int) Q480p = (640, 480);
	//	public static (int, int) Q720p = (1280, 720);
	//	public static (int, int) Q1080p = (1920, 1080);
	//	public static (int, int) Q1440p = (3440, 1440);
	//	public static (int, int) Q2160p = (3840, 2160);
	//}
}
