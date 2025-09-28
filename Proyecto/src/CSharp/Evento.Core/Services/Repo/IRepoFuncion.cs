using Evento.Core.Entidades;

namespace Evento.Core.Services.Repo
{
    public interface IRepoFuncion
    {
        Task<IEnumerable<Funcion>> ObtenerTodos();
        Task<Funcion?> ObtenerPorId(int id);
        Task<int> InsertFuncion(Funcion funcion);
        Task<bool> UpdateFuncion(Funcion funcion);
        Task<bool> DeleteFuncion(int id);
        Task<IEnumerable<Tarifa>> ObtenerTarifasDeFuncion(int id);
        Task<string> CancelarFuncion(int id);
    }
}