using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evento.Core.Entidades;

namespace Evento.Core.Services.Repo
{
    public interface IRepoRefreshToken
    {
        Task<int> InsertToken(RefreshToken token);
        Task<RefreshToken?> ObtenerToken(string token);
        Task DeleteToken(string token);
        Task DeleteTokensPorEmail(string email);
    }
}