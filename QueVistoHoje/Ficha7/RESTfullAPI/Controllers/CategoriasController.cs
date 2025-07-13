using Microsoft.AspNetCore.Mvc;
using RESTfullAPI.Repositories;
namespace RESTfullAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaRepository _categoriaRepository;

    public CategoriasController(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategorias()
    {
        var categorias = await _categoriaRepository.GetCategorias();
        return Ok(categorias);
    }
}
