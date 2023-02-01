namespace LivroReceitas.Domain.Repositorio.Receita;
public interface IReceitaUpdateOnlyRepositorio
{
	void Update(Entidades.Receita receita);
	Task<Entidades.Receita> RecuperarPorId(long receitaId);
}
