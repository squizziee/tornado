using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tornado.Infrastructure.Services.Interfaces;
using Tornado.Infrastructure.Services.Settings;
using static System.Net.Mime.MediaTypeNames;

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

        public Task<string> UploadEmptyImage(ImageType imageType)
        {
            var exactDir = imageType switch
            {
                ImageType.Avatar => _settings.Avatars,
                ImageType.VideoPreview => _settings.VideoPreviews,
                ImageType.ChannelHeader => _settings.ChannelHeaders,
                ImageType.ChannelAvatar => _settings.ChannelAvatars,
                _ => throw new Exception($"Unkwnown image type: {imageType}")
            };

            var uploadfFilename = Guid.NewGuid().ToString();

            var uploadPath = Path.Combine(
                _environment.WebRootPath,
                _settings.Root,
                exactDir,
                uploadfFilename + ".png"
            );

            _logger.LogInformation($"Starting image upload {uploadPath} of type {imageType}");

            using var stream = File.Create(uploadPath);

            return Task.FromResult(uploadPath);
        }

        public async Task<string> UploadImage(IFormFile image, ImageType imageType)
		{
			var exactDir = imageType switch
			{
				ImageType.Avatar => _settings.Avatars,
				ImageType.VideoPreview => _settings.VideoPreviews,
				ImageType.ChannelHeader => _settings.ChannelHeaders,
				ImageType.ChannelAvatar => _settings.ChannelAvatars,
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
}
