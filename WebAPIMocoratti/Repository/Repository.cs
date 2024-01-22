using Microsoft.EntityFrameworkCore;
using WebAPIMocoratti.Context;
using WebAPIMocoratti.Repository.Interfaces;

namespace WebAPIMocoratti.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        public Repository(AppDbContext context)
        {
            _context = context;
        }
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);

        }

        public  IQueryable<T> Get()
        {
            return  _context.Set<T>().AsNoTracking();
        }

        public async Task<T> GetById(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AsNoTracking().SingleOrDefaultAsync(predicate);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.Set<T>().Update(entity);

        }
    }
}
