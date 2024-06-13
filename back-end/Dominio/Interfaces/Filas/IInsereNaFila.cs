
namespace Dominio.Interfaces.Filas;


public interface IInsereNaFila
{
    void Inserir(object obj, string nomeDaFila);

    // aqui podemos definir varios tipos de "creates" para cada fluxo
}
