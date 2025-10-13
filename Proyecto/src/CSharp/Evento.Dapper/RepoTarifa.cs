using Dapper;
using Evento.Core.Entidades;
using Evento.Core.Services.Enums;
using Evento.Core.Services.Repo;
using Evento.Core.Services.Utility;
using Mysqlx.Resultset;

namespace Evento.Dapper
{
    public class RepoTarifa : IRepoTarifa
    {
        private readonly IAdo _ado;
        public RepoTarifa(IAdo ado) => _ado = ado;
        public async Task<int> InsertTarifa(Tarifa tarifa)
        {
            using var db = _ado.GetDbConnection();
            var query = "INSERT INTO Tarifa (Tipo, idFuncion, Precio, Stock, Estado) VALUES(@tipo, @idfuncion, @precio, @stock, @estado)";

            return await db.ExecuteAsync(query, new
            {
                tipo = tarifa.Tipo.ToString(),
                precio = tarifa.Precio,
                stock = tarifa.Stock,
                idfuncion = tarifa.funcion?.idFuncion,
                estado = EEstados.Creado.ToString()
            });
        }
        public async Task<Tarifa?> ObtenerPorId(int id)
        {
            using var db = _ado.GetDbConnection();
            var query = @"
                SELECT 
                    t.idTarifa,
                    t.idFuncion,
                    t.Stock,
                    t.Precio,
                    t.Estado AS Tarifa_Estado,
                    f.idFuncion AS Funcion_idFuncion,
                    f.Nombre,
                    f.Fecha,
                    f.idEvento,
                    f.Estado AS Funcion_Estado
                FROM Tarifa t
                INNER JOIN Funcion f ON t.idFuncion = f.idFuncion
                WHERE t.idTarifa = @idTarifa";

            var tarifaDiccionario = new Dictionary<int, Tarifa>();

            var resultado = await db.QueryAsync<Tarifa, Funcion, Tarifa>(
                query,
                (tarifa, funcion) =>
                {
                    if (!tarifaDiccionario.TryGetValue(tarifa.idTarifa, out var tarifaExistente))
                    {
                        tarifaExistente = tarifa;
                        tarifaExistente.funcion = funcion;
                        tarifaDiccionario.Add(tarifa.idTarifa, tarifaExistente);
                    }
                    return tarifaExistente;
                },
                new { idTarifa = id},
                splitOn: "Funcion_idFuncion"
            );

            return resultado.FirstOrDefault();
        }
        public async Task<IEnumerable<Tarifa>> ObtenerTodos()
        {
            using var db = _ado.GetDbConnection();
            var query = @"
                SELECT t.idTarifa, t.idFuncion AS Tarifa_Funcion, t.Tipo, t.Precio, t.Stock, t.Estado, f.idFuncion AS Funcion_Funcion, f.Nombre, f.Fecha, f.idEvento, f.Estado
                FROM Tarifa t
                INNER JOIN Funcion f ON t.idFuncion = f.idFuncion";

            var tarifaDiccionario = new Dictionary<int, Tarifa>();

            var resultado = await db.QueryAsync<Tarifa, Funcion, Tarifa>(
                query,
                (tarifa, funcion) =>
                {
                    if (!tarifaDiccionario.TryGetValue(tarifa.idTarifa, out var tarifaExistente))
                    {
                        tarifaExistente = tarifa;
                        tarifaExistente.funcion = funcion;
                        tarifaDiccionario.Add(tarifa.idTarifa, tarifaExistente);
                    }
                    return tarifaExistente;
                },
                splitOn: "Funcion_Funcion"
            );
            return resultado;
        }
        public async Task<bool> UpdateTarifa(Tarifa tarifa)
        {
            using var db = _ado.GetDbConnection();
            var query = "UPDATE Tarifa SET Tipo = @tipo, Precio = @precio, Estado = @estado WHERE idTarifa = @idtarifa";
            var rows = await db.ExecuteAsync(query, new
            {
                idtarifa = tarifa.idTarifa,
                tipo = tarifa.Tipo.ToString(),
                precio = tarifa.Precio,
                estado = tarifa.Estado.ToString()
            });
            return rows > 0;
        }
        public async Task<bool> ReducirStock(int id)
        {
            using var db = _ado.GetDbConnection();
            var query = "UPDATE Tarifa SET Stock = Stock - 1 WHERE idTarifa = @idtarifa";
            var rows = await db.ExecuteAsync(query, new
            {
                idtarifa = id
            });
            return rows > 0;
        }
        public ETipoTarifa ObtenerTipoTarifa(string tipo)
        {

            string tipoNormalizado = UniqueFormatStrings.NormalizarString(tipo);

            foreach (var nombre in Enum.GetNames(typeof(ETipoTarifa)))
            {
                if (nombre.ToLowerInvariant() == tipoNormalizado)
                    return (ETipoTarifa)Enum.Parse(typeof(ETipoTarifa), nombre);
            }
            throw new Exception($"El tipo de tarifa '{tipo}' no es válido.");
        }
    }
}