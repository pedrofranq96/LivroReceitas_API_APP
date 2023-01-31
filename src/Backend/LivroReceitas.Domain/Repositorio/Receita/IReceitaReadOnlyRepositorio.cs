﻿namespace LivroReceitas.Domain.Repositorio.Receita;
public interface IReceitaReadOnlyRepositorio
{
	Task<IList<Entidades.Receita>> RecuperarTodasDoUsuario(long usuarioId);
	Task<Entidades.Receita> RecuperarPorId(long receitaId);
	Task<int> QuantidadeReceitas(long usuarioId);
}
