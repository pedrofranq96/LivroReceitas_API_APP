namespace LivroReceitas.Application.UseCases.Conexao.AceitarConexao;
public interface IAceitarConexaoUseCase
{
	Task<string> Executar(string usuarioParaSeConectarId);
}
