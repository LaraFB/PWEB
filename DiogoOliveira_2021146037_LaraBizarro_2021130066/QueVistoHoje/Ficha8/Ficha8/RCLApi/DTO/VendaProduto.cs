using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLApi.DTO
{
    public class VendaProduto
    {

        public int Id { get; set; }

        public int EncomendaId { get; set; }

        public GerirVenda Encomenda { get; set; }

        public int ProdutoId { get; set; }

        public ProdutoDTO Produto { get; set; }

        public int Quantidade { get; set; }

        public decimal Subtotal => Produto.Preco * Quantidade;

    }
}
