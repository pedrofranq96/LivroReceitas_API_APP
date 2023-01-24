using LivroReceitas.Infra.AcessoRepositorio;
using UtilitarioParaOsTestes.Entidades;

namespace WebApi.Test;

public class ContextSeedInMemory
{
	public static (LivroReceitas.Domain.Entidades.Usuario, string senha) Seed(Context context)
	{
		(var usuario,string senha) = UsuarioBuilder.Construir();

		context.Usuarios.Add(usuario);

		context.SaveChanges();

		return (usuario, senha);
	}
}