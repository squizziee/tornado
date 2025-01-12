using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tornado.Contracts.Requests;
using Tornado.Contracts.Responses;

namespace Tornado.Application.UseCases.Interfaces
{
    public interface ILoginWithEmailAndPasswordUseCase
    {
        Task<(string, string)> ExecuteAsync(LoginWithEmailAndPasswordRequest request, CancellationToken cancellationToken);
    }
}
