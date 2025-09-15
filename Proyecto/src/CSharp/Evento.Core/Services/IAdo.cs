
using System.Data;

namespace Evento.Core.Services
{
    public interface IAdo
    {
        IDbConnection GetDbConnection();
    }
}