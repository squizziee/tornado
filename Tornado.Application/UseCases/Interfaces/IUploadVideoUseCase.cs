using Tornado.Contracts.Requests;

namespace Tornado.Application.UseCases.Interfaces
{
    public interface IUploadVideoUseCase
    {
        Task ExecuteAsync(MemoryStream videoData, CancellationToken cancellationToken);
    }
}
