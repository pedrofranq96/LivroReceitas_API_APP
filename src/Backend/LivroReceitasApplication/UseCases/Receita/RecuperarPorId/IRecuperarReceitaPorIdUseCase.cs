using LivroReceitas.Comunicacao.Respostas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivroReceitas.Application.UseCases.Receita.RecuperarPorId;
public interface IRecuperarReceitaPorIdUseCase
{
	Task<RespostaReceitaJson> Executar(long id);
}
