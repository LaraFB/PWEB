using System.ComponentModel.DataAnnotations;

namespace GestaoLojaTP.Data.Entities
{
    public class ModoVenda
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }
    }
}
