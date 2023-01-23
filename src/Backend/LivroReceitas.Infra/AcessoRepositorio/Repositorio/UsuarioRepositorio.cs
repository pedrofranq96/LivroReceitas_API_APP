using LivroReceitas.Domain.Entidades;
using LivroReceitas.Domain.Repositorio.Usuario;
using Microsoft.EntityFrameworkCore;

namespace LivroReceitas.Infra.AcessoRepositorio.Repositorio;

public class UsuarioRepositorio : IUsuarioWriteOnlyRepositorio, IUsuarioReadOnlyRepositorio, IUpdateOnlyRepositorio
{
    private readonly Context _context;

	public UsuarioRepositorio(Context context)
	{
		_context = context;
	}

	public async Task Adicionar(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
    }

    public async Task<bool> ExiteUsuarioComEmail(string email)
    {
        return await _context.Usuarios.AnyAsync(c => c.Email.Equals(email));
    }

	public async Task<Usuario> Login(string email, string senha)
	{
		return await _context.Usuarios.AsNoTracking()
			.FirstOrDefaultAsync(c=> c.Email.Equals(email) && c.Senha.Equals(senha));
	}

	public void Update(Usuario usuario)
	{
		_context.Usuarios.Update(usuario);
	}
}
