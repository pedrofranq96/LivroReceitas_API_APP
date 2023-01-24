using LivroReceitas.Application.Servicos.Token;
using LivroReceitas.Comunicacao.Respostas;
using LivroReceitas.Domain.Repositorio.Usuario;
using LivroReceitas.Exceptions;
using LivroReceitas.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace LivroReceitas.API.Filtros;

public class UsuarioAutenticadoAtributo : AuthorizeAttribute, IAsyncAuthorizationFilter
{
	private readonly TokenController _tokenController;
	private readonly IUsuarioReadOnlyRepositorio _repositorio;

	public UsuarioAutenticadoAtributo(TokenController tokenController, IUsuarioReadOnlyRepositorio repositorio)
	{
		_tokenController = tokenController;
		_repositorio = repositorio;
	}

	public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
	{
		try
		{
			var token = TokenNaRequisicao(context);
			var emailUsuario = _tokenController.RecuperarEmail(token);

			var usuario = await _repositorio.RecuperarPorEmail(emailUsuario);

			if (usuario is null)
			{
				throw new LivroReceitasExceptions(string.Empty);
			}
		}
		catch (SecurityTokenExpiredException)
		{
			TokenExpirado(context);
			
		}
		catch 
		{
			UsuarioSemPermissao(context);
		}

		


	}

	private static string TokenNaRequisicao(AuthorizationFilterContext context)
	{
		var authorization = context.HttpContext.Request.Headers["Authorization"].ToString();

		if (string.IsNullOrWhiteSpace(authorization))
		{
			throw new LivroReceitasExceptions(string.Empty);
		}
		return authorization["Bearer".Length..].Trim();
	}


	private static void TokenExpirado(AuthorizationFilterContext context)
	{
		context.Result = new UnauthorizedObjectResult(new RespostaErroJson(ResourceMensagensDeErro.TOKEN_EXPIRADO));
	}
	private static void UsuarioSemPermissao(AuthorizationFilterContext context)
	{
		context.Result = new UnauthorizedObjectResult(new RespostaErroJson(ResourceMensagensDeErro.USUARIO_SEM_PERMISSAO));
	}
}
