using System.Collections.Generic;
using System.Threading.Tasks;
using Evento.Core.Entidades;
using Evento.Core.Services.Repo;
using Moq;
using Xunit;

namespace Evento.Tests
{
    public class TestAdoLocal
    {
        [Fact]
        public async Task Obtener_Todos_Los_Locals_Debe_Devolver_Lista()
        {
            var mockRepo = new Mock<IRepoLocal>();
            var locales = new List<Local>
            {
                new Local { IdLocal = 1, Nombre = "Teatro A" },
                new Local { IdLocal = 2, Nombre = "Estadio B" }
            };
            mockRepo.Setup(r => r.ObtenerTodos()).ReturnsAsync(locales);

            var resultado = await mockRepo.Object.ObtenerTodos();

            Assert.NotNull(resultado);
            Assert.Equal(2, ((List<Local>)resultado).Count);
        }

        [Fact]
        public async Task Obtener_Local_Por_Id_Debe_Devolver_Local_Si_Existe()
        {
            var mockRepo = new Mock<IRepoLocal>();
            var local = new Local { IdLocal = 1, Nombre = "Teatro A" };
            mockRepo.Setup(r => r.ObtenerPorId(1)).ReturnsAsync(local);

            var resultado = await mockRepo.Object.ObtenerPorId(1);

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.IdLocal);
        }

        [Fact]
        public async Task Obtener_Sector_Por_Id_Debe_Devolver_Sector_Si_Existe()
        {
            var mockRepo = new Mock<IRepoLocal>();
            var sector = new Sector { Id = 1, Nombre = "Platea" };
            mockRepo.Setup(r => r.ObtenerSectorPorId(1)).ReturnsAsync(sector);

            var resultado = await mockRepo.Object.ObtenerSectorPorId(1);

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.Id);
        }

        [Fact]
        public async Task Insertar_Local_Debe_Devolver_Id()
        {
            var mockRepo = new Mock<IRepoLocal>();
            var local = new Local { Nombre = "Nuevo Local" };
            mockRepo.Setup(r => r.InsertLocal(local)).ReturnsAsync(5);

            var resultado = await mockRepo.Object.InsertLocal(local);

            Assert.Equal(5, resultado);
        }

        [Fact]
        public async Task Actualizar_Local_Debe_Devolver_True_Si_Se_Actualizo()
        {
            var mockRepo = new Mock<IRepoLocal>();
            var local = new Local { IdLocal = 1, Nombre = "Teatro A" };
            mockRepo.Setup(r => r.UpdateLocal(local)).ReturnsAsync(true);

            var resultado = await mockRepo.Object.UpdateLocal(local);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Eliminar_Local_Debe_Devolver_True_Si_Se_Elimino()
        {
            var mockRepo = new Mock<IRepoLocal>();
            mockRepo.Setup(r => r.DeleteLocal(1)).ReturnsAsync(true);

            var resultado = await mockRepo.Object.DeleteLocal(1);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Obtener_Sectores_Del_Local_Debe_Devolver_Lista()
        {
            var mockRepo = new Mock<IRepoLocal>();
            var sectores = new List<Sector>
            {
                new Sector { Id = 1, Nombre = "Platea" },
                new Sector { Id = 2, Nombre = "VIP" }
            };
            mockRepo.Setup(r => r.ObtenerSectoresDelLocal(1)).ReturnsAsync(sectores);

            var resultado = await mockRepo.Object.ObtenerSectoresDelLocal(1);

            Assert.NotNull(resultado);
            Assert.Equal(2, ((List<Sector>)resultado).Count);
        }

        [Fact]
        public async Task Insertar_Sector_Debe_Devolver_Id()
        {
            var mockRepo = new Mock<IRepoLocal>();
            var sector = new Sector { Nombre = "Balcon" };
            mockRepo.Setup(r => r.InsertSector(sector, 1)).ReturnsAsync(10);

            var resultado = await mockRepo.Object.InsertSector(sector, 1);

            Assert.Equal(10, resultado);
        }

        [Fact]
        public async Task Actualizar_Sector_Debe_Devolver_True_Si_Se_Actualizo()
        {
            var mockRepo = new Mock<IRepoLocal>();
            var sector = new Sector { Id = 1, Nombre = "Platea" };
            mockRepo.Setup(r => r.UpdateSector(sector, 1)).ReturnsAsync(true);

            var resultado = await mockRepo.Object.UpdateSector(sector, 1);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Eliminar_Sector_Debe_Devolver_True_Si_Se_Elimino()
        {
            var mockRepo = new Mock<IRepoLocal>();
            mockRepo.Setup(r => r.DeleteSector(1)).ReturnsAsync(true);

            var resultado = await mockRepo.Object.DeleteSector(1);

            Assert.True(resultado);
        }
    }
}
