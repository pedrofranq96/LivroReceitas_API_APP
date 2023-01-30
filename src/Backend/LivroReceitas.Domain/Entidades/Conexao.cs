namespace LivroReceitas.Domain.Entidades;
public class Conexao :EntidadeBase
{
	public long UsuarioId { get; set; }
	public long ConecatadoComUsuarioId { get; set; }
}
