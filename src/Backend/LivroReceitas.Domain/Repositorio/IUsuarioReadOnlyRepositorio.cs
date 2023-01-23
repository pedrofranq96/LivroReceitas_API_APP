using LivroReceitas.Domain.Entidades;

namespace LivroReceitas.Domain.Repositorio;

public interface IUsuarioReadOnlyRepositorio
{
	Task<bool> ExiteUsuarioComEmail(string email);
	Task<Usuario> Login(string email, string senha);
}
