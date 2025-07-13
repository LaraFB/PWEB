using System.ComponentModel.DataAnnotations;

namespace RESTfullAPI.Data.DTO
{
    public class NovaEncomenda
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string FormaPagamento { get; set; }

        [Required]
        public string Endereco { get; set; }

        [Required]
        [Phone]
        public string Telefone { get; set; }

        [Required]
        public List<ItemVendaDto> Itens { get; set; }

      
    }

    public class ItemVendaDto
    {
        [Required]
        public int ProdutoId { get; set; }

        [Required]
        public int Quantidade { get; set; }
    }
}
