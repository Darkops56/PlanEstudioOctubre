using MySql.Data.MySqlClient;
using Dapper;
using Evento.Core.Entidades;
using Evento.Core.Services.Repo;
using Org.BouncyCastle.Asn1;

namespace Evento.Dapper
{
    public class RepoEvento : IRepoEvento
    {
        private readonly IAdo _ado;

        public RepoEvento(IAdo ado) => _ado = ado;

        public async Task<string> CancelarEvento(int id)
        {
            using var db = _ado.GetDbConnection();

            var evento = await ObtenerEventoPorId(id);
            if (evento == null)
                throw new Exception("El evento no existe");

            if (evento.EstadoEvento == "Cancelado")
                throw new Exception("El evento ya está cancelado");

            var rows = await db.ExecuteAsync(
                "UPDATE Eventos SET Estado = 'Cancelado' WHERE idEvento = @IdEvento",
                new { IdEvento = id });

            return rows > 0 ? "Evento cancelado correctamente" : "No se pudo cancelar el evento";
        }

        public async Task<bool> DeleteEvento(int id)
        {
            var db = _ado.GetDbConnection();
            var rows = await db.ExecuteAsync("DELETE FROM Evento WHERE idEvento = @Id", new { Id = id });
            return rows > 0;
        }

        public async Task<int> InsertEvento(Eventos evento)
        {
            var db = _ado.GetDbConnection();
            var rows = await db.ExecuteAsync("INSERT INTO Evento(idEvento, Nombre, tipoEvento, fechaInicio, fechaFin) VALUES(@idevento, @nombre, @tipoevento, @fechainicio, @fechafin)", new
            {
                idevento = evento.idEvento,
                nombre = evento.Nombre,
                tipoevento = evento.tipoEvento.tipoEvento,
                fechainicio = evento.fechaInicio,
                fechafin = evento.fechaFin
            });
            return rows > 0 ? rows : 0;
        }

        public async Task<Eventos?> ObtenerEventoPorId(int id)
        {
            var db = _ado.GetDbConnection();
            return await db.QueryFirstAsync<Eventos?>("SELECT * FROM Evento WHERE idEvento = @idevento", new { idevento = id });
        }

        public async Task<Eventos?> ObtenerEventoPorNombre(string nombre)
        {
            var db = _ado.GetDbConnection();
            var query = "SELECT * FROM Eventos WHERE Nombre = @name";

            return await db.QueryFirstAsync<Eventos>(query, new{ name = nombre});
        }

        public async Task<IEnumerable<Funcion>> ObtenerFuncionesPorEventoAsync(int idEvento)
        {
            var db = _ado.GetDbConnection();
            return await db.QueryAsync<Funcion>("SELECT * FROM Funcion WHERE idEvento = @idevento", new { idevento = idEvento });
        }

        public async Task<IEnumerable<Sector>> ObtenerSectoresConTarifaAsync(int idEvento)
        {
            var db = _ado.GetDbConnection();
            string query = "SELECT * FROM Sector JOIN Tarifa USING (@idevento) WHERE idEvento = @idevento";
            return await db.QueryAsync<Sector>(query, new{ idevento = idEvento});
        }

        public async Task<TipoEvento?> ObtenerTipoEventoPorId(int id)
        {
            var db = _ado.GetDbConnection();
            var query = "SELECT * FROM TipoEvento WHERE idTipoEvento = @idtipoevento";

            return await db.QueryFirstAsync<TipoEvento>(query, new { idtipoevento = id });
        }

        public async Task<IEnumerable<Eventos>> ObtenerTodos()
        {
            var db = _ado.GetDbConnection();
            return await db.QueryAsync<Eventos>("SELECT * FROM Eventos");
        }

        public async Task<string> PublicarEvento(int id)
        {
            using var db = _ado.GetDbConnection();

            var evento = await ObtenerEventoPorId(id);
            if (evento == null)
                throw new Exception("El evento no existe");

            if (evento.EstadoEvento == "Publicado")
                throw new Exception("El evento ya está publicado");
                
            string query = "SELECT * FROM Funcion f JOIN Tarifa t USING (idFuncion) WHERE f.idEvento = @idevento AND t.Stock > 0 LIMIT 1";
            var respuesta = await db.ExecuteAsync(query, new
            {
                idevento = id
            });
            if (respuesta < 0)
                throw new Exception("No se puede publicar el Evento por falta de Stock");

            var rows = await db.ExecuteAsync(
                "UPDATE Eventos SET Estado = 'Publicado' WHERE idEvento = @IdEvento",
                new { IdEvento = id });

            return rows > 0 ? "Evento publicado correctamente" : "No se pudo publicar el evento";
        }

        public async Task<bool> UpdateEvento(Eventos evento)
        {
            var db = _ado.GetDbConnection();
            var query = "UPDATE Eventos SET Nombre = @nombre, tipoEvento = @tipoevento, fechaInicio = @fechainicio, fechaFin = @fechafin WHERE idEvento = @idevento";
            var rows = await db.ExecuteAsync(query, new
            {
                idevento = evento.idEvento,
                nombre = evento.Nombre,
                tipoevento = evento.tipoEvento.tipoEvento,
                fechainicio = evento.fechaInicio,
                fechafin = evento.fechaFin
            });
            if (rows == 0)
            {
                return false;
            }
            return true;
        }
    }
}