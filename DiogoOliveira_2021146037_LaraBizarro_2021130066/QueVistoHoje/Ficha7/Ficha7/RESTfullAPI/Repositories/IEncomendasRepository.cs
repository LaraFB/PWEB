using RESTfullAPI.Entities;

namespace RESTfullAPI.Repositories
{
    public interface IEncomendasRepository
    {
        Task<List<GerirVenda>> GetEncomendas();
        Task<GerirVenda> ObterDetalheEncomendaAsync(int id);
        Task AdicionarVendaAsync(GerirVenda novaEncomenda);
    }
}
