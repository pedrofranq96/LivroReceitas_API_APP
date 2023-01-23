using LivroReceitas.Domain.Entidades;

namespace LivroReceitas.Domain.Repositorio.Usuario;

public interface IUsuarioReadOnlyRepositorio
{
    Task<bool> ExiteUsuarioComEmail(string email);
    Task<Entidades.Usuario> Login(string email, string senha);
    Task<Entidades.Usuario> RecuperarPorEmail(string email);
}
