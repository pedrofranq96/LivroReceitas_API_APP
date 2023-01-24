using LivroReceitas.Comunicacao.Requesicoes;

namespace LivroReceitas.Application.UseCases.Usuario.AlterarSenha;
public interface IAlterarSenhaUseCase
{
	Task Executar(RequisicaoAlterarSenhaJson requisicao);
}
