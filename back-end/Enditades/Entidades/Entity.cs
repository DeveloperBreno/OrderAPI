using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entidades.Enums;

namespace Entidades.Entidades;

public abstract class Entity 
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column("CriadoEm")]
    public DateTime CriadoEm { get; set; }

    [Required]
    [Column("CriadoPor")]
    public string CriadoPor { get; set; }


    [Column("AlteradoEm")]
    public DateTime? AlteradoEm { get; set; }

    [Column("AlteradoPor")]
    public string? AlteradoPor { get; set; }


    [Column("ExcluidoEm")]
    public DateTime? ExcluidoEm { get; set; }
    
    [Column("ExcluidoPor")]
    public string? ExcluidoPor { get; set; }
}
