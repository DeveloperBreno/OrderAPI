using Entidades.Entidades;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entidades.Notificacoes;

public class Notifica : Entity
{
    [NotMapped]
    public string NomePropriedade { get; set; }

    [NotMapped]
    public string Mensagem { get; set; }

    [NotMapped]
    public List<Notifica> Notificacoes { get; set; }

    public Notifica()
    {
        Notificacoes = [];
    }

    public bool ValidarPropriedadesString(string valor, string nomePropiedade)
    {
        if (string.IsNullOrWhiteSpace(valor) || string.IsNullOrWhiteSpace(nomePropiedade))
        {
            Notificacoes.Add(new Notifica { NomePropriedade = nomePropiedade, Mensagem = "Campo obrigatório" });

            return false;
        }

        return true;
    }




}
