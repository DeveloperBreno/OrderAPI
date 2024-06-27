using System.ComponentModel.DataAnnotations;

namespace Dominio.Interfaces;

public interface IUsuario
{
    Task<bool> AdicionarUsuario(string email, string senha, DateTime nascimento, string celular, string userName);

    Task<bool> ExisteUsuario(string email, string senha);

    Task<string> RetornaIdUsuario(string email);
    Task<string> RetornaONomeDoUsuarioPorId(string idUsuario);
}
