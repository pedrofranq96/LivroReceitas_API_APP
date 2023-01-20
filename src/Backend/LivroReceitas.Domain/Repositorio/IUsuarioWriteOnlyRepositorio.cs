using LivroReceitas.Domain.Entidades;

namespace LivroReceitas.Domain.Repositorio;

public interface IUsuarioWriteOnlyRepositorio
{
	Task Adicionar(Usuario usuario);


}
