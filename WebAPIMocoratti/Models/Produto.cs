using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebAPIMocoratti.Models
{
	public class Produto
	{
		public int Id { get; set; }

		[Required]
		[MaxLength(50)]
		public string Nome { get; set; }

		[Required]
		[MaxLength(300,ErrorMessage ="O maximo de caracteres é 300")]
		public string Descricao { get; set; }


		[Required]
		[Column(TypeName = "decimal(10,2)")]
		public decimal Preco { get; set; }
		public string ImgUrl { get; set; }

		[Required]
		public int Estoque { get; set; }
		public DateTime DataCadastro { get; set; }
		public int CategoriaId { get; set; }
        [JsonIgnore]
        public virtual  Categoria Categoria { get; set; }



	}
}
