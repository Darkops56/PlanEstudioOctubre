using Evento.Core.Entidades;

namespace Evento.Core.Services
{
    public interface IRepoCliente
    {
        Task<IEnumerable<Cliente>> ObtenerTodos();
        Task<Cliente?> ObtenerPorId(int id);
        Task<int> InsertCliente(Cliente cliente);
        Task<bool> UpdateCliente(Cliente cliente);
        Task<bool> DeleteCliente(int id);

        Task<IEnumerable<RegistroCompra>> ObtenerComprasPorCliente(int id);
        Task<IEnumerable<Entrada>> ObtenerEntradasPorCliente(int id);
        Task<bool> ExistePorDNI(int dni);
    }
}