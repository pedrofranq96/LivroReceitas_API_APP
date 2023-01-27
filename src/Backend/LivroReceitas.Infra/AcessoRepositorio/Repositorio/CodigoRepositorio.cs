using LivroReceitas.Domain.Entidades;
using LivroReceitas.Domain.Repositorio.Codigos;
using Microsoft.EntityFrameworkCore;

namespace LivroReceitas.Infra.AcessoRepositorio.Repositorio;
public class CodigoRepositorio : ICodigoWriteOnlyRepositorio
{
	private readonly Context _context;

	public CodigoRepositorio(Context context)
	{
		_context = context;
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
