namespace Tornado.Infrastructure.Services.Interfaces
{
    public interface IFFmpegAccessService
    {
        Task ExecuteWithArguments(string args);
    }
}
