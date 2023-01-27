using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivroReceitas.Domain.Repositorio.Codigos;
public interface ICodigoWriteOnlyRepositorio
{
	Task Registrar(Entidades.Codigos codigo);
}
