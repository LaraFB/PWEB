using RESTfullAPI.Entities;
namespace RESTfullAPI.Repositories;

public interface ICategoriaRepository
{
    Task<List<Categorias>> GetCategorias();
}
