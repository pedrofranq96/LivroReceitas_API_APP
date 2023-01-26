using LivroReceitas.Comunicacao.Enum;

namespace LivroReceitas.Comunicacao.Requesicoes;
public class RequisicaoDashBoardJson
{
	public string TituloIngrediente { get; set; }
	public Categoria? Categoria { get; set; }
}
