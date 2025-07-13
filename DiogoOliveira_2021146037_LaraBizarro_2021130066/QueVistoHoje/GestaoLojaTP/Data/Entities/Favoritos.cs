using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GestaoLojaTP.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace GestaoLojaTP.Data.Entities
{
    public class Favoritos
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int ProdutoId { get; set; } 
    }
}
