using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using Entidades.Enums;
using System.ComponentModel.DataAnnotations;

namespace Entidades.Entidades;
public class ApplicationUser : IdentityUser
{
    [Column("USR_NASCIMENTO")]
    public DateTime DataDeNascimento { get; set; }

    [Required]
    [Column("USR_CELULAR")]
    public required string Celular { get; set; }

    [Column("USR_TIPO")]
    public TipoUsuario Tipo { get; set; }

    // Propriedade de navegação para Noticia
    public ICollection<Noticia> Noticias { get; set; }
}
