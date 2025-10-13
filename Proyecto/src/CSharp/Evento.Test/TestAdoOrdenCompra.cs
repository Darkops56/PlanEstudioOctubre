using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Evento.Core.Entidades;
using Evento.Core.Services.Enums;
using Evento.Core.Services.Repo;
using Moq;
using Xunit;

namespace Evento.Test
{
    public class TestAdoOrdenCompra
    {
        [Fact]
        public async Task Insertar_OrdenCompra_Debe_Devolver_Id()
        {
            var mockRepo = new Mock<IRepoOrdenCompra>();
            var orden = new OrdenesCompra
            {
                usuario = new Usuario { idUsuario = 1, Apodo = "Juan", Email = "juan@mail.com" },
                entradas = new List<Entrada>(),
                Fecha = DateTime.Now,
                Total = 500,
                metodoPago = EMetodoPago.Efectivo,
                Estado = EEstados.Activo
            };
            mockRepo.Setup(r => r.InsertOrdenCompra(orden)).ReturnsAsync(5);

            var resultado = await mockRepo.Object.InsertOrdenCompra(orden);

            Assert.Equal(5, resultado);
        }

        [Fact]
        public async Task Obtener_OrdenCompra_Por_Id_Debe_Devolver_Orden_Si_Existe()
        {
            var mockRepo = new Mock<IRepoOrdenCompra>();
            var orden = new OrdenesCompra
            {
                idOrdenCompra = 1,
                usuario = new Usuario { idUsuario = 1, Apodo = "Juan", Email = "juan@mail.com" },
                entradas = new List<Entrada>(),
                Fecha = DateTime.Now,
                Total = 500,
                metodoPago = EMetodoPago.Efectivo,
                Estado = EEstados.Activo
            };
            mockRepo.Setup(r => r.ObtenerOrdenCompra(1)).ReturnsAsync(orden);

            var resultado = await mockRepo.Object.ObtenerOrdenCompra(1);

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.idOrdenCompra);
        }

        [Fact]
        public async Task Obtener_Todas_Las_OrdenesCompra_Debe_Devolver_Lista()
        {
            var mockRepo = new Mock<IRepoOrdenCompra>();
            var ordenes = new List<OrdenesCompra>
            {
                new OrdenesCompra { idOrdenCompra = 1 },
                new OrdenesCompra { idOrdenCompra = 2 }
            };
            mockRepo.Setup(r => r.ObtenerOrdenesCompra()).ReturnsAsync(ordenes);

            var resultado = await mockRepo.Object.ObtenerOrdenesCompra();

            Assert.NotNull(resultado);
            Assert.Equal(2, ((List<OrdenesCompra>)resultado).Count);
        }

        [Fact]
        public async Task Pagar_OrdenCompra_Debe_Devolver_Cadena()
        {
            var mockRepo = new Mock<IRepoOrdenCompra>();
            mockRepo.Setup(r => r.PagarOrdenCompra(1)).ReturnsAsync("Orden pagada correctamente");

            var resultado = await mockRepo.Object.PagarOrdenCompra(1);

            Assert.Equal("Orden pagada correctamente", resultado);
        }

        [Fact]
        public async Task Cancelar_OrdenCompra_Debe_Devolver_Cadena()
        {
            var mockRepo = new Mock<IRepoOrdenCompra>();
            mockRepo.Setup(r => r.CancelarOrdenCompra(1)).ReturnsAsync("Orden cancelada correctamente");

            var resultado = await mockRepo.Object.CancelarOrdenCompra(1);

            Assert.Equal("Orden cancelada correctamente", resultado);
        }

        [Fact]
        public async Task Liberar_Stock_Expirado_Debe_Devolver_Cantidad()
        {
            var mockRepo = new Mock<IRepoOrdenCompra>();
            mockRepo.Setup(r => r.LiberarStockExpirado()).ReturnsAsync(3);

            var resultado = await mockRepo.Object.LiberarStockExpirado();

            Assert.Equal(3, resultado);
        }

        [Fact]
        public void Obtener_MetodoPago_Debe_Devolver_Correcto()
        {
            var mockRepo = new Mock<IRepoOrdenCompra>();
            mockRepo.Setup(r => r.ObtenerMetodoPago("Efectivo")).Returns(EMetodoPago.Efectivo);

            var resultado = mockRepo.Object.ObtenerMetodoPago("Efectivo");

            Assert.Equal(EMetodoPago.Efectivo, resultado);
        }

        [Fact]
        public void Obtener_Estado_Debe_Devolver_Correcto()
        {
            var mockRepo = new Mock<IRepoOrdenCompra>();
            mockRepo.Setup(r => r.ObtenerEstado("Activo")).Returns(EEstados.Activo);

            var resultado = mockRepo.Object.ObtenerEstado("Activo");

            Assert.Equal(EEstados.Activo, resultado);
        }

        [Fact]
        public async Task InsertStockReservaciones_Debe_Devolver_Cadena_Correcta()
        {
            var mockRepo = new Mock<IRepoOrdenCompra>();
            var stock = new StockReservaciones
            {
                idTarifa = 1,
                Cantidad = 2,
                fechReserva = DateTime.Now,
                expiraEn = DateTime.Now.AddMinutes(10),
                idOrdenCompra = 5
            };
            mockRepo.Setup(r => r.InsertStockReservaciones(stock)).ReturnsAsync("Se creo correctamente");

            var resultado = await mockRepo.Object.InsertStockReservaciones(stock);

            Assert.Equal("Se creo correctamente", resultado);
        }

        [Fact]
        public async Task ObtenerReservacionesPorIdOrden_Debe_Devolver_Lista()
        {
            var mockRepo = new Mock<IRepoOrdenCompra>();
            var reservaciones = new List<StockReservaciones>
            {
                new StockReservaciones { idStockReservacion = 1, idTarifa = 1, Cantidad = 1, idOrdenCompra = 5 },
                new StockReservaciones { idStockReservacion = 2, idTarifa = 2, Cantidad = 2, idOrdenCompra = 5 }
            };
            mockRepo.Setup(r => r.ObtenerReservacionesPorIdOrden(5)).ReturnsAsync(reservaciones);

            var resultado = await mockRepo.Object.ObtenerReservacionesPorIdOrden(5);

            Assert.NotNull(resultado);
            Assert.Equal(2, ((List<StockReservaciones>)resultado).Count);
        }
    }
}