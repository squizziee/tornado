using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tornado.Contracts.Requests.Profile;

namespace Tornado.Application.UseCases.Interfaces.Profile
{
    public interface IDeleteUserProfileUseCase
    {
        Task ExecuteAsync(DeleteUserProfileRequest request, CancellationToken cancellationToken);
    }
}
