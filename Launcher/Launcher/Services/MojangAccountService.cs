using System;
using System.Net;
using System.Threading.Tasks;
using Launcher.Contracts;
using Launcher.Models;
using Launcher.Properties;
using RestSharp;

namespace Launcher.Services
{
    public class MojangAccountService : IAccountService
    {
        private const string AuthServerUrl = "https://authserver.mojang.com";
        private const string ApiServerUrl = "https://api.mojang.com";
        private readonly IRestClient apiClient;
        private readonly IRestClient authClient;
        private readonly string clientToken = Settings.Default.ClientToken;
        private string accessToken;

        public MojangAccountService(IRestClient authClient, IRestClient apiClient)
        {
            this.authClient = authClient;
            this.apiClient = apiClient;
            authClient.BaseUrl = new Uri(AuthServerUrl);
            apiClient.BaseUrl = new Uri(ApiServerUrl);
        }

        public async Task<bool> AuthenticateAsync(string email, string password)
        {
            return await Authenticate("/authenticate", new AuthenticationPayload(email, password, clientToken));
        }

        public async Task<bool> AuthenticateAsync(string accessToken)
        {
            return await Authenticate("/refresh", new TokenPayload(accessToken, clientToken));
        }

        public async Task<bool> LogOff()
        {
            try
            {
                var request = new RestRequest("/invalidate", Method.POST);
                request.AddJsonBody(new TokenPayload(accessToken, clientToken));
                IRestResponse result = await authClient.ExecuteTaskAsync(request);

                Settings.Default.AccessToken = "";
                Settings.Default.Save();

                return result.StatusCode == HttpStatusCode.NoContent;
            }
            catch
            {
                return false;
            }
        }

        public async Task<UserInfo> GetUserInfoAsync()
        {
            var request = new RestRequest("/user", Method.GET);
            request.AddHeader("Authorization", "Bearer " + accessToken);
            var result = await apiClient.ExecuteTaskAsync<UserInfo>(request);

            return result.Data;
        }

        private async Task<bool> Authenticate(string endpoint, IPayload payload)
        {
            try
            {
                var request = new RestRequest(endpoint, Method.POST);
                request.AddJsonBody(payload);
                var result = await authClient.ExecuteTaskAsync<TokenPayload>(request);

                accessToken = result.Data.AccessToken;
                Settings.Default.AccessToken = accessToken;
                Settings.Default.Save();

                return result.StatusCode == HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }
    }
}