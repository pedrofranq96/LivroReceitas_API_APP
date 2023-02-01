using LivroReceitas.Application.Servicos.Token;
using LivroReceitas.Domain.Repositorio.Usuario;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace LivroReceitas.API.Filtros.UsuarioLogado;

public class UsuarioLogadoHandler : AuthorizationHandler<UsuarioLogadoRequirement>
{
	private readonly IHttpContextAccessor _contextAccessor;
	private readonly TokenController _tokenController;
	private readonly IUsuarioReadOnlyRepositorio _repositorio;

	public UsuarioLogadoHandler(IHttpContextAccessor contextAccessor, TokenController tokenController, IUsuarioReadOnlyRepositorio repositorio)
	{
		_contextAccessor = contextAccessor;
		_tokenController = tokenController;
		_repositorio = repositorio;
	}

	protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UsuarioLogadoRequirement requirement)
	{
		try
		{
			var authorization = _contextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

			if (string.IsNullOrWhiteSpace(authorization))
			{
				context.Fail();
				return;
			}

			var token = authorization["Bearer".Length..].Trim();

			var emailUsuario = _tokenController.RecuperarEmail(token);

			var usuario = await _repositorio.RecuperarPorEmail(emailUsuario);

			if (usuario is null)
			{
				context.Fail();
				return;
			}

			context.Succeed(requirement);
		}
		catch 
		{
			context.Fail();
		}
		
	}

}
