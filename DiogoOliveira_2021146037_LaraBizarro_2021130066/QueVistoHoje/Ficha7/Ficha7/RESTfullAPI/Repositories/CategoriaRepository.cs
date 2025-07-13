using Microsoft.EntityFrameworkCore;
using RESTfullAPI.Data;
using RESTfullAPI.Entities;
namespace RESTfullAPI.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly ApplicationDbContext context;

    public CategoriaRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<List<Categorias>> GetCategorias()
    {
        return await context.Categorias.ToListAsync();
    }
}
