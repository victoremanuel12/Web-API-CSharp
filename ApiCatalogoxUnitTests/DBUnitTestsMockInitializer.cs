using WebAPIMocoratti.Context;
using WebAPIMocoratti.Models;

namespace ApiCatalogoxUnitTests
{
    public class DBUnitTestsMockInitializer
    {
        public DBUnitTestsMockInitializer()
        {

        }
        public void Seed(AppDbContext context)
        {
            context.Categorias.Add(new Categoria { Id = 111, Descricao = "teste de criação e povoamento do banco", Nome = "categoria Teste" });
        }
    }
}
