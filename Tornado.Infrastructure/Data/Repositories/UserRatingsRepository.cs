using Tornado.Domain.Models.ProfileModels;
using Tornado.Infrastructure.Interfaces.Repositories;

namespace Tornado.Infrastructure.Data.Repositories
{
    public class UserRatingsRepository : IUserRatingsRepository
    {
        private readonly ApplicationDatabaseContext _databaseContext;
        public UserRatingsRepository(ApplicationDatabaseContext applicationDatabaseContext)
        {
            _databaseContext = applicationDatabaseContext;
        }

        public async Task AddAsync(UserRatings entity, CancellationToken cancellationToken)
        {
            await _databaseContext.UserRatings.AddAsync(entity, cancellationToken);
        }

        public Task DeleteAsync(UserRatings entity, CancellationToken cancellationToken)
        {
            return Task.FromResult(_databaseContext.UserRatings.Remove(entity));
        }

        public Task<IEnumerable<UserRatings>> GetAllAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_databaseContext.UserRatings.AsEnumerable());
        }

        public Task<UserRatings?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(_databaseContext.UserRatings.FirstOrDefault(user => user.Id == id));
        }

        public Task RemoveRangeAsync(IEnumerable<UserRatings> entities, CancellationToken cancellationToken)
        {
            _databaseContext.UserRatings.RemoveRange(entities);
            return Task.FromResult(true);
        }

        // entity is not directly updated
        public Task UpdateAsync(UserRatings entity, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }
}
