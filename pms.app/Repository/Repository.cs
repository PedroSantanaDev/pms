
using Microsoft.EntityFrameworkCore;
using pms.app.Data;
using System.Linq.Expressions;

namespace pms.app.Repository
{
    public class Repository<Entity> : IRepository<Entity> where Entity : class
    {
        private readonly ApplicationDbContext _dbContext;
        public Repository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task AddAsync(Entity entity)
        {
            await _dbContext.Set<Entity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbContext.Set<Entity>().Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Entity>> GetAllAsync(Expression<Func<Entity, bool>> filter = null,
                                             Func<IQueryable<Entity>, IOrderedQueryable<Entity>> orderBy = null,
                                             string includeProperties = "",
                                             int page = 1,
                                             int pageSize = 25)
        {
            IQueryable<Entity> query = _dbContext.Set<Entity>();

            // Include related entities
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            // Apply filter if provided
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Apply orderBy if provided
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            // Apply pagination
            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<Entity> GetByIdAsync(int id) => await _dbContext.Set<Entity>().FindAsync(id);

        public async Task UpdateAsync(Entity entity)
        {
            _dbContext.Set<Entity>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
