using WebAPIMocoratti.Models;
using WebAPIMocoratti.Pagination;

namespace WebAPIMocoratti.Repository.Interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<PagedList<Categoria>> GetAllCategorias(CategoriasParameters categoriasParameters);
        Task<IEnumerable<Categoria>> GetProdutoByCategoria();
    }
}
