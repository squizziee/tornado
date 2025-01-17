using Microsoft.AspNetCore.Http;
using Tornado.Domain.Models.VideoModels;

namespace Tornado.Infrastructure.Services.Interfaces
{
    public interface IVideoPreviewService
    {
        // The desired behavior: if data is provided - upload. If not - generate with ffmpeg.
        // Then return image url
        Task<string> GeneratePreviewUrlFromProvidedData(IFormFile? providedPreviewData, Video video);
    }
}
