using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Evento.Core.Entidades;
using Evento.Core.Services.Repo;
using Evento.Core.Services.Enums;
using Moq;
using Xunit;

namespace Evento.Test
{
    public class TestAdoFuncion
    {
        [Fact]
        public async Task Obtener_Todas_Las_Funciones_Debe_Devolver_Lista()
        {
            var mockRepo = new Mock<IRepoFuncion>();
            var eventoEjemplo = new Eventos { idEvento = 1, Nombre = "Concierto", fechaInicio = DateTime.Now, fechaFin = DateTime.Now.AddHours(2), EstadoEvento = EEstados.Activo };
            var funciones = new List<Funcion>
            {
                new Funcion { idFuncion = 1, Nombre = "Función 1", evento = eventoEjemplo, Fecha = DateTime.Now, Estado = EEstados.Activo },
                new Funcion { idFuncion = 2, Nombre = "Función 2", evento = eventoEjemplo, Fecha = DateTime.Now.AddHours(1), Estado = EEstados.Activo }
            };
            mockRepo.Setup(r => r.ObtenerTodos()).ReturnsAsync(funciones);

            var resultado = await mockRepo.Object.ObtenerTodos();

            Assert.NotNull(resultado);
            Assert.Equal(2, ((List<Funcion>)resultado).Count);
        }

        [Fact]
        public async Task Obtener_Funcion_Por_Id_Debe_Devolver_Funcion_Si_Existe()
        {
            var mockRepo = new Mock<IRepoFuncion>();
            var eventoEjemplo = new Eventos { idEvento = 1, Nombre = "Concierto", fechaInicio = DateTime.Now, fechaFin = DateTime.Now.AddHours(2), EstadoEvento = EEstados.Activo };
            var funcion = new Funcion { idFuncion = 1, Nombre = "Función 1", evento = eventoEjemplo, Fecha = DateTime.Now, Estado = EEstados.Activo };
            mockRepo.Setup(r => r.ObtenerPorId(1)).ReturnsAsync(funcion);

            var resultado = await mockRepo.Object.ObtenerPorId(1);

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.idFuncion);
            Assert.Equal("Función 1", resultado.Nombre);
        }

        [Fact]
        public async Task Insertar_Funcion_Debe_Devolver_Id()
        {
            var mockRepo = new Mock<IRepoFuncion>();
            var eventoEjemplo = new Eventos { idEvento = 1, Nombre = "Concierto", fechaInicio = DateTime.Now, fechaFin = DateTime.Now.AddHours(2), EstadoEvento = EEstados.Activo };
            var funcion = new Funcion { Nombre = "Nueva Función", evento = eventoEjemplo, Fecha = DateTime.Now, Estado = EEstados.Activo };
            mockRepo.Setup(r => r.InsertFuncion(funcion)).ReturnsAsync(5);

            var resultado = await mockRepo.Object.InsertFuncion(funcion);

            Assert.Equal(5, resultado);
        }

        [Fact]
        public async Task Actualizar_Funcion_Debe_Devolver_True_Si_Se_Actualizo()
        {
            var mockRepo = new Mock<IRepoFuncion>();
            var eventoEjemplo = new Eventos { idEvento = 1, Nombre = "Concierto", fechaInicio = DateTime.Now, fechaFin = DateTime.Now.AddHours(2), EstadoEvento = EEstados.Activo };
            var funcion = new Funcion { idFuncion = 1, Nombre = "Función 1", evento = eventoEjemplo, Fecha = DateTime.Now, Estado = EEstados.Activo };
            mockRepo.Setup(r => r.UpdateFuncion(funcion)).ReturnsAsync(true);

            var resultado = await mockRepo.Object.UpdateFuncion(funcion);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Eliminar_Funcion_Debe_Devolver_True_Si_Se_Elimino()
        {
            var mockRepo = new Mock<IRepoFuncion>();
            mockRepo.Setup(r => r.DeleteFuncion(1)).ReturnsAsync(true);

            var resultado = await mockRepo.Object.DeleteFuncion(1);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Obtener_Tarifas_De_Funcion_Debe_Devolver_Lista()
        {
            var mockRepo = new Mock<IRepoFuncion>();
            var eventoEjemplo = new Eventos { idEvento = 1, Nombre = "Concierto", fechaInicio = DateTime.Now, fechaFin = DateTime.Now.AddHours(2), EstadoEvento = EEstados.Activo };
            var funcionEjemplo = new Funcion { idFuncion = 1, Nombre = "Función 1", evento = eventoEjemplo, Fecha = DateTime.Now, Estado = EEstados.Activo };
            var tarifas = new List<Tarifa>
            {
                new Tarifa { idTarifa = 1, Precio = 100, Estado = EEstados.Pendiente, Stock = 50, funcion = funcionEjemplo },
                new Tarifa { idTarifa = 2, Precio = 200, Estado = EEstados.Pendiente, Stock = 30, funcion = funcionEjemplo }
            };
            mockRepo.Setup(r => r.ObtenerTarifasDeFuncion(1)).ReturnsAsync(tarifas);

            var resultado = await mockRepo.Object.ObtenerTarifasDeFuncion(1);

            Assert.NotNull(resultado);
            Assert.Equal(2, ((List<Tarifa>)resultado).Count);
        }

        [Fact]
        public async Task Cancelar_Funcion_Debe_Devolver_Cadena()
        {
            var mockRepo = new Mock<IRepoFuncion>();
            mockRepo.Setup(r => r.CancelarFuncion(1)).ReturnsAsync("Función cancelada correctamente");

            var resultado = await mockRepo.Object.CancelarFuncion(1);

            Assert.Equal("Función cancelada correctamente", resultado);
        }
    }
}