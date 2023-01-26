using FluentValidation;
using FluentValidation.Results;
using LivroReceitas.Comunicacao.Requesicoes;
using LivroReceitas.Domain.Entidades;
using LivroReceitas.Exceptions;

namespace LivroReceitas.Application.UseCases.Receita.Registrar;
public class RegistrarReceitaValidator : AbstractValidator<RequisicaoReceitaJson>
{
	public RegistrarReceitaValidator()
	{
		RuleFor(x => x).SetValidator(new ReceitaValidator());
	}
}
