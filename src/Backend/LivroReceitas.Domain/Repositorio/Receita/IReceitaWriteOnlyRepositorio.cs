namespace LivroReceitas.Domain.Repositorio.Receita;
public interface IReceitaWriteOnlyRepositorio
{
	Task Registrar(Entidades.Receita receita);
}
