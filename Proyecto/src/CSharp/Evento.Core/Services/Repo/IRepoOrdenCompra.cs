using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evento.Core.Entidades;

namespace Evento.Core.Services.Repo
{
    public interface IRepoOrdenCompra
    {
        Task<int> InsertOrdenCompra(OrdenesCompra ordenesCompra);
        Task<OrdenesCompra?> ObtenerOrdenCompra(int id);
        Task<IEnumerable<OrdenesCompra>> ObtenerOrdenesCompra();
        Task<string> PagarOrdenCompra(int id);
        Task<string> CancelarOrdenCompra(int id);
        Task<int> LiberarStockExpirado();
    }
}