using RESTfullAPI.Entities;

namespace RESTfullAPI.Repositories
{
    public interface ICarrinhoRepository
    {
        Task<Carrinho> GetCarrinho(string userId);
        Task AdicionarCarrinho(string userId, Carrinho novoitem);
    }
}
