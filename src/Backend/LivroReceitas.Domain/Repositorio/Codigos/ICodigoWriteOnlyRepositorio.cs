namespace LivroReceitas.Domain.Repositorio.Codigos;
public interface ICodigoWriteOnlyRepositorio
{
	Task Registrar(Entidades.Codigos codigo);
	Task Deletar(long usuarioId);
}
