namespace LivroReceitas.Domain.Repositorio.Conexao;
public interface IConexaoReadOnlyRepositorio
{
	Task<bool> ExisteConexao(long idUsuarioA, long idUsuarioB);
	Task<IList<Entidades.Usuario>> RecuperarUsuario(long usuarioId);
}
