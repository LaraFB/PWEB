using Microsoft.EntityFrameworkCore;
using RESTfullAPI.Data;
using RESTfullAPI.Data.DTO;
using RESTfullAPI.Entities;

namespace RESTfullAPI.Repositories
{
    public class FavoritosRepository : IFavoritosRepository
    {
        private readonly ApplicationDbContext _context;

        public FavoritosRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Favoritos>> GetFavoritos(string userId)
        {
            return await _context.Favoritos
                .Where(f => f.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<FavoritosDTO>> GetFavoritosWithProducts(string userId)
        {
            return await _context.Favoritos
               .Where(f => f.UserId == userId)
                .Join(
                   _context.Produtos,
                   favorito => favorito.ProdutoId,
                   produto => produto.Id,
                   (favorito, produto) => new FavoritosDTO
                   {
                       Id = favorito.Id,
                       ProdutoId = produto.Id,
                       NomeProduto = produto.Nome,
                       PrecoProduto = produto.Preco,
                       UrlImagem = produto.UrlImagem
                   }
               )
               .ToListAsync();
        }

        public async Task AdicionarFavoritoAsync(Favoritos novoFavorito)
        {
            await _context.Favoritos.AddAsync(novoFavorito);
            await _context.SaveChangesAsync();
        }

        public async Task<Favoritos> GetFavoritos(int id)
        {
            return await _context.Favoritos.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task RemoverFavoritoAsync(int id)
        {
            var favorito = await _context.Favoritos.FirstOrDefaultAsync(f => f.Id == id);
            if (favorito is not null)
            {
                _context.Favoritos.Remove(favorito);
                await _context.SaveChangesAsync();
            }
        }
    }
}
