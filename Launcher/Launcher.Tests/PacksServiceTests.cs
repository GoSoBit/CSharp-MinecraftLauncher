using System.Collections.Generic;
using System.Threading.Tasks;
using Launcher.Desktop.Models;
using Launcher.Desktop.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;

namespace Launcher.Tests
{
    [TestClass]
    public class PacksServiceTests
    {
        [TestMethod]
        public async Task ShouldGetAvailablePacks()
        {
            var result = new List<Pack>
            {
                new Pack("17w15a", "snapshot", "17w15a.json"),
                new Pack("17w14a", "snapshot", "17w14a.json"),
                new Pack("17w13b", "snapshot", "17w13b.json"),
                new Pack("17w06a", "snapshot", "17w06a.json")
            };
            var restClientMock = new Mock<IRestClient>();
            restClientMock.Setup(x => x.ExecuteTaskAsync<List<Pack>>(It.IsAny<IRestRequest>()))
                .ReturnsAsync(new RestResponse<List<Pack>> { Data = result });
            var service = new PacksService(restClientMock.Object);

            var packs = await service.GetAvailablePacksAsync();

            Assert.IsNotNull(result);
            Assert.AreSame(result, packs);
            restClientMock.Verify(x => x.ExecuteTaskAsync<List<Pack>>(It.IsAny<IRestRequest>()), Times.Once);
        }
    }
}