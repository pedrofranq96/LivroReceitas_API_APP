using System.Runtime.Serialization;

namespace LivroReceitas.Exceptions.ExceptionsBase;

[Serializable]
public class LivroReceitasExceptions: SystemException
{
	public LivroReceitasExceptions(string mensagem): base(mensagem)
	{

	}
	protected LivroReceitasExceptions(SerializationInfo info, StreamingContext context): base(info,context) { }
}
