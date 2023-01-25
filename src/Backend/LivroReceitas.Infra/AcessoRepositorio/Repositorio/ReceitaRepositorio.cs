using LivroReceitas.Domain.Entidades;
using LivroReceitas.Domain.Repositorio.Receita;
using Microsoft.EntityFrameworkCore;

namespace LivroReceitas.Infra.AcessoRepositorio.Repositorio;
public class ReceitaRepositorio : IReceitaWriteOnlyRepositorio, IReceitaReadOnlyRepositorio
{
	private readonly Context _context;

	public ReceitaRepositorio(Context context)
	{
		_context = context;
	}

	public async Task<IList<Receita>> RecuperarTodasDoUsuario(long usuarioId)
	{
		return await _context.Receitas.Where(r => r.UsuarioId == usuarioId).ToListAsync();
	}

	public async Task Registrar(Receita receita)
	{
		await _context.Receitas.AddAsync(receita);
	}
}
