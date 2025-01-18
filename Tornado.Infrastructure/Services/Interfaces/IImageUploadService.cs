using Microsoft.AspNetCore.Http;

namespace Tornado.Infrastructure.Services.Interfaces
{
    public interface IImageUploadService
    {
        Task<string> UploadImage(IFormFile image, ImageType imageType);
        // (real absolute path, relative to host path) e.g. (C:\..., https://localhost:1234/)
        Task<(string, string)> UploadEmptyImage(ImageType imageType);
        Task DeleteUploadedImage(string imageUrl);
    }

    public enum ImageType
    {
        Avatar,
        VideoPreview,
        ChannelHeader,
        ChannelAvatar,
    }
}
