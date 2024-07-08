using Entidades.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection.Emit;

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
            optionsBuilder.UseNpgsql(ObterStringConexao());
            base.OnConfiguring(optionsBuilder);
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ApplicationUser>().ToTable("AspNetUsers").HasKey(t => t.Id);

        base.OnModelCreating(builder);

        var dateTimeConverter = new DateTimeToUtcConverter();

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(dateTimeConverter);
                }
            }
        }

    }

    public string ObterStringConexao()
    {
        return "Host=200.6.48.21;Port=5432;Database=MyDatabase;Username=postgres;Password=DcpmQP3pFjNFrRJFNDwTRNwdAIKlvlHMCNrsC67Ijzc;";
    }

}

public class DateTimeToUtcConverter : ValueConverter<DateTime, DateTime>
{
    public DateTimeToUtcConverter()
        : base(v => v.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc)) { }
}
