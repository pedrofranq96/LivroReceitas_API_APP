using LivroReceitas.Comunicacao.Respostas;

namespace LivroReceitas.Application.UseCases.Conexao.QRCodeLido;
public interface IQRCodeLidoUseCase
{
	Task<(RespostaUsuarioConexaoJson usuarioParaSeConectar, string idUsuarioGerouQRCode)> Executar(string codigoConexao);
}
