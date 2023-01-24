using LivroReceitas.Domain.Entidades;
using LivroReceitas.Domain.Repositorio.Usuario;
using Moq;

namespace UtilitarioParaOsTestes.Repositorios;

public class UsuarioReadOnlyRepositorioBuilder
{
	private static UsuarioReadOnlyRepositorioBuilder _instance;
	private readonly Mock<IUsuarioReadOnlyRepositorio> _repositorio;

	private UsuarioReadOnlyRepositorioBuilder()
	{
		if (_repositorio == null)
		{
			_repositorio = new Mock<IUsuarioReadOnlyRepositorio>();
		}
	}

	public static UsuarioReadOnlyRepositorioBuilder Intancia()
	{
		_instance = new UsuarioReadOnlyRepositorioBuilder();
		return _instance;
	}
	public UsuarioReadOnlyRepositorioBuilder ExiteUsuarioComEmail(string email)
	{
		if (!string.IsNullOrEmpty(email))
		{
			_repositorio.Setup(i => i.ExiteUsuarioComEmail(email)).ReturnsAsync(true);
		}			

		return this;
	}
	
	public UsuarioReadOnlyRepositorioBuilder Login(Usuario usuario)
	{
		_repositorio.Setup(i=> i.Login(usuario.Email, usuario.Senha)).ReturnsAsync(usuario);		

		return this;
	}
	public IUsuarioReadOnlyRepositorio Construir()
	{
		return _repositorio.Object;
	}
}
