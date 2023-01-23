using LivroReceitas.Domain.Entidades;

namespace LivroReceitas.Domain.Repositorio.Usuario;

public interface IUsuarioWriteOnlyRepositorio
{
    Task Adicionar(Entidades.Usuario usuario);


}
