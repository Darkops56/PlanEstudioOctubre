using Evento.Core.Entidades;
using Evento.Core.Services.Enums;

namespace Evento.Core.Services.Repo
{
    public interface IRepoTarifa
    {
        Task<IEnumerable<Tarifa>> ObtenerTodos();
        Task<Tarifa?> ObtenerPorId(int id);
        Task<int> InsertTarifa(Tarifa tarifa);
        Task<bool> UpdateTarifa(Tarifa tarifa);
        Task<bool> ReducirStock(int id);
        Task<ETipoTarifa> ObtenerTipoTarifa(string tipo);
    }
}