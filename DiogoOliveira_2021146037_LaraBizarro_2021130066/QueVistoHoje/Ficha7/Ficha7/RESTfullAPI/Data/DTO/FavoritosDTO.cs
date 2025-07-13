namespace RESTfullAPI.Data.DTO
{
    public class FavoritosDTO
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public string NomeProduto { get; set; }
        public decimal PrecoProduto { get; set; }
        public string? UrlImagem { get; set; }

    }
}
