using MySql.Data.MySqlClient;
using Evento.Core.Entidades;
using System.Data;

namespace Evento.Dapper
{
    public class Ado
    {
        private readonly string _connection;
        public Ado(string connection)
        {
            _connection = connection;
        }

        public IDbConnection GetDbConnection()
        {
            return new MySqlConnection(_connection);
        }
    }
}