using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoLojaTP.Data.Entities
{
    public class GerirVenda
    {
        public int Id { get; set; }

        public string? UserId { get; set; }

        public ICollection<VendaProduto> Itens = new List<VendaProduto>();

        public string status { get; set; }

        public DateTime DataEncomenda { get; set; } = DateTime.Now;

        public string FormaPagamento { get; set; }

        public string Endereco { get; set; }

        [Phone]
        public string Telefone { get; set; }

        public decimal TotaldaEncomenda { get; set; }

        /*[NotMapped]
         public decimal Total => Itens.Sum(item => item.SubTotal*/
    }
}
