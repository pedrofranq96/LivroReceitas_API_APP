namespace LivroReceitas.Application.UseCases.Conexao.RemoverConexao;
public interface IRemoverConexaoUseCase
{
	Task Executar(long idUsuarioConectadoParaRemover);
}
