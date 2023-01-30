using LivroReceitas.Domain.Entidades;
using LivroReceitas.Domain.Repositorio.Codigos;
using Moq;

namespace UtilitarioParaOsTestes.Repositorios;
public class CodigoReadOnlyRepositorioBuilder
{
	private static CodigoReadOnlyRepositorioBuilder _instance;
	private readonly Mock<ICodigoReadOnlyRepositorio> _repositorio;

	private CodigoReadOnlyRepositorioBuilder()
	{
		if (_repositorio is null)
		{
			_repositorio = new Mock<ICodigoReadOnlyRepositorio>();
		}
	}

	public static CodigoReadOnlyRepositorioBuilder Instancia()
	{
		_instance = new CodigoReadOnlyRepositorioBuilder();
		return _instance;
	}

	public CodigoReadOnlyRepositorioBuilder RecuperarEntidadeCodigo(Codigos codigo)
	{
		_repositorio.Setup(x => x.RecuperarEntidadeCodigo(codigo.Codigo)).ReturnsAsync(codigo);
		return this;
	}

	public ICodigoReadOnlyRepositorio Construir()
	{
		return _repositorio.Object;
	}
}
