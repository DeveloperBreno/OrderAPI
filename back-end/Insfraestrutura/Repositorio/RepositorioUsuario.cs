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

    public async Task<bool> AdicionarUsuario(string email, string senha, DateTime nascimento, string celular, string userName)
    {

        try
        {
            var usuario = new ApplicationUser()
            {
                Celular = celular,
                PasswordHash = senha,
                DataDeNascimento = nascimento,
                Email = email,
                NormalizedEmail = email.ToUpper(),
                UserName = userName,
                NormalizedUserName = userName.ToUpper()
            };

            await _context.AddAsync(usuario);
            await _context.SaveChangesAsync();

        }
        catch (DbUpdateException ex)
        {

            // Lidar com outras falhas de forma apropriada
            throw ex;
        }

        return true;

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
