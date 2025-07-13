using RCLApi.DTO;
using RCLAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLApi.Services;

public interface IApiServices
{
    public Task<List<ProdutoDTO>> GetProdutosEspecificos(string produtoTipo, int? IdCategoria);
    public Task<(ProdutoDTO? Data, string? ErrorMessage)> GetDetalheProduto(int id);
    public Task<(T? Data, string? ErrorMessage)> GetAsync<T>(string endpoint);
    public Task<List<Categoria>> GetCategorias();
    public Task<(bool Data, string? ErrorMessage)> ActualizaFavorito(string acao, int produtoId);
    public Task<List<ProdutoFavorito>> GetFavoritos(string utilizadorId);
    public Task<ApiResponse<bool>> AdicionaFavorito(FavoritosDTO novoFavorito);
    public Task<ApiResponse<bool>> RegistarUtilizador(Utilizador novoUtilizador);
    public Task<ApiResponse<bool>> Login(LoginModel login);
    public Task<ApiResponse<bool>> AdicionaItemNoCarrinho(GerirVenda carrinhoCompra);
    public Task<CarrinhoDTO> GetCarrinho();
    public Task<ApiResponse<bool>> SetCarrinho(CarrinhoDTO carrinho);
    public Task<Utilizador?> GetUtilizadorAtual();
}
