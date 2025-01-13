using AutoMapper;
using Microsoft.Extensions.Logging;
using Tornado.Application.UseCases.Interfaces;
using Tornado.Contracts.DTO;
using Tornado.Contracts.Requests;
using Tornado.Domain.Models.Auth;
using Tornado.Infrastructure.Interfaces;
using Tornado.Infrastructure.Services.Interfaces;

namespace Tornado.Application.UseCases
{
    public class GetUserInfoUseCase : IGetUserInfoUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Mapper _mapper;

        public GetUserInfoUseCase(IUnitOfWork unitOfWork, Mapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDTO> ExecuteAsync(GetUserInfoRequest request, CancellationToken cancellationToken)
        {
            var tryFind = await _unitOfWork.UserRepository.GetByIdAsync(request.Id, cancellationToken);

            if (tryFind == null)
            {
                throw new Exception($"No user with id of {request.Id} was found");
            }

            var fullInfo = await _unitOfWork.UserRepository.GetWithProfileAsync(tryFind!, cancellationToken);

            if (fullInfo == null)
            {
                throw new Exception($"No user profile for user with id id of {request.Id} was found");
            }

            return _mapper.Map<User, UserDTO>(fullInfo);
        }
    }
}
