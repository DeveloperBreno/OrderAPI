using Dominio.Interfaces;
using Dominio.Interfaces.InterfaceServicos;
using Entidades.Entidades;

namespace Dominio.Servicos;

public class ServicoNoticia : IServicoNoticia
{
    private readonly INoticia _INoticia;

    public ServicoNoticia(INoticia iNoticia)
    {
        _INoticia = iNoticia;
    }

    private bool Validar(Noticia noticia)
    {
        var validarTitulo = noticia.ValidarPropriedadesString(noticia.Titulo, "Titulo");
        var validarInformacao = noticia.ValidarPropriedadesString(noticia.Informacao, "Informacao");

        if (validarTitulo && validarInformacao)
        {
            return true;
        }
        return false;
    }

    public async Task AdicionarNoticia(Noticia noticia)
    {
        if (Validar(noticia))
        {
            noticia.CriadoPor = "mudar!";
            noticia.CriadoEm = DateTime.Now;
            noticia.Ativo = true;
            await _INoticia.Adicionar(noticia);
        }
    }

    public async Task AtualizaNoticia(Noticia noticia)
    {
        if (Validar(noticia))
        {
            noticia.AlteradoPor = "mudar!";
            noticia.AlteradoEm = DateTime.Now;
            await _INoticia.Atualizar(noticia);
        }
    }


    public async Task<List<Noticia>> ListarNoticiasAtivas()
    {
        return await _INoticia.ListarNoticias(n => n.Ativo);
    }
}
