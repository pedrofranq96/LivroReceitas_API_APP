using LivroReceitas.Application.Servicos.Token;
using LivroReceitas.Domain.Entidades;
using LivroReceitas.Domain.Repositorio.Usuario;
using Microsoft.AspNetCore.Http;

namespace LivroReceitas.Application.Servicos.UsuarioLogado;
public class UsuarioLogado : IUsuarioLogado
{
	private readonly IHttpContextAccessor _contextAccessor;
	private readonly TokenController _tokenController;
	private readonly IUsuarioReadOnlyRepositorio _repositorio;

	public UsuarioLogado(IHttpContextAccessor contextAccessor, 
		TokenController tokenController, IUsuarioReadOnlyRepositorio repositorio)
	{
		_contextAccessor = contextAccessor;
		_tokenController = tokenController;
		_repositorio = repositorio;
	}

	public async Task<Usuario> RecuperarUsuario()
	{
		var authorization = _contextAccessor.HttpContext.Request.Headers["Authorization"].ToString();	
		var token = authorization["Bearer".Length..].Trim();
		var emailUsuario = _tokenController.RecuperarEmail(token);

		var usuario = await _repositorio.RecuperarPorEmail(emailUsuario);

		return usuario;
	}
}
