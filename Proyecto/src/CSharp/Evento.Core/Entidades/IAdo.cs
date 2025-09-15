
using System.Data;

namespace Evento.Core.Entidades
{
    public interface IAdo
    {
        IDbConnection GetConnection();
    }
}