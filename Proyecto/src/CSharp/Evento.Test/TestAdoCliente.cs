using System.Collections.Generic;
using System.Threading.Tasks;
using Evento.Core.Entidades;
using Evento.Core.Services.Repo;
using Moq;
using Xunit;

namespace Evento.Test
{
    public class TestAdoCliente
    {
        [Fact]
        public async Task Obtener_Todos_Los_Clientes_Debe_Devolver_Lista()
        {
            // Arrange
            var mockRepo = new Mock<IRepoCliente>();
            var clientes = new List<Cliente>
            {
                new Cliente { Id = 1, Nombre = "Juan" },
                new Cliente { Id = 2, Nombre = "Ana" }
            };
            mockRepo.Setup(r => r.ObtenerTodos()).ReturnsAsync(clientes);

            // Act
            var resultado = await mockRepo.Object.ObtenerTodos();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, ((List<Cliente>)resultado).Count);
        }

        [Fact]
        public async Task Obtener_Cliente_Por_Id_Debe_Devolver_Cliente_Si_Existe()
        {
            var mockRepo = new Mock<IRepoCliente>();
            var cliente = new Cliente { Id = 1, Nombre = "Juan" };
            mockRepo.Setup(r => r.ObtenerPorId(1)).ReturnsAsync(cliente);

            var resultado = await mockRepo.Object.ObtenerPorId(1);

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.Id);
            Assert.Equal("Juan", resultado.Nombre);
        }

        [Fact]
        public async Task Insertar_Cliente_Debe_Devolver_Id()
        {
            var mockRepo = new Mock<IRepoCliente>();
            var cliente = new Cliente { Nombre = "Pedro" };
            mockRepo.Setup(r => r.InsertCliente(cliente)).ReturnsAsync(5);

            var resultado = await mockRepo.Object.InsertCliente(cliente);

            Assert.Equal(5, resultado);
        }

        [Fact]
        public async Task Actualizar_Cliente_Debe_Devolver_True_Si_Se_Actualizo()
        {
            var mockRepo = new Mock<IRepoCliente>();
            var cliente = new Cliente { Id = 1, Nombre = "Juan" };
            mockRepo.Setup(r => r.UpdateCliente(cliente)).ReturnsAsync(true);

            var resultado = await mockRepo.Object.UpdateCliente(cliente);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Eliminar_Cliente_Debe_Devolver_True_Si_Se_Elimino()
        {
            var mockRepo = new Mock<IRepoCliente>();
            mockRepo.Setup(r => r.DeleteCliente(1)).ReturnsAsync(true);

            var resultado = await mockRepo.Object.DeleteCliente(1);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Obtener_Entradas_Por_Cliente_Debe_Devolver_Lista()
        {
            var mockRepo = new Mock<IRepoCliente>();
            var entradas = new List<Entrada>
            {
                new Entrada { Id = 1, Descripcion = "Entrada1" },
                new Entrada { Id = 2, Descripcion = "Entrada2" }
            };
            mockRepo.Setup(r => r.ObtenerEntradasPorCliente(1)).ReturnsAsync(entradas);

            var resultado = await mockRepo.Object.ObtenerEntradasPorCliente(1);

            Assert.NotNull(resultado);
            Assert.Equal(2, ((List<Entrada>)resultado).Count);
        }

        [Fact]
        public async Task Existe_Cliente_Por_DNI_Debe_Devolver_True_Si_Existe()
        {
            var mockRepo = new Mock<IRepoCliente>();
            mockRepo.Setup(r => r.ExistePorDNI(12345678)).ReturnsAsync(true);

            var resultado = await mockRepo.Object.ExistePorDNI(12345678);

            Assert.True(resultado);
        }
    }
}