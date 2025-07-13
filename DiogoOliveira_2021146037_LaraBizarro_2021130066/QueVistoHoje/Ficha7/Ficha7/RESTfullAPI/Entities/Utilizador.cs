using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RESTfullAPI.Entities;
// Add profile data for application users by adding properties to the ApplicationUser class
public class Utilizador
{ /*
    public string? Nome { get; set; }
    public string? Apelido { get; set; }
    public string? EMail { get; set; }
    public string? Password { get; set; }
    public long? NIF { get; set; }
    public string? Rua { get; set; }
    public string? Localidade1 { get; set; }
    public string? Localidade2 { get; set; }
    public string? Pais { get; set; }

    [NotMapped]
    public IFormFile? ImageFile { get; set; }
    public byte[]? Fotografia { get; set; }
    public string? UrlImagem { get; set; }*/

    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "Endereco de Email Invalido")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "A password é obrigatória")]
    public string? Password { get; set; }




    public class ValidarNif : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is long nif)
            {
                // Exemplo de validação personalizada para NIF
                return nif > 100;
            }
            return false;
        }
    }
}
