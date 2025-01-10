using Tornado.Domain.Models.ProfileModels;
using Tornado.Infrastructure.Interfaces.Repositories;

namespace Tornado.Infrastructure.Data.Repositories
{
    public class UserCommentsRepository : IUserCommentRepository
    {
        private readonly ApplicationDatabaseContext _databaseContext;
        public UserCommentsRepository(ApplicationDatabaseContext applicationDatabaseContext)
        {
            _databaseContext = applicationDatabaseContext;
        }

        public async Task AddAsync(UserComments entity, CancellationToken cancellationToken)
        {
            await _databaseContext.UserComments.AddAsync(entity, cancellationToken);
        }

        public Task DeleteAsync(UserComments entity, CancellationToken cancellationToken)
        {
            return Task.FromResult(_databaseContext.UserComments.Remove(entity));
        }

        public Task<IEnumerable<UserComments>> GetAllAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_databaseContext.UserComments.AsEnumerable());
        }

        public Task<UserComments?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(_databaseContext.UserComments.FirstOrDefault(user => user.Id == id));
        }

        public Task RemoveRangeAsync(IEnumerable<UserComments> entities, CancellationToken cancellationToken)
        {
            _databaseContext.UserComments.RemoveRange(entities);
            return Task.FromResult(true);
        }

        // entity is not directly updated
        public Task UpdateAsync(UserComments entity, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }
}
