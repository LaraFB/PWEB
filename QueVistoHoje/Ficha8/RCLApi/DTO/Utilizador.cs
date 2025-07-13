using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RCLApi.DTO;

// Add profile data for application users by adding properties to the ApplicationUser class
public class Utilizador
{
    //public string? Nome { get; set; }
    //public string? Apelido { get; set; }

    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "Endereco de Email Invalido")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "A password é obrigatória")]
    public string? Password { get; set; }


    //[Required(ErrorMessage = "A confirmação da Password é obrigatória!")]
    //[Compare("Password", ErrorMessage = "A Password e a Confirmação da Password não coincidem")]
    //public string ConfirmPassword { get; set; }


    //[ValidarNIF(ErrorMessage = "NIF inválido!")]
    //public long? NIF { get; set; }
    //public string? Rua { get; set; }
    //public string? Localidade1 { get; set; }
    //public string? Localidade2 { get; set; }
    //public string? Pais { get; set; }

    //[NotMapped]
    //public IFormFile? ImageFile { get; set; }
    //public byte[]? Fotografia { get; set; }
    //public string? UrlImagem { get; set; }

    //// Validação customizada para o NIF
    //public class ValidarNIF: ValidationAttribute
    //{
    //    public override bool IsValid(object value)
    //    {
    //        // Inserir o código que está no site das Finanças
    //        if (value is long nif)
    //        {
    //            return nif > 100;
    //        }
    //        return false;
    //    }
    //}
}
