using Tornado.Domain.Models.VideoModels;
using Tornado.Infrastructure.Interfaces.Repositories;

namespace Tornado.Infrastructure.Data.Repositories
{
    public class VideoRepository : IVideoRepository
    {
        private readonly ApplicationDatabaseContext _databaseContext;
        public VideoRepository(ApplicationDatabaseContext applicationDatabaseContext)
        {
            _databaseContext = applicationDatabaseContext;
        }

        public async Task AddAsync(Video entity, CancellationToken cancellationToken)
        {
            await _databaseContext.Videos.AddAsync(entity, cancellationToken);
        }

        public Task DeleteAsync(Video entity, CancellationToken cancellationToken)
        {
            return Task.FromResult(_databaseContext.Videos.Remove(entity));
        }

        public Task<IEnumerable<Video>> GetAllAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_databaseContext.Videos.AsEnumerable());
        }

        public Task<Video?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(_databaseContext.Videos.FirstOrDefault(video => video.Id == id));
        }

        public Task RemoveRangeAsync(IEnumerable<Video> entities, CancellationToken cancellationToken)
        {
            _databaseContext.Videos.RemoveRange(entities);
            return Task.FromResult(true); ;
        }

        public async Task UpdateAsync(Video entity, CancellationToken cancellationToken)
        {
            var itemFound = _databaseContext.Videos
               .Where(profile => profile.Id == entity.Id)
               .FirstOrDefault();

            if (itemFound == null)
            {
                throw new Exception($"No profile entity with id of {entity.Id} was found");
            }

            // only these four should be able to change
            itemFound.Title = entity.Title;
            itemFound.Description = entity.Description;
            itemFound.PreviewSourceUrl = entity.PreviewSourceUrl;

            await _databaseContext.SaveChangesAsync(cancellationToken);
        }
    }
}
