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

        public Task<string> CancelarEvento(int id)
        {
            throw new NotImplementedException();
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

        public Task<string> PublicarEvento(int id)
        {
            throw new NotImplementedException();
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