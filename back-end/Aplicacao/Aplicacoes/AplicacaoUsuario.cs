using Aplicacao.Interfaces;
using Dominio.Interfaces;

namespace Aplicacao.Aplicacoes;

public class AplicacaoUsuario : IAplicacaoUsuario
{
    IUsuario _IUsuario;

    public AplicacaoUsuario(IUsuario IUsuario)
    {
        _IUsuario = IUsuario;
    }

    public async Task<bool> AdicionarUsuario(string email, string senha, DateTime nascimento, string celular, string userName)
    {
        return await _IUsuario.AdicionarUsuario(email, senha, nascimento, celular, userName);
    }

    public async Task<bool> ExisteUsuario(string email, string senha)
    {
        return await _IUsuario.ExisteUsuario(email, senha);
    }

    public async Task<bool> RemoverUsuarioPorId(string id)
    {
        return await _IUsuario.RemoverUsuarioPorId(id);
    }

    public async Task<string> RetornaIdUsuario(string email)
    {
        return await _IUsuario.RetornaIdUsuario(email);
    }

    public async Task<string> RetornaONomeDoUsuarioPorId(string idUsuario)
    {
        return await _IUsuario.RetornaONomeDoUsuarioPorId(idUsuario);
    }
}
