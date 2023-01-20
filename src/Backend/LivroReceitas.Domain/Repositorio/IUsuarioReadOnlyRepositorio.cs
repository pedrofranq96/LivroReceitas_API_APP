namespace LivroReceitas.Domain.Repositorio;

public interface IUsuarioReadOnlyRepositorio
{
	Task<bool> ExiteUsuarioComEmail(string email);
}
