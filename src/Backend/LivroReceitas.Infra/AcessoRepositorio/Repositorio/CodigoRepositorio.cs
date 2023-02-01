using LivroReceitas.Domain.Entidades;
using LivroReceitas.Domain.Repositorio.Codigos;
using Microsoft.EntityFrameworkCore;

namespace LivroReceitas.Infra.AcessoRepositorio.Repositorio;

public class CodigoRepositorio : ICodigoWriteOnlyRepositorio, ICodigoReadOnlyRepositorio
{
	private readonly Context _context;

	public CodigoRepositorio(Context context)
	{
		_context = context;
	}

	public async Task Deletar(long usuarioId)
	{
		var codigos = await _context.Codigos.Where(c=> c.UsuarioId == usuarioId).ToListAsync();
		if (codigos.Any())
		{
			_context.Codigos.RemoveRange(codigos);
		}
	}

	public async Task<Codigos> RecuperarEntidadeCodigo(string codigo)
	{
		return await _context.Codigos
			.AsNoTracking()
			.FirstOrDefaultAsync(c=> c.Codigo == codigo);
	}

	public async Task Registrar(Codigos codigo)
	{
		var codigoDb = await _context.Codigos.FirstOrDefaultAsync(c => c.UsuarioId == codigo.UsuarioId);

		if (codigoDb is not null)
		{
			codigoDb.Codigo = codigo.Codigo;
			_context.Codigos.Update(codigoDb);
		}
		else
		{
			await _context.Codigos.AddAsync(codigo);
		}
		
	}
}
