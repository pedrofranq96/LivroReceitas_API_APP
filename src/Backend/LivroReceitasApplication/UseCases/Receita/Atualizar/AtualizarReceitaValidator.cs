using FluentValidation;
using LivroReceitas.Comunicacao.Requesicoes;
using LivroReceitas.Exceptions;

namespace LivroReceitas.Application.UseCases.Receita.Atualizar;
public class AtualizarReceitaValidator : AbstractValidator<RequisicaoReceitaJson>
{
	public AtualizarReceitaValidator()
	{
		RuleFor(x => x).SetValidator(new ReceitaValidator());
	}
}