using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RESTfullAPI.Data;
using RESTfullAPI.Data.DTO;
using RESTfullAPI.Entities;
using RESTfullAPI.Repositories;
namespace RESTfullAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EncomendasController : ControllerBase
{
    private readonly IEncomendasRepository _encomendasRepository;
    private readonly ApplicationDbContext _context;

    public EncomendasController(IEncomendasRepository encomendasRepository, ApplicationDbContext context)
    {
        _encomendasRepository = encomendasRepository;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetEncomendas()
    {
        var encomendas = await _encomendasRepository.GetEncomendas();
        return Ok(encomendas);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDetalheEncomenda(int id)
    {
        GerirVenda encomenda = await _encomendasRepository.ObterDetalheEncomendaAsync(id);
        if (encomenda is null)
        {
            return NotFound($"Encomenda com id={id} não encontrada");
        }
        return Ok(encomenda);
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddEncomenda([FromBody] NovaEncomenda novaEncomendaDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var novaEncomenda = new GerirVenda
        {
            UserId = novaEncomendaDto.UserId,
            status = novaEncomendaDto.Status,
            FormaPagamento = novaEncomendaDto.FormaPagamento,
            Endereco = novaEncomendaDto.Endereco,
            Telefone = novaEncomendaDto.Telefone,
            DataEncomenda = DateTime.Now,
        };

        decimal totalEncomenda = 0;
        foreach (var itemDto in novaEncomendaDto.Itens)
        {
            var prod = await _context.Produtos.FindAsync(itemDto.ProdutoId);

            if (prod == null)
            {
                return BadRequest($"Produto com id={itemDto.ProdutoId} não encontrado");
            }

            var vendaProduto = new VendaProduto
            {
                ProdutoId = itemDto.ProdutoId,
                Quantidade = itemDto.Quantidade,
                Produto = prod,
                Encomenda = novaEncomenda
            };

            novaEncomenda.Itens.Add(vendaProduto);
            totalEncomenda += vendaProduto.Subtotal;

        }

        novaEncomenda.TotaldaEncomenda = totalEncomenda;

        await _encomendasRepository.AdicionarVendaAsync(novaEncomenda);

        return StatusCode(StatusCodes.Status201Created);
    }
}

