using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLApi.DTO
{
    public class ItemCarrinho{
        public int quantidade {  get; set; }
        public decimal precoUnidade { get; set; }
        public string nome { get; set; }
        public int ProdutoId { get; set; }
    }
}
