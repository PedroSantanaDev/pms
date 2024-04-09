
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

        public async Task AddRangeAsync(IEnumerable<Entity> entities)
        {
            await _dbContext.Set<Entity>().AddRangeAsync(entities);
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

        public async Task<List<Entity>> GetAllAsync(
            Expression<Func<Entity, bool>> filter = null,
            Func<IQueryable<Entity>, IOrderedQueryable<Entity>> orderBy = null,
            string includeProperties = "",
            int page = 1,
            int pageSize = 25)
        {
            IQueryable<Entity> query = _dbContext.Set<Entity>();

            // Include related entities (including nested includes)
            if (!string.IsNullOrEmpty(includeProperties))
            {
                var includePropertiesArray = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var includeProperty in includePropertiesArray)
                {
                    if (includeProperty.Contains("."))
                    {
                        // This is a nested include
                        query = query.Include(includeProperty);
                    }
                    else
                    {
                        // This is a direct include
                        query = query.Include(includeProperty);
                    }
                }
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
            // Some defensive coding checking the page, should not happen if front-end properly validates
            if (page < 1)
            {
                page = 1;
            }
            if (pageSize < 1)
            {
                pageSize = 25;
            }
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
