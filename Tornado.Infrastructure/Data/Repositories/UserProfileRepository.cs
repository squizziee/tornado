using Tornado.Domain.Models.ProfileModels;
using Tornado.Infrastructure.Interfaces.Repositories;

namespace Tornado.Infrastructure.Data.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly ApplicationDatabaseContext _databaseContext;
        public UserProfileRepository(ApplicationDatabaseContext applicationDatabaseContext) {
            _databaseContext = applicationDatabaseContext;
        }

        public async Task AddAsync(UserProfile entity, CancellationToken cancellationToken)
        {
            await _databaseContext.UserProfiles.AddAsync(entity, cancellationToken);
        }

        public Task DeleteAsync(UserProfile entity, CancellationToken cancellationToken)
        {
            return Task.FromResult(_databaseContext.UserProfiles.Remove(entity));
        }

        public Task<IEnumerable<UserProfile>> GetAllAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_databaseContext.UserProfiles.AsEnumerable());
        }

        public Task<UserProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(_databaseContext.UserProfiles.FirstOrDefault(user => user.Id == id));
        }

        public Task RemoveRangeAsync(IEnumerable<UserProfile> entities, CancellationToken cancellationToken)
        {
            _databaseContext.UserProfiles.RemoveRange(entities);
            return Task.FromResult(true);
        }

        public async Task UpdateAsync(UserProfile entity, CancellationToken cancellationToken)
        {
            var itemFound = _databaseContext.UserProfiles
               .Where(profile => profile.Id == entity.Id)
               .FirstOrDefault();

            if (itemFound == null)
            {
                throw new Exception($"No profile entity with id of {entity.Id} was found");
            }

            // only these five should be able to change
            itemFound.Nickname = entity.Nickname;
            itemFound.FirstName = entity.FirstName;
            itemFound.LastName = entity.LastName;
            itemFound.AvatarUrl = entity.AvatarUrl;

            if (itemFound.ChannelId != null && itemFound.Channel != null)
            {
                throw new Exception($"Can not update channel id that was already set. Profile id - {itemFound.Id}");
            }

            itemFound.ChannelId = entity.ChannelId;

            await _databaseContext.SaveChangesAsync(cancellationToken);
        }
    }
}
