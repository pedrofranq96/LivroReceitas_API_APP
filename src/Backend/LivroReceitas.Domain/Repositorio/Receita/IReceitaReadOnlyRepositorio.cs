namespace LivroReceitas.Domain.Repositorio.Receita;
public interface IReceitaReadOnlyRepositorio
{
	Task<IList<Entidades.Receita>> RecuperarTodasDoUsuario(long usuarioId);
}
