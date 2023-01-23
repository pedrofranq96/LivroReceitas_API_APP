using FluentValidation;
using LivroReceitas.Comunicacao.Requesicoes;
using LivroReceitas.Exceptions;
using System.Text.RegularExpressions;

namespace LivroReceitas.Application.UseCases.Usuario.AlterarSenha;
public class AlterarSenhaValidator : AbstractValidator<RequisicaoAlterarSenhaJson>
{
	public AlterarSenhaValidator()
	{
		RuleFor(c => c.NovaSenha).SetValidator(new SenhaValidator());
		
	}
}
