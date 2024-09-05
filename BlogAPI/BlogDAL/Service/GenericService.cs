using BlogDAL.IService;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BlogDAL.Service
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        private AppDbContext context;
        protected DbSet<T> dbSet;
        public GenericService(AppDbContext _context)
        {
            context = _context;
            dbSet = context.Set<T>();
        }
        public async Task<T> Add(T entity)
        {
            await dbSet.AddAsync(entity);
            return entity;
        }

        public async Task Delete(Expression<Func<T, bool>> predicate)
        {
            var entity = await GetById(predicate);
            dbSet.Remove(entity);
        }

        public async Task<ICollection<T>> GetAll()
        {
            return await dbSet.ToListAsync();
        }
        public async Task<ICollection<T>> GetAll(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetById(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<TResult> GetById<TResult>(Expression<Func<T, bool>> predicate,
                                                    Expression<Func<T, TResult>> selector)
        {
            return await dbSet.Where(predicate).Select(selector).FirstOrDefaultAsync();
        }


        public async Task<ICollection<T>> GetByPredicate(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.Where(predicate).ToListAsync();
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }

        public async Task<ICollection<TResult>> GetAllWithSelect<TResult>(Expression<Func<T, TResult>> selector,
                                                                          params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.Select(selector).ToListAsync();
        }

    }
}
