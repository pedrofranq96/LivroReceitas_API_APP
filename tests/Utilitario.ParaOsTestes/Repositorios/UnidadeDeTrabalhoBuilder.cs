using LivroReceitas.Domain.Repositorio;
using Moq;

namespace UtilitarioParaOsTestes.Repositorios;

public class UnidadeDeTrabalhoBuilder
{
	private static UnidadeDeTrabalhoBuilder _instance;
	private readonly Mock<IUnidadeDeTrabalho> _repositorio;

	private UnidadeDeTrabalhoBuilder()
	{
		if (_repositorio == null) _repositorio = new Mock<IUnidadeDeTrabalho>();
	}

	public static UnidadeDeTrabalhoBuilder Intancia()
	{
		_instance = new UnidadeDeTrabalhoBuilder();
		return _instance;
	}

	public IUnidadeDeTrabalho Construir()
	{
		return _repositorio.Object;
	}
}
