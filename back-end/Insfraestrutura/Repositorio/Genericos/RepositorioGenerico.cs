using Dominio.Interfaces.Genericos;
using Insfraestrutura.Configuracoes;
using Microsoft.EntityFrameworkCore;

namespace Insfraestrutura.Repositorio.Genericos
{
    public class RepositorioGenerico<T> : IGenericos<T>, IDisposable where T : class
    {
        private bool disposed = false;
        private readonly Contexto _context;

        public RepositorioGenerico()
        {
            _context = new Contexto(new DbContextOptionsBuilder<Contexto>().Options);
        }

        public async Task Adicionar(T obj)
        {
            await _context.Set<T>().AddAsync(obj);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(T obj)
        {
            _context.Set<T>().Update(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<T> BuscarPorId(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task Excluir(T obj)
        {
            _context.Set<T>().Remove(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<List<T>> Listar()
        {
            return await _context.Set<T>().ToListAsync();
        }

        // Implementação do padrão de descarte
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose de recursos gerenciados
                    if (_context != null)
                    {
                        _context.Dispose();
                    }
                }

                // Liberar quaisquer recursos não gerenciados aqui

                disposed = true;
            }
        }

        public async Task<List<T>> Listar(T obj)
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        // Destrutor
        ~RepositorioGenerico()
        {
            Dispose(false);
        }
    }
}
