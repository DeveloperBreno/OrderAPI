using Entidades.Entidades;

namespace Dominio.Interfaces.InterfaceServicos;

public interface IServicoNoticia
{
    Task AdicionarNoticia(Noticia noticia);
    Task AtualizaNoticia(Noticia noticia);
    Task<List<Noticia>> ListarNoticiasAtivas();
}
