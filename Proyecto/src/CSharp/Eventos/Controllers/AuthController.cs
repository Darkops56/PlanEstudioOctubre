using Evento.Core.Entidades;
using Evento.Core.DTOs;
using Evento.Core.Services.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Evento.Dapper;
using Evento.Core.Services.Security;
using Evento.Core.Services.Enums;
using Evento.Core.Services.Utility;

namespace Evento.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IRepoUsuario _repoUsuario;
        private readonly IRepoCliente _repoCliente;
        private readonly IConfiguration _config;
        private readonly IRepoRefreshToken _repoRefreshToken;
        public AuthController(IRepoUsuario repoUsuario, IRepoCliente repoCliente, IConfiguration config, IRepoRefreshToken repoRefreshToken)
        {
            _repoUsuario = repoUsuario;
            _repoCliente = repoCliente;
            _config = config;
            _repoRefreshToken = repoRefreshToken;
        }

        #region Register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto nuevoUsuarioDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _repoUsuario.ExisteUsuarioPorEmail(nuevoUsuarioDto.Email))
                return BadRequest("Ya existe un usuario con este email.");

            var cliente = await _repoCliente.ObtenerPorId(nuevoUsuarioDto.DNI);
            if (cliente == null)
                return NotFound("El cliente no se encontró.");

            var hash = ContrasenaHasher.Hash(nuevoUsuarioDto.Contrasena);
            nuevoUsuarioDto.Contrasena = hash;

            var usuario = new Usuario
            {
                Apodo = nuevoUsuarioDto.Apodo,
                Email = nuevoUsuarioDto.Email,
                Contrasena = nuevoUsuarioDto.Contrasena,
                Role = ERoles.Usuario,
                cliente = new Cliente
                {
                    nombreCompleto = cliente.nombreCompleto,
                    DNI = cliente.DNI,
                    Telefono = cliente.Telefono
                }
            };

            await _repoUsuario.InsertUsuario(usuario);

            return Ok(new
            {
                mensaje = "Usuario creado con éxito",
                usuario = new
                {
                    usuario.idUsuario,
                    usuario.Apodo,
                    usuario.Email,
                    usuario.Role,
                    Cliente = usuario.cliente
                }
            });
        }
        #endregion

        #region Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // Obtener usuario por email
            var usuario = await _repoUsuario.ObtenerPorEmail(login.Email);
                if (usuario is null)
                    return Unauthorized("Email o contraseña incorrectos.");

                // Verificar contraseña usando hash seguro
                if (!ContrasenaHasher.Verificar(login.Contrasena, usuario.Contrasena))
                    return Unauthorized("Email o contraseña incorrectos.");
            // Generar JWT
            var token = GenerateJwtToken(usuario);

            // Crear refresh token
            var refreshToken = Guid.NewGuid().ToString();
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                Email = usuario.Email,
                Expiration = DateTime.UtcNow.AddMinutes(30)
            };
            await _repoRefreshToken.InsertToken(refreshTokenEntity);

            return Ok(new { token, refreshToken });
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.Email),
                new Claim(ClaimTypes.Role, string.IsNullOrEmpty(usuario.Role.ToString()) ? "Usuario" : usuario.Role.ToString()),
                new Claim("Apodo", usuario.Apodo),
                new Claim("DNI", usuario.cliente.DNI.ToString()),
                new Claim("NombreCompleto", usuario.cliente.nombreCompleto),
                new Claim("Telefono", usuario.cliente.Telefono ?? "")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion

        #region Refresh Token
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto refreshRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingToken = await _repoRefreshToken.ObtenerToken(refreshRequest.RefreshToken);
            if (existingToken == null || existingToken.Expiration < DateTime.UtcNow)
                return Unauthorized("Refresh token inválido o expirado");

            var usuario = await _repoUsuario.ObtenerPorEmail(existingToken.Email);
            if (usuario == null)
                return Unauthorized("Usuario no encontrado");

            // Generar nuevos tokens
            var newToken = GenerateJwtToken(usuario);
            var newRefreshToken = Guid.NewGuid().ToString();
            var newRefreshTokenHash = ContrasenaHasher.Hash(newRefreshToken);

            // Reemplazar token
            await _repoRefreshToken.ReemplazarToken(usuario.idUsuario, newRefreshTokenHash, DateTime.UtcNow.AddMinutes(30));

            return Ok(new { token = newToken, refreshToken = newRefreshToken });
        }
        #endregion

        #region Logout
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenDto refreshTokenDto)
        {
            await _repoRefreshToken.DeleteToken(refreshTokenDto.RefreshToken);
            return Ok(new { mensaje = "Logout exitoso" });
        }
        #endregion

        #region Me
        [HttpGet("me")]
        [Authorize ]
        public IActionResult Me()
        {
            var email = User.Identity?.Name;
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;
            var apodo = User.FindFirst("Apodo")?.Value;
            var dni = User.FindFirst("DNI")?.Value;
            var nombre = User.FindFirst("NombreCompleto")?.Value;

            return Ok(new
            {
                Email = email,
                Rol = rol,
                Apodo = apodo,
                DNI = dni,
                Nombre = nombre
            });
        }
        #endregion

        #region Roles
        [HttpGet("roles")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetRoles()
        {
            var roles = new[] { ERoles.Admin, ERoles.Usuario };
            return Ok(roles);
        }

        [HttpPost("/api/usuarios/{usuarioId}/roles")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AsignarRol(int usuarioId, [FromBody] string rol)
        {
            var usuario = await _repoUsuario.ObtenerPorId(usuarioId);
            if (usuario == null)
                return NotFound("Usuario no encontrado");

            if (UniqueFormatStrings.NormalizarString(ERoles.Usuario.ToString()) != UniqueFormatStrings.NormalizarString(rol) || UniqueFormatStrings.NormalizarString(ERoles.Admin.ToString()) != UniqueFormatStrings.NormalizarString(rol))
                return NotFound("El Role no se encuentra existente");

            if (UniqueFormatStrings.NormalizarString(ERoles.Usuario.ToString()) == UniqueFormatStrings.NormalizarString(rol))
                usuario.Role = ERoles.Usuario;
            else
            {
                usuario.Role = ERoles.Admin;
            }
            await _repoUsuario.UpdateUsuario(usuario);

            return Ok(usuario);
        }
        #endregion

    }
}