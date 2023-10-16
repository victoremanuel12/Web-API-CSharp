using WebAPIMocoratti.Models;
using WebAPIMocoratti.Pagination;

namespace WebAPIMocoratti.Repository.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<PagedList<Produto>> GetAllProducts( ProdutosParameters produtosParameters);
        Task<IEnumerable<Produto>> GetProdutoByPreco();
    }
}
