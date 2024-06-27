﻿using Entidades.Entidades;
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
            optionsBuilder.UseNpgsql(ObterStringConexao());
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
        return "Host=localhost;Port=5432;Database=MyDatabase;Username=postgres;Password=SuaSenha";
    }

}
