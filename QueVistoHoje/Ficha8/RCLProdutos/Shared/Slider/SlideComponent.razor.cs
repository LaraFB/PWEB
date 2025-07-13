using Microsoft.AspNetCore.Components;
using RCLApi.DTO;
using RCLApi.Services;
using RCLProdutos.Services.Interfaces;
using System.Net.NetworkInformation;
using System.Xml.Linq;
using Xamarin.Essentials;

namespace RCLProdutos.Shared.Slider;

public partial class SlideComponent
{

    [SupplyParameterFromQuery]
    public int prodSugereId { get; set; }

    [Parameter]
    public ProdutoDTO? produto { get; set; } = new ProdutoDTO();

    [Parameter]
    public float? width { get; set; }

    [Parameter]
    public string? marginLeft { get; set; }

    [Inject]
    public IApiServices? _apiServices { get; set; }
    public int countSlide { get; set; } = 0;

    public ProdutoFavorito? produtoFavorito { get; set; } = new ProdutoFavorito();
    private string? uidprod { get; set; }
    private string? favoritoicon { get; set; }
    private string? pathurlimg { get; set; }

    [Parameter] public EventCallback<GerirVenda> OnItemAdicionado { get; set; }
    private CarrinhoDTO carrinho { get; set; } = new CarrinhoDTO();
    public ProdutoDTO recebe = new ProdutoDTO();
    private string emailuser = null;
    protected override async Task OnInitializedAsync()
    {
        modalDisplay2 = "none";
        modalDisplay1 = "none";

        mostraInfo = "none";
        fazcompra = "none";
        if (!produto.Favorito)
            favoritoicon = $"images/heart.png";
        else
        {
            favoritoicon = $"images/heartfilltransp.png";
        }
        carrinho = await _apiServices.GetCarrinho() ?? new CarrinhoDTO();
        var username = await _apiServices.GetUtilizadorAtual();
        emailuser = username.Email;
    }

    private string mostraInfo;
    private string fazcompra;

    private string acao;
    private string acaocompra;
    private void Info()
    {
        acao = mostraInfo;

        if (acao == "none")
            mostraInfo = "block";
        else mostraInfo = "none";
    }


    //*************** MODAIS ************

    private string modalDisplay1 = "none;";
    private string modalDisplay2 = "none;";

    private string modalClass = string.Empty;

    private int quantidade = 0;
    private decimal total = 0;
    public string limiteQtd = "";

    private bool abreModal1 = false;
    private bool abreModal2 = false;

    public async void AbreFecha(string janela1, string janela2)
    {
        if (janela1 == "abre")
        {
            modalDisplay1 = "block";
            abreModal1 = true;
        }
        else if (janela1 == "fecha")
        {
            modalDisplay1 = "none";
            abreModal1 = false;
            limiteQtd = "";
        }
        else if (janela1 == "grava")
        {
            modalDisplay2 = "none";
            abreModal2 = false;
            if (janela1 == "grava")
            {
                var vendaProduto = new ItemCarrinho
                {
                    ProdutoId = produto.Id,
                    quantidade = (int)quantidade,
                    precoUnidade = produto.Preco,
                    nome = produto.Nome
                };

                carrinho.ItensCarrinho.Add(vendaProduto);
                await _apiServices.SetCarrinho(carrinho);
                NavigationManager.NavigateTo("/carrinho");
            }

            AbreFecha("fecha", null);
        }

        if (janela2 == "abre")
        {
            modalDisplay2 = "block";
            abreModal2 = true;
        }
        else if (janela2 == "fecha")
        {
            modalDisplay2 = "none";
            abreModal2 = false;
            quantidade = 0;
        }

    }

    public async void Favoritos(string acao, int pId)
    {
        if (favoritoicon == $"images/heart.png")
        {
            favoritoicon = $"images/heartfilltransp.png";
            var produtoFavorito = new ProdutoFavorito
            {
                ProdutoId = pId,
                emailID = emailuser,
                Efavorito = true
            };

            var favoritoDto = new FavoritosDTO
            {
                UserId = produtoFavorito.emailID,
                ProdutoId = produtoFavorito.ProdutoId
            };

            var novoFavorito = await _apiServices.AdicionaFavorito(favoritoDto);

        }
        else
        {
            favoritoicon = $"images/heart.png";
            
        }
    }
    public void Incrementa(string incredec, string janela2)
    {
        if (incredec == "incrementa")
        {
            if (quantidade >= produto.EmStock)
            {
                limiteQtd = "Lamentamos mas não existe mais stock!";
                return;
            }
            quantidade++;
        }

        else if (incredec == "desincrementa" && quantidade > 0)
        {
            quantidade--;
            limiteQtd = "";
        }

        total = quantidade * produto.Preco;
    }
}

