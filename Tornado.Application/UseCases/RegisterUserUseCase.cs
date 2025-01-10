﻿using Microsoft.Extensions.Logging;
using Tornado.Application.UseCases.Interfaces;
using Tornado.Contracts.Requests;
using Tornado.Domain.Models.Auth;
using Tornado.Domain.Models.ProfileModels;
using Tornado.Infrastructure.Interfaces;

namespace Tornado.Application.UseCases
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private ILogger<RegisterUserUseCase> _logger;

        public RegisterUserUseCase(
            IUnitOfWork unitOfWork,
            ILogger<RegisterUserUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task ExecuteAsync(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var tryFind = await _unitOfWork.UserRepository.FindByEmailAsync(request.Email);

            if (tryFind != null)
            {
                throw new Exception($"Email {request.Email} is already registered");
            }

            try
            {
                _logger.LogInformation("Creating new transaction...");
                _unitOfWork.CreateTransaction();

                // new ids
                var newUserId = Guid.NewGuid();
                var newUserProfileId = Guid.NewGuid();
                var newUserRatingsId = Guid.NewGuid();
                var newUserCommentsId = Guid.NewGuid();

                var newUser = new User
                {
                    Id = newUserId,
                    Email = request.Email,
                    PasswordHash = SimpleHash(request.Password),
                    ProfileId = newUserProfileId,
                };

                var newUserProfile = new UserProfile
                {
                    Id = newUserProfileId,
                    Nickname = request.Nickname,
                    LastName = request.LastName,
                    FirstName = request.FirstName,
                    UserCommentsId = newUserCommentsId,
                    UserRatingsId = newUserRatingsId,
                };

                var newUserComments = new UserComments
                {
                    Id = newUserCommentsId,
                };

                var newUserRatings = new UserRatings
                {
                    Id = newUserRatingsId,
                };

                await _unitOfWork.UserRepository.AddAsync(newUser, cancellationToken);
                _logger.LogInformation($"New user with id {newUserId} added");

                await _unitOfWork.UserProfileRepository.AddAsync(newUserProfile, cancellationToken);
                _logger.LogInformation($"New user profile with id {newUserProfileId} added");

                await _unitOfWork.UserCommentRepository.AddAsync(newUserComments, cancellationToken);
                _logger.LogInformation($"New user comments with id {newUserCommentsId} added");

                await _unitOfWork.UserRatingsRepository.AddAsync(newUserRatings, cancellationToken);
                _logger.LogInformation($"New user ratings with id {newUserRatingsId} added");

                _logger.LogInformation($"Commiting transaction...");

                _unitOfWork.Save();
                _unitOfWork.Commit();

                _logger.LogInformation($"New user fully registered");

            } catch (Exception ex) {
                _logger.LogError("Exception occured while trying to register new user: " + ex.Message);
                _logger.LogInformation("Performing transaction rollback...");
                _unitOfWork.Rollback();
            }
        }

        private static string SimpleHash(string password)
        {
            return "hash" + password.ToUpper();
        }
    }
}
