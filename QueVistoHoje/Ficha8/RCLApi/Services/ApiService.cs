    using Microsoft.Extensions.Logging;
    using System.Net;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System;
    using System.Net.Http;
    using Microsoft.AspNetCore.Http;
    using static System.Runtime.InteropServices.JavaScript.JSType;
    using System.Data.Entity.Core.Objects;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using System.Net.NetworkInformation;
    using RCLApi.DTO;
    using RCLApi.Services;
    using RCLApi;
    using static SQLite.SQLite3;
    using Xamarin.Essentials;
    using System.Net.Http.Json;

    namespace RCLAPI.Services;
    public class ApiService : IApiServices
    {
        private readonly ILogger<ApiService> _logger;
        private readonly HttpClient _httpClient = new();

        private readonly IHttpContextAccessor _httpContextAccessor;

        JsonSerializerOptions _serializerOptions;

        private List<ProdutoDTO> produtos;

        private List<Categoria> categorias;

        private ProdutoDTO _detalhesProduto;
        private string _token;
        private string userID = null;
        public ApiService(ILogger<ApiService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            _logger = logger;
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _detalhesProduto = new ProdutoDTO();
            categorias = new List<Categoria>();
        }
        private void AddAuthorizationHeader()
        {
            if (!string.IsNullOrEmpty(_token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
            }
        }

        // ********************* Categorias  **********
        public async Task<List<Categoria>> GetCategorias()
        {
            string endpoint = $"api/Categorias";

            try
            {
                HttpResponseMessage httpResponseMessage =
                    await _httpClient.GetAsync($"{AppConfig.BaseUrl}{endpoint}");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string content = "";
                    content = await httpResponseMessage.Content.ReadAsStringAsync();
                    _logger.LogInformation("JSON recebido: {Content}", content);
                    categorias = JsonSerializer.Deserialize<List<Categoria>>(content, _serializerOptions) ?? new List<Categoria>();
                }
                else
                {
                    _logger.LogError("Erro ao buscar categorias: {ReasonPhrase}", httpResponseMessage.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

            return categorias;
        }

        // ********************* Produtos  **********
        public async Task<List<ProdutoDTO>> GetProdutosEspecificos(string produtoTipo, int? IdCategoria)
        {
            string endpoint = "";

            if (produtoTipo == "categoria" && IdCategoria != null)
            {
                endpoint = $"api/Produto?tipoProduto=categoria&categoriaId={IdCategoria}";
            }
            else if (produtoTipo == "todos")
            {
                endpoint = $"api/Produto?tipoProduto=todos";
            }
            else
            {
                return null;
            }
            try
            {
                HttpResponseMessage httpResponseMessage =
                    await _httpClient.GetAsync($"{AppConfig.BaseUrl}{endpoint}");
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string content = "";
                    content = await httpResponseMessage.Content.ReadAsStringAsync();
                    produtos = JsonSerializer.Deserialize<List<ProdutoDTO>>(content, _serializerOptions)!;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            return produtos;
        }
        public async Task<(ProdutoDTO? Data, string? ErrorMessage)> GetDetalheProduto(int id)
        {
            try
            {
                string endpoint = $"api/Produto/{id}";
                _logger.LogDebug($"Endpoint: {AppConfig.BaseUrl}{endpoint}");
                HttpResponseMessage response = await _httpClient.GetAsync($"{AppConfig.BaseUrl}{endpoint}");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    _logger.LogDebug($"Resposta recebida: {content}");
                    var produto = JsonSerializer.Deserialize<ProdutoDTO>(content, _serializerOptions);
                    return (produto, null);
                }
                else
                {
                    _logger.LogError($"Erro na requisição: {response.ReasonPhrase}");
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return (null, $"Produto com id={id} não encontrado.");
                    }

                    string generalErrorMessage = $"Erro na requisição: {response.ReasonPhrase}";
                    _logger.LogError(generalErrorMessage);
                    return (null, generalErrorMessage);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"Erro inesperado: {ex.Message}";
                _logger.LogError(errorMessage);
                return (null, errorMessage);
            }
        }


        public async Task<(T? Data, string? ErrorMessage)> GetAsync<T>(string endpoint)
        {
            try
            {
                //AddAuthorizationHeader();
                var response = await _httpClient.GetAsync(AppConfig.BaseUrl + endpoint);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<T>(responseString, _serializerOptions);
                    return (data ?? Activator.CreateInstance<T>(), null);
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        string errorMessage = "Unauthorized";
                        _logger.LogWarning(errorMessage);
                        return (default, errorMessage);
                    }
                    string generalErrorMessage = $"Erro na requisição: {response.ReasonPhrase}";
                    _logger.LogError(generalErrorMessage);
                    return (default, generalErrorMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                string errrMessage = $"Erro de requisição HTTP: {ex.Message}";
                _logger.LogError(errrMessage);
                return (default, errrMessage);
            }
            catch (JsonException ex)
            {
                string errorMessage = $"Erro de desserialização JSON: {ex.Message}";
                _logger.LogError(ex.Message);
                return (default, errorMessage);
            }
            catch (Exception ex)
            {
                string errorMessage = $"Erro inesperado: {ex.Message}";
                _logger.LogError(ex.Message);
                return (default, errorMessage);
            }
        }

        // ***************** Compras ******************
        public async Task<ApiResponse<bool>> AdicionaItemNoCarrinho(GerirVenda carrinhoCompra)
        {
            AddAuthorizationHeader();
            try
            {

                //_logger.LogInformation($"Número de itens no carrinho: {carrinhoCompra.Itens.Count}");
                //foreach (var item in carrinhoCompra.Itens){
               
                //    _logger.LogInformation($"Item: { item.Produto.Nome}");
                //    _logger.LogInformation($"---------<<<<<<<> {item.Quantidade}" );
                //}

                string endpoint = "api/Encomendas";
                var json = JsonSerializer.Serialize(carrinhoCompra, _serializerOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                //_logger.LogInformation($"Corpo da requisição: {json}");
                //_logger.LogInformation($"Request URL: {AppConfig.BaseUrl}{endpoint}");

                var response = await PostRequest($"{AppConfig.BaseUrl}{endpoint}", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Erro ao enviar requisição HTTP: {response.StatusCode}. Detalhes: {errorDetails}");
                    return new ApiResponse<bool>
                    {
                        ErrorMessage = $"Erro ao enviar requisição HTTP: {response.StatusCode}"
                    };
                }

                return new ApiResponse<bool> { Data = true };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao adicionar item no carrinho de compras: {ex.Message}");
                return new ApiResponse<bool> { ErrorMessage = ex.Message };
            }
        }

        // ****************** Utilizadores ********************
        public async Task<ApiResponse<bool>> RegistarUtilizador(Utilizador novoUtilizador)
        {
            try
            {
                string endpoint = "api/Utilizadores/RegistarUser";
                var json = JsonSerializer.Serialize(novoUtilizador, _serializerOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await PostRequest($"{AppConfig.BaseUrl}{endpoint}", content);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    if (errorDetails.Contains("Já existe um utilizador com este email"))
                    {
                        _logger.LogWarning("Erro: Utilizador com o e-mail já existe.");
                        return new ApiResponse<bool> { ErrorMessage = "Já existe um utilizador com este e-mail." };
                    }
                    else
                    {
                        _logger.LogError($"Erro ao enviar requisição. Status: {response.StatusCode}, Detalhes: {errorDetails}");
                        return new ApiResponse<bool> { ErrorMessage = "Erro ao enviar requisição." };
                    }
                }

                var jsonResult = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Resposta da API: {Response}", jsonResult);

                var result = JsonSerializer.Deserialize<Token>(jsonResult, _serializerOptions);
                if (result == null || string.IsNullOrEmpty(result.AccessToken))
                {
                    _logger.LogError("Token não retornado ou inválido.");
                    return new ApiResponse<bool> { ErrorMessage = "Token não retornado ou inválido." };
                }

                _token = result.AccessToken;
                return new ApiResponse<bool> { Data = true };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro inesperado: {ex.Message}");
                return new ApiResponse<bool> { ErrorMessage = $"Erro inesperado: {ex.Message}" };
            }
        }


        public async Task<ApiResponse<bool>> Login(LoginModel login)
        {
            try
            {
                string endpoint = "api/Utilizadores/LoginUser";
                var json = JsonSerializer.Serialize(login, _serializerOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await PostRequest($"{AppConfig.BaseUrl}{endpoint}", content);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Erro ao enviar requisição Http: {response.StatusCode}");
                    _logger.LogInformation("JSON enviado: {Json}", json);

                }

                var jsonResult = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Resposta da API: {Response}", jsonResult);

                var result = JsonSerializer.Deserialize<Token>(jsonResult, _serializerOptions);
                if (result == null || string.IsNullOrEmpty(result.AccessToken))
                {
                    _logger.LogError("Token não retornado ou inválido");
                    return new ApiResponse<bool> { ErrorMessage = "Token não retornado ou inválido" };
                }
                _token = result.AccessToken;
                userID = login.EMail;
                return new ApiResponse<bool> { Data = true };
            }
            catch (Exception ex)
            {
                string errorMessage = $"Erro inesperado: {ex.Message}";
                _logger.LogError(ex.Message);
                return (default);
            }

        }
        private async Task<HttpResponseMessage> PostRequest(string enderecoURL, HttpContent content)
        {
            try
            {
                var result = await _httpClient.PostAsync(enderecoURL, content);
                return result;
            }
            catch (Exception ex)
            {
                // Log o erro ou trata conforme necessario
                _logger.LogError($"Erro ao enviar requisição POST para enderecoURL: {ex.Message}");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        // *************** Gerir Favoritos ******************

        public async Task<List<ProdutoFavorito>> GetFavoritos(string utilizadorId)
        {
            AddAuthorizationHeader();
            string endpoint = $"api/Favoritos/{utilizadorId}";

            // AddAuthorizationHeader();
            HttpResponseMessage response = await _httpClient.GetAsync($"{AppConfig.BaseUrl}{endpoint}");

            var responseString = await response.Content.ReadAsStringAsync();
            List<ProdutoFavorito> data = JsonSerializer.Deserialize<List<ProdutoFavorito>>(responseString, _serializerOptions);

            return data;

        }
        public async Task<(bool Data, string? ErrorMessage)> ActualizaFavorito(string acao, int produtoId)
        {
            AddAuthorizationHeader();
            try
            {
                var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

                var response = await FavoritosPutRequest($"api/Favoritos", content);

                if (!response.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        string errorMessage = "Unauthorized";
                        _logger.LogWarning(errorMessage);
                        return (false, errorMessage);
                    }
                    string generalErrorMessage = $"Erro na requisição: {response.ReasonPhrase}";
                    _logger.LogError(generalErrorMessage);
                    return (false, generalErrorMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                string errorMessage = $"Erro de requisição HTTP: {ex.Message}";
                _logger.LogError(errorMessage);
                return (false, errorMessage);
            }
            catch (Exception ex)
            {
                string errorMessage = $"Erro inesperado: {ex.Message}";
                _logger.LogError(errorMessage);
                return (false, errorMessage);
            }
        }
    public async Task<ApiResponse<bool>> AdicionaFavorito(FavoritosDTO novoFavorito)
    {
        AddAuthorizationHeader();
        if (novoFavorito == null)
        {
            _logger.LogError("Favorito vazio");
            return new ApiResponse<bool> { ErrorMessage = "Favorito vazio" };
        }

        try
        {

           
            string endpoint = "api/Favoritos";
            var json = JsonSerializer.Serialize(novoFavorito, _serializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            //var response = await FavoritosPutRequest(endpoint, content);

            var response = await _httpClient.PostAsync($"{AppConfig.BaseUrl}{endpoint}", content);

            _logger.LogInformation("Resposta da API: {Response}", response);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Favorito adicionado com sucesso.");
                return new ApiResponse<bool> { Data = true };

            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Erro ao adicionar favorito: {errorContent}");
                return new ApiResponse<bool> { Data = false, ErrorMessage = "Erro na validação dos dados enviados." };
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Erro inesperado ao adicionar favorito: {errorContent}");
                return new ApiResponse<bool> { Data = false, ErrorMessage = "Erro inesperado ao adicionar o favorito." };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exceção ao adicionar favorito: {ex.Message}");
            return new ApiResponse<bool> { Data = false, ErrorMessage = "Erro ao comunicar com a API." };
        }
    }


    private async Task<HttpResponseMessage> FavoritosPutRequest(string uri, HttpContent content)
        {
            var enderecoUrl = AppConfig.BaseUrl + uri;
            try
            {
                AddAuthorizationHeader();
                var result = await _httpClient.PutAsync(enderecoUrl, content);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao enviar requisição PUT para {uri}: {ex.Message}");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        public async Task<CarrinhoDTO> GetCarrinho()
        {
            try
            {
                _logger.LogInformation("UserID: {UserID}", userID);
                if (string.IsNullOrEmpty(userID))
                    return null;

                string endpoint = $"api/Carrinho/{userID}";
                _logger.LogInformation("Endpoint: {Endpoint}", endpoint);
                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync($"{AppConfig.BaseUrl}{endpoint}");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string content = await httpResponseMessage.Content.ReadAsStringAsync();
                    _logger.LogInformation("Carrinho recebido: {Content}", content);

                    var carrinho = JsonSerializer.Deserialize<CarrinhoDTO>(content, _serializerOptions);
                    return carrinho;
                }
                else
                {
                    _logger.LogError("Erro ao buscar carrinho: {ReasonPhrase}", httpResponseMessage.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao obter carrinho: {ex.Message}");
                return null;
            }

            return null;
        }

        public async Task<ApiResponse<bool>> SetCarrinho(CarrinhoDTO carrinho){
            try
            {
                _logger.LogInformation("UserID: {UserID}", userID);
                if (string.IsNullOrEmpty(userID))
                    return null;

                var json = JsonSerializer.Serialize(carrinho, _serializerOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                string endpoint = $"api/Carrinho/{userID}";

                _logger.LogInformation("Endpoint: {Endpoint}", endpoint);
                var response = await PostRequest($"{AppConfig.BaseUrl}{endpoint}", content);

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse<bool> { Data = true };
                }
                else
                {
                    string errorMessage = $"Erro ao atualizar o carrinho: {response.ReasonPhrase}";
                    _logger.LogError(errorMessage);
                    return new ApiResponse<bool> { ErrorMessage = errorMessage };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao adicionar item no carrinho: {ex.Message}");
                return new ApiResponse<bool> { ErrorMessage = ex.Message };
            }
        }

        public async Task<Utilizador?> GetUtilizadorAtual()
        {
            try
            {
                var utilizadorAtual = new Utilizador{
                   Email = userID
                };

            
                await Task.Delay(100);
                return utilizadorAtual;
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro inesperado ao receber utilizador atual: {Message}", ex.Message);
            }
            return null;
        }

    }
