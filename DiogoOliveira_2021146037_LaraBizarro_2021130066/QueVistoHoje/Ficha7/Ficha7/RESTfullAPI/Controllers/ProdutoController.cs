using Microsoft.AspNetCore.Mvc;
using RESTfullAPI.Entities;
using RESTfullAPI.Repositories;
namespace RESTfullAPI.Controllers;

[Route("api/[controller]")]
[ApiController]

//[Authorize]
public class ProdutoController : ControllerBase
{
    private readonly IProdutoRepository _produtoRepository;

    public ProdutoController(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetProdutos(string tipoProduto, int? categoriaId = null)
    {
        List<Produtos> produtos;
        if(tipoProduto == "categoria" &&  categoriaId != null)
        {
            produtos = await _produtoRepository.ObterProdutosPorCategoriaAsync(categoriaId.Value);
        }
        else if (tipoProduto == "detalhe" && categoriaId != null)
        {
            Produtos produto = await _produtoRepository.ObterDetalheProdutoAsync(categoriaId.Value);
            return Ok(produto);
        }
        else if(tipoProduto == "promocao")
        {
            var promocoes = await _produtoRepository.ObterProdutosPromocaoAsync();
            return Ok(promocoes);
        }
        else if(tipoProduto == "maisvendido")
        {
            produtos = await _produtoRepository.ObterProdutosMaisVendidoAsync();
        }
        else if(tipoProduto == "todos")
        {
            produtos = await _produtoRepository.ObterTodosProdutosAsync();
        }
        else
        {
            return BadRequest("Tipo de produto inválido");
        }
        return Ok(produtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDetalheProduto(int id)
    {
        Produtos produto = await _produtoRepository.ObterDetalheProdutoAsync(id);
        if (produto is null)
        {
            return NotFound($"Produto com id={id} não encontrado");
        }
        return Ok(produto);
    }
}
