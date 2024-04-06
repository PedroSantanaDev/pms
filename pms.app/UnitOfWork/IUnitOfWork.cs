using pms.app.Repository;

namespace pms.app.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Entity> GetRepository<Entity>() where Entity : class;
        Task<int> SaveChangesAsync();
    }
}
