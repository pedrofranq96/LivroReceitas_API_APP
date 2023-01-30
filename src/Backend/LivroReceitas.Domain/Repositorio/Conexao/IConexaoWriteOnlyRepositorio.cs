namespace LivroReceitas.Domain.Repositorio.Conexao;
public interface IConexaoWriteOnlyRepositorio
{
	Task Registrar(Entidades.Conexao conexao);
}
