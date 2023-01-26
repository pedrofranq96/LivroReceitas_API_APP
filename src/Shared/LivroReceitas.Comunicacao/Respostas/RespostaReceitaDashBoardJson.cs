using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivroReceitas.Comunicacao.Respostas;
public class RespostaReceitaDashBoardJson
{
	public string Id { get; set; }
	public string Titulo { get; set; }
	public int QuantidadeIngredientes { get; set; }
}
