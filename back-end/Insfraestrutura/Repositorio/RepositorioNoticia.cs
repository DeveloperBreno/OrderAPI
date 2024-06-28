using Dominio.Interfaces;
using Entidades.Entidades;
using Insfraestrutura.Configuracoes;
using Insfraestrutura.Repositorio.Genericos;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Insfraestrutura.Repositorio;

public class RepositorioNoticia : RepositorioGenerico<Noticia>, INoticia
{
    private readonly Contexto _context;

    public RepositorioNoticia()
    {
        _context = new Contexto(new DbContextOptionsBuilder<Contexto>().Options);
    }

    public async Task<bool> ExcluirNoticiasPorUsuarioId(string id)
    {
        var noticias = await _context.Noticia.Where(o => o.UsuarioId == id).ToListAsync();

        foreach (var noticia in noticias)
        {
            _context.Noticia.Remove(noticia);
        }

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<Noticia>> ListarNoticias(Expression<Func<Noticia, bool>> exNoticia)
    {
        return await _context.Noticia.Where(exNoticia).ToListAsync();
    }
}
