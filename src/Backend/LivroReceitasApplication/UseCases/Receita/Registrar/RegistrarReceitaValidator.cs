using FluentValidation;
using FluentValidation.Results;
using LivroReceitas.Comunicacao.Requesicoes;
using LivroReceitas.Domain.Entidades;

namespace LivroReceitas.Application.UseCases.Receita.Registrar;
public class RegistrarReceitaValidator : AbstractValidator<RequisicaoReceitaJson>
{
	public RegistrarReceitaValidator()
	{
		RuleFor(x => x.Titulo).NotEmpty();
		RuleFor(x => x.Categoria).IsInEnum();
		RuleFor(x => x.ModoPreparo).NotEmpty();
		RuleFor(x => x.Ingredientes).NotEmpty();
		RuleForEach(x => x.Ingredientes).ChildRules(ingrediente =>
		{
			ingrediente.RuleFor(x=> x.Produto).NotEmpty();
			ingrediente.RuleFor(x=> x.Quantidade).NotEmpty();
		});

		RuleFor(x => x.Ingredientes).Custom((ingredientes, contexto)=>
		{
			var produtosDistintos = ingredientes.Select(c => c.Produto).Distinct();
			if (produtosDistintos.Count() != ingredientes.Count())
			{
				contexto.AddFailure(new FluentValidation.Results.ValidationFailure("Ingredientes", ""));
			}
		});
	}
}
