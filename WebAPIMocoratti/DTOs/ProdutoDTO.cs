namespace WebAPIMocoratti.DTOs
{
    public class ProdutoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public string ImgUrl { get; set; }
        public int CategoriaId { get; set; }
    }
}
