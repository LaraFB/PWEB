namespace RCLApi.DTO;
public class ProdutoFavorito
{
    public int Id { get; set; }
    public bool Efavorito { get; set; }
    public int ProdutoId { get; set; }
    public ProdutoDTO produto { get; set; }
    public string emailID { get; set; }


}
