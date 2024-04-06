using System.Linq.Expressions;

namespace pms.app.Repository
{
    public interface IRepository<Entity> where Entity : class
    {
        Task<List<Entity>> GetAllAsync(Expression<Func<Entity, bool>> filter = null,
                                 Func<IQueryable<Entity>, IOrderedQueryable<Entity>> orderBy = null,
                                 string includeProperties = "",
                                 int page = 1,
                                 int pageSize = 25);
        Task<Entity> GetByIdAsync(int id);
        Task AddAsync(Entity entity);
        Task AddRangeAsync(IEnumerable<Entity> entities);
        Task UpdateAsync(Entity entity);
        Task DeleteAsync(int id);

    }
}
