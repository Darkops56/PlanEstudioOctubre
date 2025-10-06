using System.Collections.Generic;
using System.Threading.Tasks;
using Evento.Core.Entidades;
using Evento.Core.Services.Repo;
using Evento.Core.Services.Enums;
using Moq;
using Xunit;

namespace Evento.Test
{
    public class TestAdoEntrada
    {
        [Fact]
        public async Task Obtener_Todas_Las_Entradas_Debe_Devolver_Lista()
        {
            // Arrange
            var mockRepo = new Mock<IRepoEntrada>();
            var entradas = new List<Entrada>
            {
                new Entrada { idEntrada = 1, PrecioPagado = 1000, Estado = EEstados.Activo },
                new Entrada { idEntrada = 2, PrecioPagado = 2000, Estado = EEstados.Activo }
            };

            mockRepo.Setup(r => r.ObtenerTodos()).ReturnsAsync(entradas);

            // Act
            var resultado = await mockRepo.Object.ObtenerTodos();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, ((List<Entrada>)resultado).Count);
        }

        [Fact]
        public async Task Insertar_Entrada_Debe_Devolver_Id()
        {
            // Arrange
            var mockRepo = new Mock<IRepoEntrada>();
            var entrada = new Entrada { PrecioPagado = 1500, Estado = EEstados.Activo };
            mockRepo.Setup(r => r.InsertEntrada(entrada)).ReturnsAsync(10);

            // Act
            var resultado = await mockRepo.Object.InsertEntrada(entrada);

            // Assert
            Assert.Equal(10, resultado);
        }

        [Fact]
        public async Task Eliminar_Entrada_Debe_Devolver_True_Si_Se_Elimino()
        {
            // Arrange
            var mockRepo = new Mock<IRepoEntrada>();
            mockRepo.Setup(r => r.DeleteEntrada(1)).ReturnsAsync(true);

            // Act
            var resultado = await mockRepo.Object.DeleteEntrada(1);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public async Task Obtener_Entrada_Por_Id_Debe_Devolver_Entrada_Si_Existe()
        {
            // Arrange
            var mockRepo = new Mock<IRepoEntrada>();
            var entrada = new Entrada { idEntrada = 1, PrecioPagado = 1800, Estado = EEstados.Activo };
            mockRepo.Setup(r => r.ObtenerEntrada(1)).ReturnsAsync(entrada);

            // Act
            var resultado = await mockRepo.Object.ObtenerEntrada(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.idEntrada);
            Assert.Equal(1800, resultado.PrecioPagado);
            Assert.Equal(EEstados.Activo, resultado.Estado);
        }

        [Fact]
        public async Task Anular_Entrada_Debe_Devolver_Cadena()
        {
            // Arrange
            var mockRepo = new Mock<IRepoEntrada>();
            mockRepo.Setup(r => r.AnularEntrada(1)).ReturnsAsync("Entrada anulada correctamente");

            // Act
            var resultado = await mockRepo.Object.AnularEntrada(1);

            // Assert
            Assert.Equal("Entrada anulada correctamente", resultado);
        }
    }
}

