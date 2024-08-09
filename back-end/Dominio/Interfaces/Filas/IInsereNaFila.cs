
namespace Dominio.Interfaces.Filas;


public interface IInsereNaFila
{
    void Inserir(object obj, string nomeDaFila);
    void InserirNaFilaDeErro(object obj, string nomeDaFila, Exception e);
}
