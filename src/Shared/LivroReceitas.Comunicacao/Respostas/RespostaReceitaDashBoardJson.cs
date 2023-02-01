namespace LivroReceitas.Comunicacao.Respostas;
public class RespostaReceitaDashBoardJson
{
	public string Id { get; set; }
	public string Titulo { get; set; }
	public int QuantidadeIngredientes { get; set; }

	public int TempoPreparo { get; set; }
}
