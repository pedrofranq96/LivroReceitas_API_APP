using LivroReceitas.Comunicacao.Requesicoes;
using LivroReceitas.Comunicacao.Respostas;

namespace LivroReceitas.Application.UseCases.Receita.Registrar;
public interface IRegistrarReceitaUseCase
{
	Task<RespostaReceitaJson> Executar(RequisicaoReceitaJson requisicao);
}
