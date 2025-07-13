using RESTfullAPI.Data;
using RESTfullAPI.Entities;
using Microsoft.EntityFrameworkCore;
namespace RESTfullAPI.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProdutoRepository(ApplicationDbContext applicationDbContext)
    {
        _dbContext = applicationDbContext;
    }

    public async Task<Produtos> ObterDetalheProdutoAsync(int id)
    {
        var detalheProduto = _dbContext.Produtos
            .Where(x => x.Imagem!.Length > 0)
            .Include("modoentrega")
            .Include("categoria")
            .FirstOrDefaultAsync(p => p.Id == id);

        if(detalheProduto is null)
            throw new InvalidOperationException("Produto não encontrado");

        return await detalheProduto;
    }

    public async Task<List<Produtos>> ObterProdutosMaisVendidoAsync()
    {
        return await _dbContext.Produtos
            .Where(p => p.MaisVendido)
            .Where(x => x.Imagem!.Length > 0)
            .Include("modoentrega")
            .Include("categoria")
            .OrderBy(p => p.categoria.Ordem)
            .ThenBy(p => p.Nome)
            .ToListAsync();
    }

    public async Task<List<Produtos>> ObterProdutosPorCategoriaAsync(int categoria)
    {
        return await _dbContext.Produtos
            .Where(p => p.CategoriaId == categoria)
            .Where(x => x.Imagem.Length > 0)
            .Include("modoentrega") //se é ao kg, litro, etc
            .Include("categoria")
            .OrderBy(o => o.Nome)
            .ToListAsync();
    }

    public async Task<List<Produtos>> ObterProdutosPromocaoAsync()
    {
        return await _dbContext.Produtos
            .Where(p => p.Promocao == true)
            .Where(x => x.Imagem!.Length > 0)
            .Include("modoentrega")
            .Include("categoria")
            .OrderBy(p => p.categoria.Ordem)
            .ThenBy(o => o.Nome)
            .ToListAsync();
    }

    public async Task<List<Produtos>> ObterTodosProdutosAsync()
    {
        return await _dbContext.Produtos
            .Where(x => x.Imagem!.Length > 0)
            .Include(x => x.modoentrega)
            .Include(x => x.categoria)
            .OrderBy(p => p.categoria.Ordem)
            .ThenBy(o => o.Nome)
            .ToListAsync();
    }
}
