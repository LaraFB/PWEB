using GestaoLojaTP.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GestaoLojaTP.Data{
	public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : 
		IdentityDbContext<ApplicationUser>(options){
		public DbSet<Categorias> Categorias { get; set; }

		public DbSet<Produtos> Produtos { get; set; }

		public DbSet<ModoEntrega> ModoEntrega { get; set; }

		public DbSet<GerirVenda> GerirVendas { get; set; }

        public DbSet<VendaProduto> VendaProdutos { get; set; }

        public DbSet<ModoVenda> ModosVenda { get; set; }

        public DbSet<SubCategoria> SubCategorias { get; set; }

		public DbSet<Favoritos> Favoritos { get; set; }
    }
}
