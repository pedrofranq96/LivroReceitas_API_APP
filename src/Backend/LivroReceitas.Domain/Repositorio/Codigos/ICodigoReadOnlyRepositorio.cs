namespace LivroReceitas.Domain.Repositorio.Codigos;
public interface ICodigoReadOnlyRepositorio
{
	Task<Entidades.Codigos> RecuperarEntidadeCodigo(string codigo);
}
