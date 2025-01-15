using Microsoft.AspNetCore.Http;

namespace Tornado.Infrastructure.Services.Interfaces
{
    public interface IImageUploadService
    {
        Task<string> UploadImage(IFormFile image, ImageType imageType);
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
