using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Evento.Core.Entidades;
using Evento.Core.Services.Repo;
using Moq;
using Xunit;

namespace Evento.Test
{
    public class TestAdoQR
    {
        [Fact]
        public async Task Insertar_QR_Debe_Devolver_Id()
        {
            var mockRepo = new Mock<IRepoQR>();
            var qr = new QR 
            { 
                idEntrada = 1, 
                url = "http://localhost/qr", 
                token = "abc123", 
                ExpiraEn = DateTime.Now.AddMinutes(5), 
                VCard = "VCARD DATA"
            };

            mockRepo.Setup(r => r.InsertQR(qr)).ReturnsAsync(42);

            var resultado = await mockRepo.Object.InsertQR(qr);

            Assert.Equal(42, resultado);
        }

        [Fact]
        public async Task Obtener_QR_Por_Entrada_Debe_Devolver_QR_Si_Existe()
        {
            var mockRepo = new Mock<IRepoQR>();
            var qr = new QR 
            { 
                idQR = 1, 
                idEntrada = 1, 
                url = "http://localhost/qr", 
                token = "abc123", 
                ExpiraEn = DateTime.Now.AddMinutes(5), 
                VCard = "VCARD DATA"
            };

            mockRepo.Setup(r => r.ObtenerQRPorEntrada(1)).ReturnsAsync(qr);

            var resultado = await mockRepo.Object.ObtenerQRPorEntrada(1);

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.idQR);
            Assert.Equal(1, resultado.idEntrada);
            Assert.Equal("abc123", resultado.token);
        }

        [Fact]
        public async Task Obtener_QR_Por_Token_Debe_Devolver_QR_Si_Existe()
        {
            var mockRepo = new Mock<IRepoQR>();
            var qr = new QR 
            { 
                idQR = 1, 
                idEntrada = 1, 
                url = "http://localhost/qr", 
                token = "abc123", 
                ExpiraEn = DateTime.Now.AddMinutes(5), 
                VCard = "VCARD DATA"
            };

            mockRepo.Setup(r => r.ObtenerQRPorToken("abc123")).ReturnsAsync(qr);

            var resultado = await mockRepo.Object.ObtenerQRPorToken("abc123");

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.idQR);
            Assert.Equal("abc123", resultado.token);
        }
    }
}