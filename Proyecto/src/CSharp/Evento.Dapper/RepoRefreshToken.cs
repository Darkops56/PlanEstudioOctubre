using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Evento.Core.Entidades;
using Evento.Core.Services.Repo;
using Mysqlx.Resultset;
using MySqlX.XDevAPI.Common;

namespace Evento.Dapper
{
    public class RepoRefreshToken : IRepoRefreshToken
    {
        private readonly IAdo _ado;

        public RepoRefreshToken(IAdo ado) => _ado = ado;

        public async Task<int> InsertToken(RefreshToken token)
        {
            var db = _ado.GetDbConnection();
            var sql = "INSERT INTO RefreshTokens (Token, Email, Expiration) VALUES (@Token, @Email, @Expiration)";
            var rows = await db.ExecuteAsync(sql, token);

            return rows > 0 ? rows : 0;
        }

        public async Task<RefreshToken?> ObtenerToken(string token)
        {
            var db = _ado.GetDbConnection();
            var sql = "SELECT * FROM RefreshTokens WHERE Token = @Token";
            return await db.QueryFirstOrDefaultAsync<RefreshToken>(sql, new { Token = token });
        }

        public async Task DeleteToken(string token)
        {
            var db = _ado.GetDbConnection();
            var sql = "DELETE FROM RefreshTokens WHERE Token = @Token";
            await db.ExecuteAsync(sql, new { Token = token });
        }

        public async Task DeleteTokensPorEmail(string email)
        {
            var db = _ado.GetDbConnection();
            var sql = "DELETE FROM RefreshTokens WHERE Email = @Email";
            await db.ExecuteAsync(sql, new { Email = email });
        }
    }
}