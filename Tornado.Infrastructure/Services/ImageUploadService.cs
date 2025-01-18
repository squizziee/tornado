using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tornado.Infrastructure.Services.Interfaces;
using Tornado.Infrastructure.Services.Settings;

namespace Tornado.Infrastructure.Services
{
	public class ImageUploadService : IImageUploadService
	{
		private readonly IConfiguration _configuration;
		private readonly ImageUploadSettings _uploadSettings;
		private readonly ILogger<ImageUploadService> _logger;
		private readonly IWebHostEnvironment _environment;
        private readonly HostSettings _hostSettings;

        public ImageUploadService(
			IConfiguration configuration, 
			IOptions<ImageUploadSettings> options,
			ILogger<ImageUploadService> logger,
            IOptions<HostSettings> hOptions,
            IWebHostEnvironment environment) 
		{ 
			_configuration = configuration;
			_uploadSettings = options.Value;
			_logger = logger;
			_environment = environment;
            _hostSettings = hOptions.Value;
        }


		public Task DeleteUploadedImage(string imageUrl)
		{
			if (File.Exists(imageUrl))
			{
                File.Delete(imageUrl);
            }	

			return Task.CompletedTask;
		}

        public Task<(string, string)> UploadEmptyImage(ImageType imageType)
        {
            var exactDir = imageType switch
            {
                ImageType.Avatar => _uploadSettings.Avatars,
                ImageType.VideoPreview => _uploadSettings.VideoPreviews,
                ImageType.ChannelHeader => _uploadSettings.ChannelHeaders,
                ImageType.ChannelAvatar => _uploadSettings.ChannelAvatars,
                _ => throw new Exception($"Unkwnown image type: {imageType}")
            };

            var uploadfFilename = Guid.NewGuid().ToString();

            var uploadPath = Path.Combine(
                _environment.WebRootPath,
                _uploadSettings.Root,
                exactDir,
                uploadfFilename + ".png"
            );

            _logger.LogInformation($"Starting image upload {uploadPath} of type {imageType}");

            using var stream = File.Create(uploadPath);

			//         var accessiblePath = Path.Combine(
			//            _hostSettings.BaseUrl,
			//            _uploadSettings.Root,
			//            exactDir,
			//            uploadfFilename + ".png"
			//);

			// kind of crappy but whatever
			var accessiblePath =
			   _hostSettings.BaseUrl + "/" +
			   _uploadSettings.Root.Replace("\\", "/") + "/" +
			   exactDir + "/" +
			   uploadfFilename + ".png";

            return Task.FromResult((uploadPath, accessiblePath));
        }

        public async Task<string> UploadImage(IFormFile image, ImageType imageType)
		{
			var exactDir = imageType switch
			{
				ImageType.Avatar => _uploadSettings.Avatars,
				ImageType.VideoPreview => _uploadSettings.VideoPreviews,
				ImageType.ChannelHeader => _uploadSettings.ChannelHeaders,
				ImageType.ChannelAvatar => _uploadSettings.ChannelAvatars,
				_ => throw new Exception($"Unkwnown image type: {imageType}")
            };

			var providedImageExtension = image.FileName.Split('.').Last();

			if (_uploadSettings.SupportedExtensions
					.Where(ext => ext == providedImageExtension)
					.FirstOrDefault() == null)
			{
				throw new Exception($"Unsupported image extension: {providedImageExtension}");
            }

			var uploadfFilename = Guid.NewGuid().ToString();

            var uploadPath = Path.Combine(
				_environment.WebRootPath,
				_uploadSettings.Root,
                exactDir,
				uploadfFilename + "." + providedImageExtension
			);

			_logger.LogInformation($"Starting image upload {uploadPath} of type {imageType}");

			using (var stream = File.Create(uploadPath))
			{
				await image.CopyToAsync(stream);
			}

            var accessiblePath =
               _hostSettings.BaseUrl + "/" +
               _uploadSettings.Root.Replace("\\", "/") + "/" +
               exactDir + "/" +
               uploadfFilename + providedImageExtension;

            return uploadPath;
		}
	}
}
