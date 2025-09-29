using Evento.Core.Entidades;
namespace Evento.Core.Services.Repo
{
    public interface IRepoUsuario
    {
        Task<IEnumerable<Usuario>> ObtenerTodos();
        Task<Usuario?> ObtenerPorId(int id);
        Task<int> InsertUsuario(Usuario usuario);
        Task<bool> UpdateUsuario(Usuario usuario);
        Task<bool> DeleteUsuario(int id);
        Task<IEnumerable<OrdenesCompra>> ObtenerComprasPorUsuario(int id);
        Task<Usuario?> ObtenerPorEmail(string nuevoEmail);
        Task<Usuario?> Login(string nuevoEmail, string nuevaContrasena);
        Task<bool> ExisteUsuarioPorEmail(string nuevoEmail);
    }
}