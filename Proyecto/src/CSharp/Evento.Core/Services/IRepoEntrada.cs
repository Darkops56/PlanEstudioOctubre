using Evento.Core.Entidades;

namespace Evento.Core.Services
{
    public interface IRepoEntrada
    {
        Task<IEnumerable<Entrada>> ObtenerTodos();
        Task<Entrada?> ObtenerEntrada(int id);
        Task<int> InsertEntrada(Entrada entrada);
        Task<bool> UpdateEntrada(Entrada entrada);
        Task<bool> DeleteEntrada(int id);

        Task<Entrada?> ObtenerEntradaConQR(int idEntrada);
        Task<bool> ValidarQR(string qrCodigo);
    }
}