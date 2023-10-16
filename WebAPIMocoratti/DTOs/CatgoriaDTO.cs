using WebAPIMocoratti.Models;

namespace WebAPIMocoratti.DTOs
{
    public class CategoriaDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public virtual ICollection<Produto> Produtos { get; set; }
    }
}
