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
    }
}