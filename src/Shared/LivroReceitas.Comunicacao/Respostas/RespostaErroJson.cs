namespace LivroReceitas.Comunicacao.Respostas;

public class RespostaErroJson
{
	public List<string> Mensagens { get; set; }



	public RespostaErroJson(string mensagens)
	{
		Mensagens = new List<string>
		{
			mensagens
		};
	}
	public RespostaErroJson(List<string> mensagens)
	{
		Mensagens = mensagens;
	}
}
