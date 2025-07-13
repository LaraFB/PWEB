using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLApi.DTO
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Endereço de Email Inválido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "A indicação da Password é obrigatória!")]
        public string Password { get; set; }
        //public string? UrlImagem { get; set; }

        //public string? UserName { get; set; }
    }
}
