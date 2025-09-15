using MySql.Data.MySqlClient;
using Evento.Core.Services;
using System.Data;

namespace Evento.Dapper
{
    public class Ado : IAdo
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