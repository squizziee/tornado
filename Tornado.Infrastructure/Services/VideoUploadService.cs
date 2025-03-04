﻿using Microsoft.Extensions.Logging;
using Tornado.Infrastructure.Services.Interfaces;
using FFMpegCore;
using FFMpegCore.Enums;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using System.Text;

namespace Tornado.Infrastructure.Services
{
	public class VideoUploadService : IVideoUploadService
	{
		private readonly ILogger<VideoUploadService> logger;
		private readonly IConfiguration configuration;
		private readonly IWebHostEnvironment _environment;
		private readonly IFFmpegAccessService _ffmpegAccessService;

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
			IConfiguration configuration,
			IWebHostEnvironment environment,
			IFFmpegAccessService ffmpegAccessService)
		{
			this.logger = logger;
			this.configuration = configuration;
			_environment = environment;
			_ffmpegAccessService = ffmpegAccessService;
		}

		public async Task<(string, TimeSpan)> Upload(
			MemoryStream videoData,
			CancellationToken cancellationToken)
		{

			// paths for different video formats
			var dirs = configuration
				.GetSection("VideoUpload")
				.Get<UploadDirectories>();

			// random file name
			var fileName = Guid
				.NewGuid()
				.ToString();

			var uploadPath = Path.Combine(
				_environment.WebRootPath,
				dirs!.Root,
				dirs!.Source,
				fileName);

			if (videoData.Length <= 0)
			{
				throw new Exception("Uploaded file size was zero");
			}

			// source file is uploaded in raw format
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

			await Lightning(uploadPath, fileName, closestPossibleDimensions, dirs);	

			return (fileName, mediaInfo.Duration);
		}

		// experiment
		private async Task Lightning(string srcFilePath,
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

			var overallTimer = Stopwatch.StartNew();
			var args = new StringBuilder($"-y -vsync 0 -hwaccel cuda -hwaccel_output_format cuda -i {srcFilePath} ");

			// encode video in every format equal and below the original dimensions
			for (int i = maxDimensionsIndex; i >= 0; i--)
			{
				var outputWidth = dimensions[i].Item1;
				var outputHeight = dimensions[i].Item2;

				// path to upload for current dimension
				var currentDimensionUploadPath = Path.Combine(
					_environment.WebRootPath,
					uploadDirectories.Root,
					mappedDirs[i]!,
					fileName + ".mov"
				);

				args.Append($"-vf scale_cuda={outputWidth}:{outputHeight} -c:a copy -c:v h264_nvenc {currentDimensionUploadPath} ");
			}

            logger.LogInformation($"Started uploading everything for {fileName}");

            await _ffmpegAccessService.ExecuteWithArguments(args.ToString());

            overallTimer.Stop();

			logger.LogInformation($"Upload for {fileName} finished. {maxDimensionsIndex + 1} videos encoded. Time elapsed: {overallTimer.ElapsedMilliseconds / 1000.0} s");
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

			var overallTimer = Stopwatch.StartNew();

			// encode video in every format equal and below the original dimensions
			for (int i = maxDimensionsIndex; i >= 0; i--)
			{
				var outputWidth = dimensions[i].Item1;
				var outputHeight = dimensions[i].Item2;

				// path to upload for current dimension
				var currentDimensionUploadPath = Path.Combine(
					_environment.WebRootPath,
					uploadDirectories.Root,
					mappedDirs[i]!,
					fileName + ".mov"
				);

				logger.LogInformation($"Started uploading {outputWidth}x{outputHeight} for {fileName}");

				var timer = Stopwatch.StartNew();

				await _ffmpegAccessService.ExecuteWithArguments(
					$"-y -vsync 0 -hwaccel cuda -hwaccel_output_format cuda -i {srcFilePath} " +
					$"-vf scale_cuda={outputWidth}:{outputHeight} -c:a copy -c:v h264_nvenc {currentDimensionUploadPath}"
				);

				timer.Stop();

				logger.LogInformation($"Ended uploading {outputWidth}x{outputHeight} for {fileName}. Time elapsed: {timer.ElapsedMilliseconds / 1000.0} s");
			}

			overallTimer.Stop();

			logger.LogInformation($"Upload for {fileName} finished. {maxDimensionsIndex + 1} videos encoded. Time elapsed: {overallTimer.ElapsedMilliseconds / 1000.0} s");
		}

		private async Task ProcessWithFFMpegMt(
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

			var overallTimer = Stopwatch.StartNew();

			var tasks = new Task[maxDimensionsIndex + 1];

			// encode video in every format equal and below the original dimensions
			for (int i = maxDimensionsIndex; i >= 0; i--)
			{
				var outputWidth = dimensions[i].Item1;
				var outputHeight = dimensions[i].Item2;

				// path to upload for current dimension
				var currentDimensionUploadPath = Path.Combine(
					_environment.WebRootPath,
					uploadDirectories.Root,
					mappedDirs[i]!,
					fileName + ".mov"
				);

				// encode with H.264 and AAC codecs with resizing
				tasks[i] = Task.Factory.StartNew(() =>
				{
					logger.LogInformation($"Started uploading {outputWidth}x{outputHeight} for {fileName}");

					var timer = Stopwatch.StartNew();

					FFMpegArguments
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
							//.WithFastStart()
					)
					.ProcessSynchronously();

					timer.Stop();

					logger.LogInformation($"Ended uploading {outputWidth}x{outputHeight} for {fileName}. Time elapsed: {timer.ElapsedMilliseconds / 1000.0} s");
				});
			}

			await Task.Factory.ContinueWhenAll(tasks, completedTasks =>		
			{
				overallTimer.Stop();
				logger.LogInformation($"Upload for {fileName} finished. {maxDimensionsIndex + 1} videos encoded. Time elapsed: {overallTimer.ElapsedMilliseconds / 1000.0} s");
			});
		}

		// closest (rounded upwards) standart dimension searched by height, 
		// e.g. 1263x701 -> 1280x720,
		// e.g. 1000x481 -> 1280x720.
		// Final result will most likely have black frame if it does
		// not suit standart dimensions or is vertical. This is done to preserve all of
		// the uploaded content. The algorithm is temprorary, will be
		// much more adaptable in the future.
		private static int FindClosestDimensions(int height)
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
