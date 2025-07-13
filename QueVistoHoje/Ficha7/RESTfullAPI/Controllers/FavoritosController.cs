using Microsoft.AspNetCore.Mvc;
using RESTfullAPI.Entities;
using RESTfullAPI.Repositories;

namespace RESTfullAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritosController : ControllerBase
    {
        private readonly IFavoritosRepository _favoritosRepository;

        public FavoritosController(IFavoritosRepository favoritosRepository)
        {
            _favoritosRepository = favoritosRepository;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetFavoritos(string userId)
        {
            var favoritos = await _favoritosRepository.GetFavoritosWithProducts(userId);
            if (favoritos is null)
            {
                return NotFound($"Favoritos do utilizador com id={userId} não encontrados");
            }
            return Ok(favoritos);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddFavoritos([FromBody] Favoritos novoFavorito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _favoritosRepository.AdicionarFavoritoAsync(novoFavorito);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteFavorito(int id)
        {
            var fav = await _favoritosRepository.GetFavoritos(id);

            if (fav is null)
            {
                return NotFound($"Favorito com id={id} não encontrado");
            }

            await _favoritosRepository.RemoverFavoritoAsync(id);
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
