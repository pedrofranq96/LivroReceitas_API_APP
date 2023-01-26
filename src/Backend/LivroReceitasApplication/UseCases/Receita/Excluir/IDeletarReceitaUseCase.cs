namespace LivroReceitas.Application.UseCases.Receita.Excluir;
public interface IDeletarReceitaUseCase
{
	Task Executar(long id);
}
