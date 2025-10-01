using System.Collections.Generic;
using System.Threading.Tasks;
using Evento.Core.Entidades;
using Evento.Core.Services.Repo;
using Moq;
using Xunit;

namespace Evento.Tests
{
    public class TestAdoTarifa
    {
        [Fact]
        public async Task Obtener_Todas_Las_Tarifas_Debe_Devolver_Lista()
        {
            var mockRepo = new Mock<IRepoTarifa>();
            var tarifas = new List<Tarifa>
            {
                new Tarifa { Id = 1, Descripcion = "Tarifa 1" },
                new Tarifa { Id = 2, Descripcion = "Tarifa 2" }
            };
            mockRepo.Setup(r => r.ObtenerTodos()).ReturnsAsync(tarifas);

            var resultado = await mockRepo.Object.ObtenerTodos();

            Assert.NotNull(resultado);
            Assert.Equal(2, ((List<Tarifa>)resultado).Count);
        }

        [Fact]
        public async Task Obtener_Tarifa_Por_Id_Debe_Devolver_Tarifa_Si_Existe()
        {
            var mockRepo = new Mock<IRepoTarifa>();
            var tarifa = new Tarifa { Id = 1, Descripcion = "Tarifa 1" };
            mockRepo.Setup(r => r.ObtenerPorId(1)).ReturnsAsync(tarifa);

            var resultado = await mockRepo.Object.ObtenerPorId(1);

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.Id);
        }

        [Fact]
        public async Task Insertar_Tarifa_Debe_Devolver_Id()
        {
            var mockRepo = new Mock<IRepoTarifa>();
            var tarifa = new Tarifa { Descripcion = "Nueva Tarifa" };
            mockRepo.Setup(r => r.InsertTarifa(tarifa)).ReturnsAsync(5);

            var resultado = await mockRepo.Object.InsertTarifa(tarifa);

            Assert.Equal(5, resultado);
        }

        [Fact]
        public async Task Actualizar_Tarifa_Debe_Devolver_True_Si_Se_Actualizo()
        {
            var mockRepo = new Mock<IRepoTarifa>();
            var tarifa = new Tarifa { Id = 1, Descripcion = "Tarifa 1" };
            mockRepo.Setup(r => r.UpdateTarifa(tarifa)).ReturnsAsync(true);

            var resultado = await mockRepo.Object.UpdateTarifa(tarifa);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Reducir_Stock_Debe_Devolver_True_Si_Se_Redujo()
        {
            var mockRepo = new Mock<IRepoTarifa>();
            mockRepo.Setup(r => r.ReducirStock(1)).ReturnsAsync(true);

            var resultado = await mockRepo.Object.ReducirStock(1);

            Assert.True(resultado);
        }
    }
}
