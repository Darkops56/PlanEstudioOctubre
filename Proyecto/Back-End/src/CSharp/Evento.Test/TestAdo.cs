using System.Data;
using Evento.Core.Services.Repo;
using Moq;
using Xunit;

namespace Evento.Test
{
    public class TestAdo
    {
        [Fact]
        public void GetDbConnection_ShouldReturnMockedConnection()
        {
            var mockAdo = new Mock<IAdo>();

            var mockConnection = new Mock<IDbConnection>();

            mockAdo.Setup(a => a.GetDbConnection()).Returns(mockConnection.Object);

            var result = mockAdo.Object.GetDbConnection();

            Assert.NotNull(result);

            mockAdo.Verify(a => a.GetDbConnection(), Times.Once);
        }
    }
}