using System.Net;
using System.Threading.Tasks;
using Launcher.Models;
using Launcher.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using RestSharp;
using RestSharp.Serializers;

namespace Launcher.Tests
{
    [TestClass]
    public class MojangAccountServiceTests
    {
        private readonly IFixture fixture = new Fixture();
        private readonly AuthenticationPayload validAuthPayload;
        private readonly TokenPayload validTokenPayload;

        public MojangAccountServiceTests()
        {
            validTokenPayload = new TokenPayload(fixture.Create<string>(), string.Empty);
            validAuthPayload = new AuthenticationPayload(fixture.Create<string>(), fixture.Create<string>(), string.Empty);
        }

        [TestMethod]
        public async Task ShouldAuthenticateOrRefuseUsingCredentials()
        {
            var restClientMock = SetupAuthRestClientMock(validAuthPayload);
            var service = new MojangAccountService(restClientMock.Object, restClientMock.Object);

            bool resultValid = await service.AuthenticateAsync(validAuthPayload.Username, validAuthPayload.Password);
            bool resultInvalid1 = await service.AuthenticateAsync(null, null);
            bool resultInvalid2 = await service.AuthenticateAsync(fixture.Create<string>(), fixture.Create<string>());

            Assert.IsTrue(resultValid);
            Assert.IsFalse(resultInvalid1);
            Assert.IsFalse(resultInvalid2);
        }

        [TestMethod]
        public async Task ShouldAuthenticateOrRefuseUsingRefreshing()
        {
            var restClientMock = SetupAuthRestClientMock(validTokenPayload);
            var service = new MojangAccountService(restClientMock.Object, restClientMock.Object);

            bool resultValid = await service.AuthenticateAsync(validTokenPayload.AccessToken);
            bool resultInvalid1 = await service.AuthenticateAsync(null);
            bool resultInvalid2 = await service.AuthenticateAsync(fixture.Create<string>());

            Assert.IsTrue(resultValid);
            Assert.IsFalse(resultInvalid1);
            Assert.IsFalse(resultInvalid2);
        }

        [TestMethod]
        public async Task ShouldLogOff()
        {
            var restClientMock = new Mock<IRestClient>();
            restClientMock.Setup(x => x.ExecuteTaskAsync(It.IsAny<IRestRequest>())).ReturnsAsync(new RestResponse { StatusCode = HttpStatusCode.NoContent});

            var service = new MojangAccountService(restClientMock.Object, restClientMock.Object);
            var result = await service.LogOff();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task ShouldGetUserInfo()
        {
            var validUserInfo = fixture.Create<UserInfo>();
            var restClientMock = new Mock<IRestClient>();
            restClientMock.
                Setup(x => x.ExecuteTaskAsync<UserInfo>(It.IsAny<IRestRequest>()))
                .ReturnsAsync(new RestResponse<UserInfo> { Data = validUserInfo });

            var service = new MojangAccountService(restClientMock.Object, restClientMock.Object);
            var result = await service.GetUserInfoAsync();
            
            Assert.AreEqual(validUserInfo, result);
        }

        private IMock<IRestClient> SetupAuthRestClientMock(object body)
        {
            var mock = new Mock<IRestClient>();
            mock
                .Setup(x => x.ExecuteTaskAsync<TokenPayload>(It.Is<IRestRequest>(r => BodyEquals(r, body))))
                .ReturnsAsync(new RestResponse<TokenPayload> { StatusCode = HttpStatusCode.OK, Data = validTokenPayload });

            return mock;
        }

        private bool BodyEquals(IRestRequest request, object value)
        {
            var serializer = new JsonSerializer();
            string valueJson = serializer.Serialize(value);
            string bodyJson = GetBody(request);

            return bodyJson == valueJson;
        }

        private string GetBody(IRestRequest request)
        {
            return request.Parameters[0].Value.ToString();
        }
    }
}