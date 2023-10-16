using Microsoft.EntityFrameworkCore;
using WebAPIMocoratti.Context;
using WebAPIMocoratti.Models;
using WebAPIMocoratti.Pagination;
using WebAPIMocoratti.Repository.Interfaces;

namespace WebAPIMocoratti.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<PagedList<Produto>> GetAllProducts(ProdutosParameters produtosParameters)
        {
            return await  PagedList<Produto>.ToPagedList(
                Get().OrderBy(on => on.Id),
                produtosParameters.PageNumber,
                produtosParameters.PageSize
            );
       
        }

        public async Task<IEnumerable<Produto>> GetProdutoByPreco()
        {
            return await Get().OrderBy(p => p.Preco).ToListAsync();
        }
    }
}
