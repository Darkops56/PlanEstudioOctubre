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
            var mockRepo = new Mock<IRepoEntrada>();
            var entradas = new List<Entrada>
            {
                new Entrada { idEntrada = 1, PrecioPagado = 1000, Estado = EEstados.Activo },
                new Entrada { idEntrada = 2, PrecioPagado = 2000, Estado = EEstados.Activo }
            };

            mockRepo.Setup(r => r.ObtenerTodos()).ReturnsAsync(entradas);

            var resultado = await mockRepo.Object.ObtenerTodos();

            Assert.NotNull(resultado);
            Assert.Equal(2, ((List<Entrada>)resultado).Count);
        }

        [Fact]
        public async Task Insertar_Entrada_Debe_Devolver_Id()
        {
            var mockRepo = new Mock<IRepoEntrada>();
            var entrada = new Entrada { PrecioPagado = 1500, Estado = EEstados.Activo };
            mockRepo.Setup(r => r.InsertEntrada(entrada)).ReturnsAsync(10);

            var resultado = await mockRepo.Object.InsertEntrada(entrada);

            Assert.Equal(10, resultado);
        }

        [Fact]
        public async Task Eliminar_Entrada_Debe_Devolver_True_Si_Se_Elimino()
        {
            var mockRepo = new Mock<IRepoEntrada>();
            mockRepo.Setup(r => r.DeleteEntrada(1)).ReturnsAsync(true);

            var resultado = await mockRepo.Object.DeleteEntrada(1);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Obtener_Entrada_Por_Id_Debe_Devolver_Entrada_Si_Existe()
        {
            var mockRepo = new Mock<IRepoEntrada>();
            var entrada = new Entrada { idEntrada = 1, PrecioPagado = 1800, Estado = EEstados.Activo };
            mockRepo.Setup(r => r.ObtenerEntrada(1)).ReturnsAsync(entrada);

            var resultado = await mockRepo.Object.ObtenerEntrada(1);

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.idEntrada);
            Assert.Equal(1800, resultado.PrecioPagado);
            Assert.Equal(EEstados.Activo, resultado.Estado);
        }

        [Fact]
        public async Task Anular_Entrada_Debe_Devolver_Cadena()
        {
            var mockRepo = new Mock<IRepoEntrada>();
            mockRepo.Setup(r => r.AnularEntrada(1)).ReturnsAsync("Entrada anulada correctamente");

            var resultado = await mockRepo.Object.AnularEntrada(1);

            Assert.Equal("Entrada anulada correctamente", resultado);
        }

        [Fact]
        public async Task Marcar_Entrada_Usada_Debe_Devolver_True_Si_Se_Actualizo()
        {
            var mockRepo = new Mock<IRepoEntrada>();
            mockRepo.Setup(r => r.MarcarEntradaUsada(1)).ReturnsAsync(true);

            var resultado = await mockRepo.Object.MarcarEntradaUsada(1);

            Assert.True(resultado);
        }
    }
}