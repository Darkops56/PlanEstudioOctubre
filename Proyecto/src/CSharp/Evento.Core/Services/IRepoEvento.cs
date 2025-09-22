using Evento.Core.Entidades;

namespace Evento.Core.Services
{
    public interface IRepoEvento
    {
        Task<IEnumerable<Eventos>> ObtenerTodos();
        Task<Eventos?> ObtenerEventoPorId(int id);
        Task<TipoEvento?> ObtenerTipoEventoPorId(int id);
        Task<Eventos?> ObtenerEventoPorNombre(string nombre);
        Task<int> InsertEvento(Eventos evento);
        Task<bool> UpdateEvento(Eventos evento);
        Task<bool> DeleteEvento(int id);
        Task<IEnumerable<Funcion>> ObtenerFuncionesPorEventoAsync(int idEvento);
        Task<IEnumerable<Sector>> ObtenerSectoresConTarifaAsync(int idEvento);
    }
}