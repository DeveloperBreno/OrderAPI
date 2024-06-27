using Dominio.Interfaces;
using Entidades.Entidades;
using Insfraestrutura.Configuracoes;
using Insfraestrutura.Repositorio.Genericos;
using Microsoft.EntityFrameworkCore;

namespace Insfraestrutura.Repositorio;

public class RepositorioUsuario : RepositorioGenerico<ApplicationUser>, IUsuario
{
    private readonly Contexto _context;
    public RepositorioUsuario()
    {
        _context = new Contexto(new DbContextOptionsBuilder<Contexto>().Options);
    }

    public async Task<bool> AdicionarUsuario(string email, string senha, DateTime nascimento, string celular)
    {

        try
        {
            var usuario = new ApplicationUser()
            {
                Celular = celular,
                PasswordHash = senha,
                DataDeNascimento = nascimento,
                Email = email
            };

            await _context.AddAsync(usuario);
            await _context.SaveChangesAsync();

        }
        catch (DbUpdateException ex)
        {
            var innerException = ex.InnerException;
            // Verifica se é uma exceção de falha transitória específica, se necessário
            if (IsTransientFailure(innerException))
            {
                // Implemente lógica de retentativa aqui, se aplicável
                // Exemplo: aguarde um tempo e tente novamente
                await Task.Delay(TimeSpan.FromSeconds(5));
                await _context.SaveChangesAsync();
            }
            else
            {
                // Lidar com outras falhas de forma apropriada
                throw;
            }
        }

        return true;

    }

    bool IsTransientFailure(Exception ex)
    {
        // Implemente lógica para verificar se a exceção é devido a uma falha transitória
        // Exemplo: verifica se a exceção é do tipo de timeout de conexão, etc.
        return false;
    }

    public async Task<bool> ExisteUsuario(string email, string senha)
    {
        return await _context.ApplicationUser.Where(o => o.Email.Equals(email) && o.PasswordHash.Equals(senha))
            .AsNoTracking()
            .AnyAsync();
    }

    public async Task<string> RetornaIdUsuario(string email)
    {
        var user = await _context.ApplicationUser.Where(o => o.Email.Equals(email))
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return user.Id;

    }

    public async Task<string> RetornaONomeDoUsuarioPorId(string idUsuario)
    {
        var user = await _context.ApplicationUser.Where(o => o.Id.Equals(idUsuario))
          .AsNoTracking()
          .FirstAsync();
        return user?.UserName ?? "Usuário não encontrado.";
    }
}
