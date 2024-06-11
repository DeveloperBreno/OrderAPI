using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entidades.Notificacoes;

namespace Entidades.Entidades;

[Table("TB_NOTICIA")]
public class Noticia : Notifica
{

    [Column("Titulo")]
    public string Titulo { get; set; }

    [Column("Informacao")]
    public string Informacao { get; set; }

    [Column("Ativo")]
    public bool Ativo { get; set; }

    // Propriedade da chave estrangeira para ApplicationUser
    [Column("UsuarioId")]
    public string UsuarioId { get; set; }

    // Propriedade de navegação para ApplicationUser
    [ForeignKey("UsuarioId")]
    public ApplicationUser Usuario { get; set; }

}
