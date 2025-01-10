using Tornado.Contracts.Requests;

namespace Tornado.Application.UseCases.Interfaces
{
    public interface IUploadVideoUseCase
    {
        Task ExecuteAsync(UploadVideoRequest request, CancellationToken cancellationToken);
    }
}
