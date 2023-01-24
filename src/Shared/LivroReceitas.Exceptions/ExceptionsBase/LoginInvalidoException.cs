using System.Runtime.Serialization;

namespace LivroReceitas.Exceptions.ExceptionsBase;


[Serializable]
public class LoginInvalidoException : LivroReceitasExceptions
{

	public LoginInvalidoException() : base(ResourceMensagensDeErro.LOGIN_INVALIDO)
	{

	}
	protected LoginInvalidoException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
