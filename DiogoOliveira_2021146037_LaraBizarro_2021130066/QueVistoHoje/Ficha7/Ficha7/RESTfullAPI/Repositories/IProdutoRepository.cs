using RESTfullAPI.Entities;
namespace RESTfullAPI.Repositories;

public interface IProdutoRepository
{
    Task<List<Produtos>> ObterProdutosPorCategoriaAsync (int categoria);
    Task<List<Produtos>> ObterProdutosPromocaoAsync();
    Task<List<Produtos>> ObterProdutosMaisVendidoAsync();
    Task<List<Produtos>> ObterTodosProdutosAsync();
    Task<Produtos> ObterDetalheProdutoAsync(int id);
}
