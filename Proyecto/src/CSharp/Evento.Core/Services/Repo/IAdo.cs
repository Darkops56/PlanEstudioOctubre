
using System.Data;

namespace Evento.Core.Services.Repo
{
    public interface IAdo
    {
        IDbConnection GetDbConnection();
    }
}