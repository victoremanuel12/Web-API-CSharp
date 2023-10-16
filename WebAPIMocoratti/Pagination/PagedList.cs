using Microsoft.EntityFrameworkCore;

namespace WebAPIMocoratti.Pagination
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevius => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public PagedList( List<T> itemsPaginados, int count, int pageNumber,int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(itemsPaginados);
        }
        public async static Task<PagedList<T>> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            //chama o construtor
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
    
}
