using LivroReceitas.Comunicacao.Requesicoes;
using LivroReceitas.Comunicacao.Respostas;

namespace LivroReceitas.Application.UseCases.Usuario.Registrar;

public interface IRegistrarUsuarioUseCase
{
	Task<RespostaUsuarioRegistradoJson> Executar(RequisicaoRegistrarUsuarioJson requisicao);
}
