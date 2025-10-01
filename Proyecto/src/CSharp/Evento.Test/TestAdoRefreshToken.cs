using System;
using System.Threading.Tasks;
using Evento.Core.Entidades;
using Evento.Core.Services.Repo;
using Moq;
using Xunit;

namespace Evento.Tests
{
    public class TestAdoRefreshToken
    {
        [Fact]
        public async Task Insertar_Token_Debe_Devolver_Id()
        {
            var mockRepo = new Mock<IRepoRefreshToken>();
            var token = new RefreshToken { Id = 0, Token = "abc123" };
            mockRepo.Setup(r => r.InsertToken(token)).ReturnsAsync(5);

            var resultado = await mockRepo.Object.InsertToken(token);

            Assert.Equal(5, resultado);
        }

        [Fact]
        public async Task Obtener_Token_Debe_Devolver_Token_Si_Existe()
        {
            var mockRepo = new Mock<IRepoRefreshToken>();
            var token = new RefreshToken { Id = 1, Token = "abc123" };
            mockRepo.Setup(r => r.ObtenerToken("abc123")).ReturnsAsync(token);

            var resultado = await mockRepo.Object.ObtenerToken("abc123");

            Assert.NotNull(resultado);
            Assert.Equal("abc123", resultado.Token);
        }

        [Fact]
        public async Task Eliminar_Token_Debe_Correr_Sin_Errores()
        {
            var mockRepo = new Mock<IRepoRefreshToken>();
            mockRepo.Setup(r => r.DeleteToken("abc123")).Returns(Task.CompletedTask);

            await mockRepo.Object.DeleteToken("abc123");

            mockRepo.Verify(r => r.DeleteToken("abc123"), Times.Once);
        }

        [Fact]
        public async Task Eliminar_Tokens_Por_Email_Debe_Correr_Sin_Errores()
        {
            var mockRepo = new Mock<IRepoRefreshToken>();
            mockRepo.Setup(r => r.DeleteTokensPorEmail("usuario@test.com")).Returns(Task.CompletedTask);

            await mockRepo.Object.DeleteTokensPorEmail("usuario@test.com");

            mockRepo.Verify(r => r.DeleteTokensPorEmail("usuario@test.com"), Times.Once);
        }

        [Fact]
        public async Task Reemplazar_Token_Debe_Correr_Sin_Errores()
        {
            var mockRepo = new Mock<IRepoRefreshToken>();
            mockRepo.Setup(r => r.ReemplazarToken(1, "nuevoHash", DateTime.UtcNow.AddHours(1)))
                    .Returns(Task.CompletedTask);

            await mockRepo.Object.ReemplazarToken(1, "nuevoHash", DateTime.UtcNow.AddHours(1));

            mockRepo.Verify(r => r.ReemplazarToken(1, "nuevoHash", It.IsAny<DateTime>()), Times.Once);
        }
    }
}