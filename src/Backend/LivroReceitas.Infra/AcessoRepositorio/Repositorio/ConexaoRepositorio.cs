using LivroReceitas.Domain.Entidades;
using LivroReceitas.Domain.Repositorio.Conexao;
using Microsoft.EntityFrameworkCore;

namespace LivroReceitas.Infra.AcessoRepositorio.Repositorio;
public class ConexaoRepositorio : IConexaoReadOnlyRepositorio, IConexaoWriteOnlyRepositorio
{
	private readonly Context _context;

	public ConexaoRepositorio(Context context)
	{
		_context = context;
	}

	public async Task<bool> ExisteConexao(long idUsuarioA, long idUsuarioB)
	{
		return await _context.Conexoes.AnyAsync(c => c.UsuarioId == idUsuarioA && c.ConecatadoComUsuarioId == idUsuarioB);
	}

	public async Task<IList<Usuario>> RecuperarUsuario(long usuarioId)
	{
		return await _context.Conexoes.AsNoTracking()
			.Include(c => c.ConecatadoComUsuario)
			.Where(c => c.UsuarioId == usuarioId)
			.Select(c => c.ConecatadoComUsuario)
			.ToListAsync();
	}

	public async Task Registrar(Conexao conexao)
	{
		await _context.Conexoes.AddAsync(conexao);
	}
}
