using Aplicacao.Interfaces.Genericos;
using Entidades.Entidades;
namespace Aplicacao.Interfaces;

public interface IAplicacaoNoticia : IGenericaAplicacoes<Noticia>
{

    Task AdicionarNoticia(Noticia noticia);
    Task AtualizaNoticia(Noticia noticia);
    Task<List<Noticia>> ListarNoticiasAtivas();
}
