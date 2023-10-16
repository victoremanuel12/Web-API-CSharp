using System.Linq.Expressions;

namespace WebAPIMocoratti.Repository.Interfaces
{
    public interface IRepository<T>
    {
        //IQueryble permite chamadas asyncronas, não precisando tipalas como Task
        IQueryable<T> Get();
        Task<T> GetById(Expression<Func<T,bool>> predicate);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);

    }
}
