using Microsoft.EntityFrameworkCore;
using WebAPIMocoratti.Context;
using WebAPIMocoratti.Models;
using WebAPIMocoratti.Pagination;
using WebAPIMocoratti.Repository.Interfaces;

namespace WebAPIMocoratti.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<PagedList<Categoria>> GetAllCategorias( CategoriasParameters categoriasParameters)
        {
            return await  PagedList<Categoria>.ToPagedList(
                    Get().OrderBy(ca => ca.Id), 
                    categoriasParameters.PageNumber, 
                    categoriasParameters.PageSize
            );
        }

        public async Task<IEnumerable<Categoria>> GetProdutoByCategoria()
        {
            return await Get().Include(c => c.Produtos).ToListAsync();
        }
    }
}
