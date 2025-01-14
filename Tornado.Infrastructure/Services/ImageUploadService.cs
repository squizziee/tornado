using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tornado.Infrastructure.Services.Interfaces;

namespace Tornado.Infrastructure.Services
{
	public class ImageUploadService : IImageUploadService
	{
		private readonly IConfiguration _configuration;
		private readonly ImageUploadSettings _settings;
		private readonly ILogger<ImageUploadService> _logger;
		private readonly IWebHostEnvironment _environment;
		public ImageUploadService(
			IConfiguration configuration, 
			IOptions<ImageUploadSettings> options,
			ILogger<ImageUploadService> logger,
            IWebHostEnvironment environment) 
		{ 
			_configuration = configuration;
			_settings = options.Value;
			_logger = logger;
			_environment = environment;
		}


		public Task DeleteUploadedImage(string imageUrl)
		{
			if (File.Exists(imageUrl))
			{
                File.Delete(imageUrl);
            }	

			return Task.CompletedTask;
		}

		public async Task<string> UploadImage(IFormFile image, ImageType imageType)
		{
			var exactDir = imageType switch
			{
				ImageType.Avatar => _configuration["ImageUpload:Avatars"]!,
				ImageType.VideoPreview => _configuration["ImageUpload:VideoPreviews"]!,
				_ => throw new Exception($"Unkwnown image type: {imageType}")
            };

			var providedImageExtension = image.FileName.Split('.').Last();

			if (_settings.SupportedExtensions
					.Where(ext => ext == providedImageExtension)
					.FirstOrDefault() == null)
			{
				throw new Exception($"Unsupported image extension: {providedImageExtension}");
            }

			var uploadfFilename = Guid.NewGuid().ToString();

            var uploadPath = Path.Combine(
				_environment.WebRootPath,
				_configuration["ImageUpload:Root"]!,
                exactDir,
				uploadfFilename + "." + providedImageExtension
			);

			_logger.LogInformation($"Starting image upload {uploadPath} of type {imageType}");

			using (var stream = File.Create(uploadPath))
			{
				await image.CopyToAsync(stream);
			}

			return uploadPath;
		}
	}

	public record ImageUploadSettings
	{
		public required string Root { get; set; }
		public required string Avatars { get; set; }
		public required string VideoPreviews { get; set; }
		public required string[] SupportedExtensions { get; set; }
	}
}
