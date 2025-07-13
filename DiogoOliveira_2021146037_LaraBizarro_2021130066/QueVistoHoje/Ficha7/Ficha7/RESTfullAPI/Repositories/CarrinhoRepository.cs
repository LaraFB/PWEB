using RESTfullAPI.Entities;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace RESTfullAPI.Repositories
{
    public class CarrinhoRepository : ICarrinhoRepository
    {
        private static readonly ConcurrentDictionary<string, Carrinho> _carrinhos = new ConcurrentDictionary<string, Carrinho>();

        public async Task<Carrinho> GetCarrinho(string userId)
        {
            if (_carrinhos.TryGetValue(userId, out var carrinho))
            {
                return await Task.FromResult(carrinho);
            }
            return await Task.FromResult(new Carrinho());
        }

        public async Task AdicionarCarrinho(string userId, Carrinho novoCarrinho)
        {
            _carrinhos.AddOrUpdate(userId, novoCarrinho, (key, oldValue) => novoCarrinho);
            await Task.CompletedTask;
        }
    }
}