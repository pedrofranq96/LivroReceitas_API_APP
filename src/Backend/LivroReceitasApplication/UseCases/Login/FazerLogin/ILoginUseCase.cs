using LivroReceitas.Comunicacao.Requesicoes;
using LivroReceitas.Comunicacao.Respostas;

namespace LivroReceitas.Application.UseCases.Login.FazerLogin;

public interface ILoginUseCase
{
	Task<RespostaLoginJson> Executar(RequisicaoLoginJson request);
}
