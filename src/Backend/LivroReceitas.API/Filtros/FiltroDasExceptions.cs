using LivroReceitas.Comunicacao.Respostas;
using LivroReceitas.Exceptions;
using LivroReceitas.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace LivroReceitas.API.Filtros;

public class FiltroDasExceptions : IExceptionFilter
{
	public void OnException(ExceptionContext context)
	{
		if (context.Exception is LivroReceitasExceptions)
		{
			TratarLivroReceitasException(context);
		}
		else
		{
			LancarErroDesconhecido(context);
		}
	}

	private static void LancarErroDesconhecido(ExceptionContext context)
	{
		context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
		context.Result = new ObjectResult(new RespostaErroJson(ResourceMensagensDeErro.ERRO_DESCONHECIDO));
	}

	private static void TratarLivroReceitasException(ExceptionContext context)
	{
		if(context.Exception is ErrosDeValidacaoException)
		{
			TratarErroDeValidacoesException(context);
		}
		else if(context.Exception is LoginInvalidoException)
		{
			TratarLoginException(context);
		}
	}

	private static void TratarErroDeValidacoesException(ExceptionContext context)
	{
		var erroValidacaoException = context.Exception as ErrosDeValidacaoException;

		context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
		context.Result = new ObjectResult(new RespostaErroJson(erroValidacaoException.MensagensDeErro));
	}

	private static void TratarLoginException(ExceptionContext context)
	{
		var erroLogin = context.Exception as LoginInvalidoException;
		context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
		context.Result = new ObjectResult(new RespostaErroJson(erroLogin.Message));
	}
}
