using System.Collections.Generic;
using System.Threading.Tasks;
using RESTfullAPI.Data.DTO;
using RESTfullAPI.Entities;

namespace RESTfullAPI.Repositories
{
    public interface IFavoritosRepository
    {
        Task<List<Favoritos>> GetFavoritos(string userId);
        Task<List<FavoritosDTO>> GetFavoritosWithProducts(string userId);
        Task AdicionarFavoritoAsync(Favoritos novoFavorito);
        Task<Favoritos> GetFavoritos(int id);
        Task RemoverFavoritoAsync(int id);
    }
}
