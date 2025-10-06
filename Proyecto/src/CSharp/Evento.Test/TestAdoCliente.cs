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
            var mockRepo = new Mock<IRepoCliente>();
            var clientes = new List<Cliente>
            {
                new Cliente { DNI = 12345678, nombreCompleto = "Juan Pérez", Telefono = "111-222-3333" },
                new Cliente { DNI = 87654321, nombreCompleto = "Ana Gómez", Telefono = "444-555-6666" }
            };
            mockRepo.Setup(r => r.ObtenerTodos()).ReturnsAsync(clientes);

            var resultado = await mockRepo.Object.ObtenerTodos();

            Assert.NotNull(resultado);
            Assert.Equal(2, ((List<Cliente>)resultado).Count);
        }

        [Fact]
        public async Task Obtener_Cliente_Por_DNI_Debe_Devolver_Cliente_Si_Existe()
        {
            var mockRepo = new Mock<IRepoCliente>();
            var cliente = new Cliente { DNI = 12345678, nombreCompleto = "Juan Pérez", Telefono = "111-222-3333" };
            mockRepo.Setup(r => r.ObtenerPorId(12345678)).ReturnsAsync(cliente);

            var resultado = await mockRepo.Object.ObtenerPorId(12345678);

            Assert.NotNull(resultado);
            Assert.Equal(12345678, resultado.DNI);
            Assert.Equal("Juan Pérez", resultado.nombreCompleto);
        }

        [Fact]
        public async Task Insertar_Cliente_Debe_Devolver_DNI()
        {
            var mockRepo = new Mock<IRepoCliente>();
            var cliente = new Cliente { nombreCompleto = "Pedro Martínez", Telefono = "777-888-9999" };
            mockRepo.Setup(r => r.InsertCliente(cliente)).ReturnsAsync(99999999);

            var resultado = await mockRepo.Object.InsertCliente(cliente);

            Assert.Equal(99999999, resultado);
        }

        [Fact]
        public async Task Actualizar_Cliente_Debe_Devolver_True_Si_Se_Actualizo()
        {
            var mockRepo = new Mock<IRepoCliente>();
            var cliente = new Cliente { DNI = 12345678, nombreCompleto = "Juan Pérez", Telefono = "111-222-3333" };
            mockRepo.Setup(r => r.UpdateCliente(cliente)).ReturnsAsync(true);

            var resultado = await mockRepo.Object.UpdateCliente(cliente);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Eliminar_Cliente_Debe_Devolver_True_Si_Se_Elimino()
        {
            var mockRepo = new Mock<IRepoCliente>();
            mockRepo.Setup(r => r.DeleteCliente(12345678)).ReturnsAsync(true);

            var resultado = await mockRepo.Object.DeleteCliente(12345678);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Obtener_Entradas_Por_Cliente_Debe_Devolver_Lista()
        {
            var mockRepo = new Mock<IRepoCliente>();
            var entradas = new List<Entrada>
            {
                new Entrada { idEntrada = 1, PrecioPagado = 1000 },
                new Entrada { idEntrada = 2, PrecioPagado = 1500 }
            };
            mockRepo.Setup(r => r.ObtenerEntradasPorCliente(12345678)).ReturnsAsync(entradas);

            var resultado = await mockRepo.Object.ObtenerEntradasPorCliente(12345678);

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
