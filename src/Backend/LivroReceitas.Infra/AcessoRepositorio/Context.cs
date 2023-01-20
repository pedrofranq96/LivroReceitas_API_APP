using LivroReceitas.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace LivroReceitas.Infra.AcessoRepositorio;

public class Context :DbContext
{
	public Context(DbContextOptions<Context> options): base(options) { }

	public DbSet<Usuario> Usuarios { get; set; }
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(Context).Assembly);
	}
}
