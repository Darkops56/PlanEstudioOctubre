using System.Data;
using Evento.Core.Services.Repo;
using Moq;
using Xunit;

namespace Evento.Test
{
    public class AdoTests
    {
        [Fact]
        public void GetDbConnection_ShouldReturnMockedConnection()
        {
            // 1. Crear el mock de IAdo
            var mockAdo = new Mock<IAdo>();

            // 2. Crear un mock de IDbConnection para devolver
            var mockConnection = new Mock<IDbConnection>();

            // 3. Configurar el mock para que GetDbConnection() devuelva el mockConnection
            mockAdo.Setup(a => a.GetDbConnection()).Returns(mockConnection.Object);

            // 4. Llamar al método
            var result = mockAdo.Object.GetDbConnection();

            // 5. Verificar que el resultado no sea null
            Assert.NotNull(result);

            // 6. Verificar que se llamó exactamente una vez al método
            mockAdo.Verify(a => a.GetDbConnection(), Times.Once);
        }
    }
}