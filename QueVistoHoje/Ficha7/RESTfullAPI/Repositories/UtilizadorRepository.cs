using Microsoft.AspNetCore.Mvc;
using RESTfullAPI.Data;
using RESTfullAPI.Entities;

namespace RESTfullAPI.Repositories;

public class UtilizadorRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UtilizadorRepository(ApplicationDbContext applicationDbContext)
    {
        _dbContext = applicationDbContext;
    }

    public Task<IActionResult> RegistarUser([FromBody] Utilizador utilizador)
    {
        throw new NotImplementedException();
    }
}
