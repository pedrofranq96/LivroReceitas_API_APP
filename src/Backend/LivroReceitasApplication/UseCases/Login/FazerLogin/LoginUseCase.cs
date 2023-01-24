using LivroReceitas.Application.Servicos.Criptografia;
using LivroReceitas.Application.Servicos.Token;
using LivroReceitas.Comunicacao.Requesicoes;
using LivroReceitas.Comunicacao.Respostas;
using LivroReceitas.Domain.Repositorio.Usuario;
using LivroReceitas.Exceptions.ExceptionsBase;

namespace LivroReceitas.Application.UseCases.Login.FazerLogin;

public class LoginUseCase : ILoginUseCase
{
	private readonly EncriptadorDeSenha _encriptadorDeSenha;
	private readonly TokenController _tokenController;
	private readonly IUsuarioReadOnlyRepositorio _repositorioRead;

	public LoginUseCase(EncriptadorDeSenha encriptadorDeSenha, 
		TokenController tokenController, IUsuarioReadOnlyRepositorio repositorioRead)
	{
		_encriptadorDeSenha = encriptadorDeSenha;
		_tokenController = tokenController;
		_repositorioRead = repositorioRead;
	}

	public async Task<RespostaLoginJson> Executar(RequisicaoLoginJson request)
	{
		var senhaCripto = _encriptadorDeSenha.Criptografar(request.Senha);

		var usuario = await _repositorioRead.Login(request.Email, senhaCripto);

		if (usuario == null)
		{
			throw new LoginInvalidoException();
		}
		return new RespostaLoginJson
		{
			Nome = usuario.Nome,
			Token = _tokenController.GerarToken(usuario.Email)
		};
	}
}
