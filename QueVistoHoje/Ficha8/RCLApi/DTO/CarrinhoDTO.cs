using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLApi.DTO
{
    public class CarrinhoDTO{
        public List<ItemCarrinho> ItensCarrinho { get; set; } = new List<ItemCarrinho>();
    }
}
