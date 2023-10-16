namespace WebAPIMocoratti.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        IProdutoRepository ProdutoRepository { get; }
        ICategoriaRepository CategoriaRepository { get; }
        Task Comit();
    }
}
