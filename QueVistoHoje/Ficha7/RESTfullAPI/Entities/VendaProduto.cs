namespace RESTfullAPI.Entities
{
    public class VendaProduto
    {

        public int Id { get; set; }

        public int EncomendaId { get; set; }

        public GerirVenda Encomenda { get; set; }

        public int ProdutoId { get; set; }

        public Produtos Produto { get; set; }

        public int Quantidade { get; set; }

        public decimal Subtotal => Produto.Preco * Quantidade;

    }
}
