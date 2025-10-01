using System.Collections.Generic;
using System.Threading.Tasks;
using Evento.Core.Entidades;
using Evento.Core.Services.Repo;
using Moq;
using Xunit;

namespace Evento.Test
{
    public class TestAdoEvento
    {
        [Fact]
        public async Task Obtener_Todos_Los_Eventos_Debe_Devolver_Lista()
        {
            var mockRepo = new Mock<IRepoEvento>();
            var eventos = new List<Eventos>
            {
                new Eventos { Id = 1, Nombre = "Concierto" },
                new Eventos { Id = 2, Nombre = "Obra de teatro" }
            };
            mockRepo.Setup(r => r.ObtenerTodos()).ReturnsAsync(eventos);

            var resultado = await mockRepo.Object.ObtenerTodos();

            Assert.NotNull(resultado);
            Assert.Equal(2, ((List<Eventos>)resultado).Count);
        }

        [Fact]
        public async Task Obtener_Evento_Por_Id_Debe_Devolver_Evento_Si_Existe()
        {
            var mockRepo = new Mock<IRepoEvento>();
            var evento = new Eventos { Id = 1, Nombre = "Concierto" };
            mockRepo.Setup(r => r.ObtenerEventoPorId(1)).ReturnsAsync(evento);

            var resultado = await mockRepo.Object.ObtenerEventoPorId(1);

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.Id);
            Assert.Equal("Concierto", resultado.Nombre);
        }

        [Fact]
        public async Task Obtener_TipoEvento_Por_Id_Debe_Devolver_TipoEvento()
        {
            var mockRepo = new Mock<IRepoEvento>();
            var tipo = new TipoEvento { Id = 1, Nombre = "Música" };
            mockRepo.Setup(r => r.ObtenerTipoEventoPorId(1)).ReturnsAsync(tipo);

            var resultado = await mockRepo.Object.ObtenerTipoEventoPorId(1);

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.Id);
        }

        [Fact]
        public async Task Obtener_Evento_Por_Nombre_Debe_Devolver_Evento_Si_Existe()
        {
            var mockRepo = new Mock<IRepoEvento>();
            var evento = new Eventos { Id = 1, Nombre = "Concierto" };
            mockRepo.Setup(r => r.ObtenerEventoPorNombre("Concierto")).ReturnsAsync(evento);

            var resultado = await mockRepo.Object.ObtenerEventoPorNombre("Concierto");

            Assert.NotNull(resultado);
            Assert.Equal("Concierto", resultado.Nombre);
        }

        [Fact]
        public async Task Insertar_Evento_Debe_Devolver_Id()
        {
            var mockRepo = new Mock<IRepoEvento>();
            var evento = new Eventos { Nombre = "Exposición" };
            mockRepo.Setup(r => r.InsertEvento(evento)).ReturnsAsync(5);

            var resultado = await mockRepo.Object.InsertEvento(evento);

            Assert.Equal(5, resultado);
        }

        [Fact]
        public async Task Actualizar_Evento_Debe_Devolver_True_Si_Se_Actualizo()
        {
            var mockRepo = new Mock<IRepoEvento>();
            var evento = new Eventos { Id = 1, Nombre = "Concierto" };
            mockRepo.Setup(r => r.UpdateEvento(evento)).ReturnsAsync(true);

            var resultado = await mockRepo.Object.UpdateEvento(evento);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Eliminar_Evento_Debe_Devolver_True_Si_Se_Elimino()
        {
            var mockRepo = new Mock<IRepoEvento>();
            mockRepo.Setup(r => r.DeleteEvento(1)).ReturnsAsync(true);

            var resultado = await mockRepo.Object.DeleteEvento(1);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Obtener_Funciones_Por_Evento_Debe_Devolver_Lista()
        {
            var mockRepo = new Mock<IRepoEvento>();
            var funciones = new List<Funcion>
            {
                new Funcion { Id = 1, Descripcion = "Función 1" },
                new Funcion { Id = 2, Descripcion = "Función 2" }
            };
            mockRepo.Setup(r => r.ObtenerFuncionesPorEventoAsync(1)).ReturnsAsync(funciones);

            var resultado = await mockRepo.Object.ObtenerFuncionesPorEventoAsync(1);

            Assert.NotNull(resultado);
            Assert.Equal(2, ((List<Funcion>)resultado).Count);
        }

        [Fact]
        public async Task Obtener_Sectores_Con_Tarifa_Debe_Devolver_Lista()
        {
            var mockRepo = new Mock<IRepoEvento>();
            var sectores = new List<Sector>
            {
                new Sector { Id = 1, Nombre = "Platea" },
                new Sector { Id = 2, Nombre = "VIP" }
            };
            mockRepo.Setup(r => r.ObtenerSectoresConTarifaAsync(1)).ReturnsAsync(sectores);

            var resultado = await mockRepo.Object.ObtenerSectoresConTarifaAsync(1);

            Assert.NotNull(resultado);
            Assert.Equal(2, ((List<Sector>)resultado).Count);
        }

        [Fact]
        public async Task Publicar_Evento_Debe_Devolver_Cadena()
        {
            var mockRepo = new Mock<IRepoEvento>();
            mockRepo.Setup(r => r.PublicarEvento(1)).ReturnsAsync("Evento publicado correctamente");

            var resultado = await mockRepo.Object.PublicarEvento(1);

            Assert.Equal("Evento publicado correctamente", resultado);
        }

        [Fact]
        public async Task Cancelar_Evento_Debe_Devolver_Cadena()
        {
            var mockRepo = new Mock<IRepoEvento>();
            mockRepo.Setup(r => r.CancelarEvento(1)).ReturnsAsync("Evento cancelado correctamente");

            var resultado = await mockRepo.Object.CancelarEvento(1);

            Assert.Equal("Evento cancelado correctamente", resultado);
        }
    }
}
