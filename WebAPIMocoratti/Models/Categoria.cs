using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using WebAPIMocoratti.Validations;

namespace WebAPIMocoratti.Models
{
    public class Categoria : IValidatableObject
    {
        public Categoria()
        {
            Produtos = new Collection<Produto>();
        }
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [LetraMaiusculaAtributte]
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public virtual ICollection<Produto> Produtos { get; set; }

      

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(this.Descricao))
            {
                bool terminaComPonto = this.Descricao.ToString().EndsWith(".");
                if (!terminaComPonto)
                {
                    yield return new ValidationResult("A Descricão deve terminar com '.' ", new[] { nameof(this.Descricao) });
                }
            }
        }
    }
}
