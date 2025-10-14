using System.Collections.Generic;
using System.Threading.Tasks;
using Evento.Core.Entidades;
using Evento.Core.Services.Repo;
using Moq;
using Xunit;

namespace Evento.Test
{
    public class TestAdoLocal
    {
        [Fact]
        public async Task Obtener_Todos_Los_Locals_Debe_Devolver_Lista()
        {
            var mockRepo = new Mock<IRepoLocal>();
            var locales = new List<Local>
            {
                new Local { idLocal = 1, Nombre = "Teatro A", Ubicacion = "Calle 1" },
                new Local { idLocal = 2, Nombre = "Estadio B", Ubicacion = "Calle 2" }
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
            var local = new Local { idLocal = 1, Nombre = "Teatro A", Ubicacion = "Calle 1" };
            mockRepo.Setup(r => r.ObtenerPorId(1)).ReturnsAsync(local);

            var resultado = await mockRepo.Object.ObtenerPorId(1);

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.idLocal);
            Assert.Equal("Teatro A", resultado.Nombre);
        }

        [Fact]
        public async Task Obtener_Sector_Por_Id_Debe_Devolver_Sector_Si_Existe()
        {
            var mockRepo = new Mock<IRepoLocal>();
            var localEjemplo = new Local { idLocal = 1, Nombre = "Teatro A", Ubicacion = "Calle 1" };
            var sector = new Sector { idSector = 1, Capacidad = 100, local = localEjemplo };
            mockRepo.Setup(r => r.ObtenerSectorPorId(1)).ReturnsAsync(sector);

            var resultado = await mockRepo.Object.ObtenerSectorPorId(1);

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.idSector);
            Assert.Equal(100, resultado.Capacidad);
        }

        [Fact]
        public async Task Insertar_Local_Debe_Devolver_Id()
        {
            var mockRepo = new Mock<IRepoLocal>();
            var local = new Local { Nombre = "Nuevo Local", Ubicacion = "Calle 3" };
            mockRepo.Setup(r => r.InsertLocal(local)).ReturnsAsync(5);

            var resultado = await mockRepo.Object.InsertLocal(local);

            Assert.Equal(5, resultado);
        }

        [Fact]
        public async Task Actualizar_Local_Debe_Devolver_True_Si_Se_Actualizo()
        {
            var mockRepo = new Mock<IRepoLocal>();
            var local = new Local { idLocal = 1, Nombre = "Teatro A", Ubicacion = "Calle 1" };
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
            var localEjemplo = new Local { idLocal = 1, Nombre = "Teatro A", Ubicacion = "Calle 1" };
            var sectores = new List<Sector>
            {
                new Sector { idSector = 1, Capacidad = 100, local = localEjemplo },
                new Sector { idSector = 2, Capacidad = 50, local = localEjemplo }
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
            var localEjemplo = new Local { idLocal = 1, Nombre = "Teatro A", Ubicacion = "Calle 1" };
            var sector = new Sector { Capacidad = 120, local = localEjemplo };
            mockRepo.Setup(r => r.InsertSector(sector, 1)).ReturnsAsync(10);

            var resultado = await mockRepo.Object.InsertSector(sector, 1);

            Assert.Equal(10, resultado);
        }

        [Fact]
        public async Task Actualizar_Sector_Debe_Devolver_True_Si_Se_Actualizo()
        {
            var mockRepo = new Mock<IRepoLocal>();
            var localEjemplo = new Local { idLocal = 1, Nombre = "Teatro A", Ubicacion = "Calle 1" };
            var sector = new Sector { idSector = 1, Capacidad = 100, local = localEjemplo };
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