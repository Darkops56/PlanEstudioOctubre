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
            var sql = "INSERT INTO RefreshTokens (Token, Email, Expiration) VALUES (@toke, @email, @expiration)";
            var rows = await db.ExecuteAsync(sql, new
            {
                toke = token.Token,
                email = token.Email,
                expiration = token.Expiration
            });

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

        public async Task ReemplazarToken(int idUsuario, string nuevoHash, DateTime expiracion)
        {
            var db = _ado.GetDbConnection();
            var deleteSql = "DELETE FROM RefreshTokens WHERE idUsuario = @idusuario";
            await db.ExecuteAsync(deleteSql, new { idusuario = idUsuario });

            string queryUsuario = "SELECT * FROM Usuario WHERE idUsuario = @idusuario";
            var usuario = await db.QueryFirstOrDefaultAsync<Usuario>(queryUsuario, new {idusuario = idUsuario});

            string insertSql = "INSERT INTO RefreshTokens (idUsuario, Token, Email, Expiration) VALUES (@idusuario, @token, @email, @expiration)";
            await db.ExecuteAsync(insertSql, new
            {
                idusuario = idUsuario,
                token = nuevoHash,
                email = usuario?.Email,
                expiration = expiracion
            });
        }
    }
}