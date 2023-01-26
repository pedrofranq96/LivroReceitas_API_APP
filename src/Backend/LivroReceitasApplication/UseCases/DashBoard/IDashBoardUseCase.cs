using LivroReceitas.Comunicacao.Requesicoes;
using LivroReceitas.Comunicacao.Respostas;

namespace LivroReceitas.Application.UseCases.DashBoard;
public interface IDashBoardUseCase
{
	Task<RespostaDashBoardJson> Executar(RequisicaoDashBoardJson requisicao);
}
