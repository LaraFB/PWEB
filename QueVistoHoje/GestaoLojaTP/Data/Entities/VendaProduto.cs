namespace GestaoLojaTP.Data.Entities
{
    public class VendaProduto{

        public int Id { get; set; }

        public int EncomendaId { get; set; }

        public int ProdutoId { get; set; }

        public int Quantidade { get; set; }

    }
}
