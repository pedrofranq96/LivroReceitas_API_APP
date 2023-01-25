﻿using LivroReceitas.Domain.Repositorio;
using Moq;

namespace UtilitarioParaOsTestes.Repositorios;

public class UnidadeDeTrabalhoBuilder
{
	private static UnidadeDeTrabalhoBuilder _instance;
	private readonly Mock<IUnidadeDeTrabalho> _repositorio;

	private UnidadeDeTrabalhoBuilder()
	{
		if (_repositorio is null)
		{
			_repositorio = new Mock<IUnidadeDeTrabalho>();
		}
	}

	public static UnidadeDeTrabalhoBuilder Instancia()
	{
		_instance = new UnidadeDeTrabalhoBuilder();
		return _instance;
	}

	public IUnidadeDeTrabalho Construir()
	{
		return _repositorio.Object;
	}
}
