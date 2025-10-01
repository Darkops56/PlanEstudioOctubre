using System.Collections.Generic;
using System.Threading.Tasks;
using Evento.Core.Entidades;
using Evento.Core.Services.Repo;
using Moq;
using Xunit;

namespace Evento.Tests
{
    public class TestAdoUsuario
    {
        [Fact]
        public async Task Obtener_Todos_Los_Usuarios_Debe_Devolver_Lista()
        {
            var mockRepo = new Mock<IRepoUsuario>();
            var usuarios = new List<Usuario>
            {
                new Usuario { IdUsuario = 1, Apodo = "Usuario1" },
                new Usuario { IdUsuario = 2, Apodo = "Usuario2" }
            };
            mockRepo.Setup(r => r.ObtenerTodos()).ReturnsAsync(usuarios);

            var resultado = await mockRepo.Object.ObtenerTodos();

            Assert.NotNull(resultado);
            Assert.Equal(2, ((List<Usuario>)resultado).Count);
        }

        [Fact]
        public async Task Obtener_Usuario_Por_Id_Debe_Devolver_Usuario_Si_Existe()
        {
            var mockRepo = new Mock<IRepoUsuario>();
            var usuario = new Usuario { IdUsuario = 1, Apodo = "Usuario1" };
            mockRepo.Setup(r => r.ObtenerPorId(1)).ReturnsAsync(usuario);

            var resultado = await mockRepo.Object.ObtenerPorId(1);

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.IdUsuario);
        }

        [Fact]
        public async Task Insertar_Usuario_Debe_Devolver_Id()
        {
            var mockRepo = new Mock<IRepoUsuario>();
            var usuario = new Usuario { Apodo = "NuevoUsuario" };
            mockRepo.Setup(r => r.InsertUsuario(usuario)).ReturnsAsync(5);

            var resultado = await mockRepo.Object.InsertUsuario(usuario);

            Assert.Equal(5, resultado);
        }

        [Fact]
        public async Task Actualizar_Usuario_Debe_Devolver_True_Si_Se_Actualizo()
        {
            var mockRepo = new Mock<IRepoUsuario>();
            var usuario = new Usuario { IdUsuario = 1, Apodo = "Usuario1" };
            mockRepo.Setup(r => r.UpdateUsuario(usuario)).ReturnsAsync(true);

            var resultado = await mockRepo.Object.UpdateUsuario(usuario);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Eliminar_Usuario_Debe_Devolver_True_Si_Se_Elimino()
        {
            var mockRepo = new Mock<IRepoUsuario>();
            mockRepo.Setup(r => r.DeleteUsuario(1)).ReturnsAsync(true);

            var resultado = await mockRepo.Object.DeleteUsuario(1);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Obtener_Compras_Por_Usuario_Debe_Devolver_Lista()
        {
            var mockRepo = new Mock<IRepoUsuario>();
            var compras = new List<OrdenesCompra>
            {
                new OrdenesCompra { Id = 1 },
                new OrdenesCompra { Id = 2 }
            };
            mockRepo.Setup(r => r.ObtenerComprasPorUsuario(1)).ReturnsAsync(compras);

            var resultado = await mockRepo.Object.ObtenerComprasPorUsuario(1);

            Assert.NotNull(resultado);
            Assert.Equal(2, ((List<OrdenesCompra>)resultado).Count);
        }

        [Fact]
        public async Task Obtener_Usuario_Por_Email_Debe_Devolver_Usuario_Si_Existe()
        {
            var mockRepo = new Mock<IRepoUsuario>();
            var usuario = new Usuario { IdUsuario = 1, Email = "test@correo.com" };
            mockRepo.Setup(r => r.ObtenerPorEmail("test@correo.com")).ReturnsAsync(usuario);

            var resultado = await mockRepo.Object.ObtenerPorEmail("test@correo.com");

            Assert.NotNull(resultado);
            Assert.Equal("test@correo.com", resultado.Email);
        }

        [Fact]
        public async Task Login_Debe_Devolver_Usuario_Si_Credenciales_Son_Correctas()
        {
            var mockRepo = new Mock<IRepoUsuario>();
            var usuario = new Usuario { IdUsuario = 1, Email = "test@correo.com" };
            mockRepo.Setup(r => r.Login("test@correo.com", "1234")).ReturnsAsync(usuario);

            var resultado = await mockRepo.Object.Login("test@correo.com", "1234");

            Assert.NotNull(resultado);
            Assert.Equal("test@correo.com", resultado.Email);
        }

        [Fact]
        public async Task Existe_Usuario_Por_Email_Debe_Devolver_True_Si_Existe()
        {
            var mockRepo = new Mock<IRepoUsuario>();
            mockRepo.Setup(r => r.ExisteUsuarioPorEmail("test@correo.com")).ReturnsAsync(true);

            var resultado = await mockRepo.Object.ExisteUsuarioPorEmail("test@correo.com");

            Assert.True(resultado);
        }
    }
}
