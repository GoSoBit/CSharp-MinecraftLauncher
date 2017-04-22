using System.Net;
using System.Threading.Tasks;
using Launcher.Desktop.Models;
using Launcher.Desktop.Services;
using Launcher.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using RestSharp;

namespace Launcher.Tests
{
    [TestClass]
    public class MojangAccountServiceTests
    {
        private readonly IFixture fixture = new Fixture();
        private readonly TokenPayload invalidTokenPayload;
        private readonly AuthenticationPayload validAuthPayload;
        private readonly TokenPayload validTokenPayload;

        public MojangAccountServiceTests()
        {
            validTokenPayload = fixture.Create<TokenPayload>();
            invalidTokenPayload = fixture.Create<TokenPayload>();
            validAuthPayload = new AuthenticationPayload(fixture.Create<string>(), fixture.Create<string>(), validTokenPayload.ClientToken);
        }

        [TestMethod]
        public async Task ShouldAuthenticateOrRefuseUsingCredentials()
        {
            var restClientMock = SetupAuthRestClientMock(validAuthPayload);
            var service = new MojangAccountService(validTokenPayload, restClientMock.Object, restClientMock.Object);

            bool validResult = await service.AuthenticateAsync(validAuthPayload.Username, validAuthPayload.Password);
            bool invalidResult1 = await service.AuthenticateAsync(null, null);
            bool invalidResult2 = await service.AuthenticateAsync(fixture.Create<string>(), fixture.Create<string>());

            Assert.IsTrue(validResult);
            Assert.IsFalse(invalidResult1);
            Assert.IsFalse(invalidResult2);
        }

        [TestMethod]
        public async Task ShouldAuthenticateOrRefuseUsingRefreshing()
        {
            var restClientMock = SetupAuthRestClientMock(validTokenPayload);
            var validService = new MojangAccountService(validTokenPayload, restClientMock.Object, restClientMock.Object);
            var invalidService = new MojangAccountService(invalidTokenPayload, restClientMock.Object, restClientMock.Object);

            bool validResult1 = await validService.RefreshAuthenticationAsync();
            bool validResult2 = await validService.RefreshAuthenticationAsync();
            bool invalidResult = await invalidService.RefreshAuthenticationAsync();

            Assert.IsTrue(validResult1);
            Assert.IsTrue(validResult2);
            Assert.IsFalse(invalidResult);
        }

        [TestMethod]
        public async Task ShouldLogOff()
        {
            var restClientMock = new Mock<IRestClient>();
            restClientMock.Setup(x => x.ExecuteTaskAsync(It.IsAny<IRestRequest>()))
                .ReturnsAsync(new RestResponse { StatusCode = HttpStatusCode.NoContent });

            var service = new MojangAccountService(validTokenPayload, restClientMock.Object, restClientMock.Object);
            bool result = await service.LogOffAsync();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task ShouldGetUserInfo()
        {
            var validUserInfo = fixture.Create<UserInfo>();
            var restClientMock = new Mock<IRestClient>();
            restClientMock.Setup(x => x.ExecuteTaskAsync<UserInfo>(It.IsAny<IRestRequest>()))
                .ReturnsAsync(new RestResponse<UserInfo> { Data = validUserInfo });

            var service = new MojangAccountService(validTokenPayload, restClientMock.Object, restClientMock.Object);
            UserInfo result = await service.GetUserInfoAsync();

            Assert.AreEqual(validUserInfo, result);
        }

        private IMock<IRestClient> SetupAuthRestClientMock(object body)
        {
            var mock = new Mock<IRestClient>();
            mock
                .Setup(x => x.ExecuteTaskAsync<TokenPayload>(It.Is<IRestRequest>(r => r.JsonBodyEquals(body))))
                .ReturnsAsync(new RestResponse<TokenPayload> { StatusCode = HttpStatusCode.OK, Data = validTokenPayload });

            return mock;
        }
    }
}