using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivroReceitas.Domain.Repositorio.Conexao;
public interface IConexaoReadOnlyRepositorio
{
	Task<bool> ExisteConexao(long idUsuarioA, long idUsuarioB);
}
