namespace LivroReceitas.Comunicacao.Respostas;

public class RespostaErro
{
	public List<string> Mensagens { get; set; }



	public RespostaErro(string mensagens)
	{
		Mensagens = new List<string>
		{
			mensagens
		};
	}
	public RespostaErro(List<string> mensagens)
	{
		Mensagens = mensagens;
	}
}
