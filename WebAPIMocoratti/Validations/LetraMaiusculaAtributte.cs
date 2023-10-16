using System.ComponentModel.DataAnnotations;

namespace WebAPIMocoratti.Validations
{
    public class LetraMaiusculaAtributte : ValidationAttribute
    {
        protected override ValidationResult IsValid(Object PropertyValue, ValidationContext validationContext)
        {
            if(PropertyValue == null || String.IsNullOrEmpty(PropertyValue.ToString())) 
            {
                return ValidationResult.Success;
            }
            var primeiraLetra = PropertyValue.ToString()[0].ToString();
            if(primeiraLetra != primeiraLetra.ToUpper())
            {
                return new ValidationResult("A primeira letra do nome deve ser maiscula");
            }
            return ValidationResult.Success;
        }
    }
}
