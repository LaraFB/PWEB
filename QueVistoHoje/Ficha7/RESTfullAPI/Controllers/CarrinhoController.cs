using Microsoft.AspNetCore.Mvc;
using RESTfullAPI.Entities;
using RESTfullAPI.Repositories;
using System.Threading.Tasks;

namespace RESTfullAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarrinhoController : ControllerBase
    {
        private readonly ICarrinhoRepository _carrinhoRepository;

        public CarrinhoController(ICarrinhoRepository carrinhoRepository)
        {
            _carrinhoRepository = carrinhoRepository;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCarrinho(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Utilizador não autenticado.");

            var carrinho = await _carrinhoRepository.GetCarrinho(userId);

            if (carrinho == null)
                return NotFound("Carrinho não encontrado.");

            return Ok(carrinho);
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> AdicionarCarrinho(string userId, [FromBody] Carrinho novoItem)
        {
            if (novoItem == null)
                return BadRequest("Carrinho inválido.");

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Utilizador não autenticado.");

            await _carrinhoRepository.AdicionarCarrinho(userId, novoItem);

            var carrinho = await _carrinhoRepository.GetCarrinho(userId);

            return Ok(new { mensagem = "Carrinho atualizado com sucesso.", item = carrinho });
        }
    }
}