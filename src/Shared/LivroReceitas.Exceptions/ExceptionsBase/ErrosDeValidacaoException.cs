namespace LivroReceitas.Exceptions.ExceptionsBase;

public class ErrosDeValidacaoException : LivroReceitasExceptions
{
	public List<string> MensagensDeErro { get; set; }

	public ErrosDeValidacaoException(List<string> mensagensDeErro) :base(String.Empty)
	{
		MensagensDeErro = mensagensDeErro;
	}
}
