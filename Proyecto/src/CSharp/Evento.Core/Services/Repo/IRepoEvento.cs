using Evento.Core.DTOs;
using Evento.Core.Entidades;

namespace Evento.Core.Services.Repo
{
    public interface IRepoEvento
    {
        Task<IEnumerable<Eventos>> ObtenerTodos();
        Task<Eventos?> ObtenerEventoPorId(int id);
        Task<TipoEventoDto?> ObtenerTipoEventoPorNombre(string tipo);
        Task<Eventos?> ObtenerEventoPorNombre(string nombre);
        Task<int> InsertEvento(Eventos evento);
        Task<bool> UpdateEvento(Eventos evento);
        Task<bool> DeleteEvento(int id);
        Task<IEnumerable<Funcion>> ObtenerFuncionesPorEventoAsync(int idEvento);
        Task<string> PublicarEvento(int id);
        Task<string> CancelarEvento(int id);
    }
}