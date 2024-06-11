using Entidades.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Insfraestrutura.Configuracoes;

public class Contexto : IdentityDbContext<ApplicationUser>
{

    public Contexto(DbContextOptions<Contexto> options) : base(options)
    {

    }
    public DbSet<Noticia> Noticia { get; set; }
    public DbSet<ApplicationUser> ApplicationUser { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(ObterStringConexao());
            base.OnConfiguring(optionsBuilder);
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ApplicationUser>().ToTable("AspNetUsers").HasKey(t => t.Id);

        base.OnModelCreating(builder);
    }

    public string ObterStringConexao()
    {
        string strCon = "Data Source=192.168.197.180;Initial Catalog=API_DDD;User Id=sa;Password=boein@747; TrustServerCertificate=True; Connect Timeout=180";

        return strCon;
    }

}
