using LivroReceitas.Domain.Entidades;
using LivroReceitas.Domain.Repositorio.Receita;

namespace LivroReceitas.Infra.AcessoRepositorio.Repositorio;
public class ReceitaRepositorio : IReceitaWriteOnlyRepositorio
{
	private readonly Context _context;

	public ReceitaRepositorio(Context context)
	{
		_context = context;
	}

	public async Task Registrar(Receita receita)
	{
		await _context.Receitas.AddAsync(receita);
	}
}
