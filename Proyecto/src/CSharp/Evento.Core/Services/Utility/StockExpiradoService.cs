using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Evento.Core.Services.Repo;
namespace Evento.Core.Services.Utility;

public class StockExpiradoService : BackgroundService
{
    private readonly IRepoOrdenCompra _repoOrden;
    private readonly ILogger<StockExpiradoService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(1); // Intervalo de ejecución

    public StockExpiradoService(IRepoOrdenCompra repoOrden, ILogger<StockExpiradoService> logger)
    {
        _repoOrden = repoOrden;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Servicio de liberación de stock expirado iniciado.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                int cantidadLiberada = await _repoOrden.LiberarStockExpirado();
                if (cantidadLiberada > 0)
                {
                    _logger.LogInformation($"Se liberaron {cantidadLiberada} reservas expiradas.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al liberar stock expirado.");
            }

            await Task.Delay(_interval, stoppingToken);
        }

        _logger.LogInformation("Servicio de liberación de stock expirado detenido.");
    }
}
