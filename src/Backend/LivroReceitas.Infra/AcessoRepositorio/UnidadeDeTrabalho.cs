﻿using LivroReceitas.Domain.Repositorio;

namespace LivroReceitas.Infra.AcessoRepositorio;

public sealed class UnidadeDeTrabalho : IDisposable, IUnidadeDeTrabalho
{
	private readonly Context _context;
	private bool _disposed;

	public UnidadeDeTrabalho(Context context)
	{
		_context = context;
	}
	public async Task Commit()
	{
		await _context.SaveChangesAsync();
	}

	public void Dispose()
	{
		Dispose(true);
	}

	private void Dispose(bool disposing)
	{
		if (!_disposed && disposing)
		{
			_context.Dispose();
		}
		_disposed = true;
	}
}
