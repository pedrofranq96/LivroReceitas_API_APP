using LivroReceitas.Comunicacao.Respostas;

namespace LivroReceitas.Application.UseCases.Usuario.RecuperarPerfil;
public interface IRecuperarPerfilUseCase
{
	Task<RespostaPerfilUsuarioJson> Executar();
}
