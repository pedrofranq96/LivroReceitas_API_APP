using System.Runtime.Serialization;

namespace LivroReceitas.Exceptions.ExceptionsBase;


[Serializable]
public class ErrosDeValidacaoException : LivroReceitasException
{
	public List<string> MensagensDeErro { get; set; }

	public ErrosDeValidacaoException(List<string> mensagensDeErro) :base(String.Empty)
	{
		MensagensDeErro = mensagensDeErro;
	}
	protected ErrosDeValidacaoException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
