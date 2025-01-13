using Microsoft.EntityFrameworkCore;
using Tornado.Domain.Models.Auth;
using Tornado.Infrastructure.Interfaces.Repositories;

namespace Tornado.Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDatabaseContext _databaseContext;
        public UserRepository(ApplicationDatabaseContext applicationDatabaseContext) { 
            _databaseContext = applicationDatabaseContext;
        }

        public async Task AddAsync(User entity, CancellationToken cancellationToken)
        {
            await _databaseContext.Users.AddAsync(entity, cancellationToken);
        }

        public Task DeleteAsync(User entity, CancellationToken cancellationToken)
        {
            return Task.FromResult(_databaseContext.Users.Remove(entity));
        }

        public Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return Task.FromResult(_databaseContext.Users.FirstOrDefault(user => user.Email == email));
        }

        public Task<User?> FindByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            return Task.FromResult(_databaseContext.Users.FirstOrDefault(user => user.RefreshToken == refreshToken));
        }

        public Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_databaseContext.Users.AsEnumerable());
        }

        public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(_databaseContext.Users.FirstOrDefault(user => user.Id == id));
        }

        // includes profile with only primitive properties
        public async Task<User?> GetWithProfileAsync(User user, CancellationToken cancellationToken)
        {
            return await _databaseContext.Users
                .Where(u => u.Id == user.Id)
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }

        public Task RemoveRangeAsync(IEnumerable<User> entities, CancellationToken cancellationToken)
        {
            _databaseContext.Users.RemoveRange(entities);
            return Task.FromResult(true);
        }

        public async Task UpdateAsync(User entity, CancellationToken cancellationToken)
        {
            var itemFound = _databaseContext.Users
                .Where(user => user.Id == entity.Id)
                .FirstOrDefault();

            if (itemFound == null)
            {
                throw new Exception($"No user entity with id of {entity.Id} was found");
            }

            // only these three should be able to change
            itemFound.PasswordHash = entity.PasswordHash;
            itemFound.RefreshToken = entity.RefreshToken;
            itemFound.Role = entity.Role;

            await _databaseContext.SaveChangesAsync(cancellationToken);
        }
    }
}
