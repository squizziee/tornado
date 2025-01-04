namespace Tornado.Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void CreateTransaction();
        void Commit();
        void Rollback();
        void Save();
    }
}
