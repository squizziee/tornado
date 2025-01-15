using Tornado.Domain.Models.ChannelModels;
using Tornado.Infrastructure.Interfaces.Repositories;

namespace Tornado.Infrastructure.Data.Repositories
{
    public class ChannelRepository : IChannelRepository
    {
        private readonly ApplicationDatabaseContext _databaseContext;
        public ChannelRepository(ApplicationDatabaseContext applicationDatabaseContext)
        {
            _databaseContext = applicationDatabaseContext;
        }

        public async Task AddAsync(Channel entity, CancellationToken cancellationToken) 
        {
            await _databaseContext.Channels.AddAsync(entity, cancellationToken);
        }

        public Task DeleteAsync(Channel entity, CancellationToken cancellationToken)
        {
            return Task.FromResult(_databaseContext.Channels.Remove(entity));
        }

        public Task<IEnumerable<Channel>> GetAllAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_databaseContext.Channels.AsEnumerable());
        }

        public Task<Channel?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(_databaseContext.Channels.FirstOrDefault(channel => channel.Id == id));
        }

        public Task RemoveRangeAsync(IEnumerable<Channel> entities, CancellationToken cancellationToken)
        {
            _databaseContext.Channels.RemoveRange(entities);
            return Task.CompletedTask;
        }

        public async Task UpdateAsync(Channel entity, CancellationToken cancellationToken)
        {
            var itemFound = _databaseContext.Channels
               .Where(profile => profile.Id == entity.Id)
               .FirstOrDefault();

            if (itemFound == null)
            {
                throw new Exception($"No channel entity with id of {entity.Id} was found");
            }

            // only these four should be able to change directly
            itemFound.Name = entity.Name;
            itemFound.Description = entity.Description;
            itemFound.ChannelHeaderSourceUrl = entity.ChannelHeaderSourceUrl;
            itemFound.ChannelAvatarSourceUrl = entity.ChannelAvatarSourceUrl;

            await _databaseContext.SaveChangesAsync(cancellationToken);
        }
    }
}
