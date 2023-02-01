using LivroReceitas.Comunicacao.Requesicoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivroReceitas.Application.UseCases.Receita.Atualizar;
public interface IReceitaAtualizarUseCase
{
	Task Executar(long id, RequisicaoReceitaJson requisicao);
}
