using Microsoft.EntityFrameworkCore;
using RESTfullAPI.Data;
using RESTfullAPI.Entities;

namespace RESTfullAPI.Repositories
{
    public class EncomendasRepository : IEncomendasRepository
    {
        private readonly ApplicationDbContext context;
        public EncomendasRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<GerirVenda>> GetEncomendas()
        {
            return await context.GerirVendas.ToListAsync();
        }
        public async Task<GerirVenda> ObterDetalheEncomendaAsync(int id)
        {
            return await context.GerirVendas.FirstOrDefaultAsync(p => p.Id == id);
        }


        public async Task AdicionarVendaAsync(GerirVenda novaEncomenda)
        {
            await context.GerirVendas.AddAsync(novaEncomenda);
            await context.SaveChangesAsync();
        }
    }
}