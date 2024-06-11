namespace Aplicacao.Interfaces;

public interface IAplicacaoUsuario
{
    Task<bool> AdicionarUsuario(string email, string senha, DateTime nascimento, string celular);

    Task<bool> ExisteUsuario(string email, string senha);

    Task<string> RetornaIdUsuario(string email);
}
