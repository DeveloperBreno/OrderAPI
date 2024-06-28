namespace Aplicacao.Interfaces;

public interface IAplicacaoUsuario
{
    Task<bool> AdicionarUsuario(string email, string senha, DateTime nascimento, string celular, string userName);

    Task<bool> ExisteUsuario(string email, string senha);
    Task<bool> RemoverUsuarioPorId(string id);
    Task<string> RetornaIdUsuario(string email);
    Task<string> RetornaONomeDoUsuarioPorId(string idUsuario);
}
