using Evento.Core.Entidades;

namespace Evento.Core.Services
{
    public interface IRepoLocal
    {
        Task<IEnumerable<Local>> ObtenerTodos();
        Task<Local?> ObtenerPorId(int id);
        Task<Sector?> ObtenerSectorPorId(int id);
        Task<int> InsertLocal(Local local);
        Task<bool> UpdateLocal(Local local);
        Task<bool> DeleteLocal(int id);
        Task<IEnumerable<Sector>> ObtenerSectoresDelLocal(int id);
        Task<int> InsertSector(Sector sector, int id);
        Task<bool> UpdateSector(Sector sector, int id);
        Task<bool> DeleteSector(int id);
    }
}