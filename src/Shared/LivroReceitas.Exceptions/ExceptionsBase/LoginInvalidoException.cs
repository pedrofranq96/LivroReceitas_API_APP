namespace LivroReceitas.Exceptions.ExceptionsBase;

public class LoginInvalidoException : LivroReceitasExceptions
{

	public LoginInvalidoException() : base(ResourceMensagensDeErro.LOGIN_INVALIDO)
	{

	}
}
