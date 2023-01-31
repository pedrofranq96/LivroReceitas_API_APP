using LivroReceitas.Comunicacao.Respostas;

namespace LivroReceitas.Application.UseCases.Conexao.Recuperar;
public interface IRecuperarTodasConexoesUseCase
{
	Task<IList<RespostaUsuarioConectadoJson>> Executar();
}
