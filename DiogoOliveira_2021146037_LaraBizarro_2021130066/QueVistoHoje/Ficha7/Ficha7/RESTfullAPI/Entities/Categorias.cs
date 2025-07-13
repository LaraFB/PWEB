using System.ComponentModel.DataAnnotations.Schema;

namespace RESTfullAPI.Entities;
public class Categorias
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public int? Ordem { get; set; }
        public string? UrlImagem { get; set; }

        public byte[]? Imagem { get; set; }

        public List<SubCategoria> SubCategorias { get; set; } = new List<SubCategoria>();

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }