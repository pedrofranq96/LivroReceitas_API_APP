using System.Runtime.Serialization;

namespace LivroReceitas.Exceptions.ExceptionsBase;

[Serializable]
public class LivroReceitasException: SystemException
{
	public LivroReceitasException(string mensagem): base(mensagem)
	{

	}
	protected LivroReceitasException(SerializationInfo info, StreamingContext context): base(info,context) { }
}
