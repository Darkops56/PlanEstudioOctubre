using Evento.Core.Entidades;

namespace Evento.Core.Services
{
    public interface IRepoEntrada
    {
        Task<IEnumerable<Entrada>> ObtenerTodos();
        Task<int> InsertEntrada(Entrada entrada);
        Task<bool> DeleteEntrada(int id);
        Task<Entrada?> ObtenerEntrada(int id);
        Task<string> AnularEntrada(int id);
    }
}