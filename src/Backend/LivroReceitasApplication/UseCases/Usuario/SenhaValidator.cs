﻿using FluentValidation;
using LivroReceitas.Exceptions;

namespace LivroReceitas.Application.UseCases.Usuario;
public class SenhaValidator  :AbstractValidator<string>
{
	public SenhaValidator()
	{
		RuleFor(senha => senha).NotEmpty().WithMessage(ResourceMensagensDeErro.SENHA_USUARIO_EMBRANCO);
		When(senha => !string.IsNullOrWhiteSpace(senha), () =>
		{
			RuleFor(senha => senha.Length).GreaterThanOrEqualTo(6).WithMessage(ResourceMensagensDeErro.SENHA_USUARIO_MINIMO_SEIS_CARACTERES);
		});
	}
}
