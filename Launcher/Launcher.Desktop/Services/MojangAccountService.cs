using System;
using System.Net;
using System.Threading.Tasks;
using Launcher.Desktop.Contracts;
using Launcher.Desktop.Models;
using Launcher.Desktop.Properties;
using MahApps.Metro.Controls.Dialogs;
using RestSharp;

namespace Launcher.Desktop.Services
{
    public class MojangAccountService : IAccountService
    {
        private const string AuthServerUrl = "https://authserver.mojang.com";
        private const string ApiServerUrl = "https://api.mojang.com";
        private readonly IRestClient apiClient;
        private readonly IRestClient authClient;
        private readonly string clientToken;
        private string accessToken;

        public MojangAccountService(TokenPayload tokens, IRestClient authClient, IRestClient apiClient)
        {
            clientToken = tokens.ClientToken;
            accessToken = tokens.AccessToken;
            this.authClient = authClient;
            this.apiClient = apiClient;
            authClient.BaseUrl = new Uri(AuthServerUrl);
            apiClient.BaseUrl = new Uri(ApiServerUrl);
        }

        public async Task<bool> AuthenticateAsync(string email, string password)
        {
            return await Authenticate("/authenticate", new AuthenticationPayload(email, password, clientToken));
        }

        public async Task<(bool success, bool userCanceled)> AuthenticateAsync(LoginDialogData credentials)
        {
            return credentials == null
                ? (false, true)
                : (await AuthenticateAsync(credentials.Username, credentials.Password), false);
        }

        public async Task<bool> RefreshAuthenticationAsync()
        {
            return await Authenticate("/refresh", new TokenPayload(accessToken, clientToken));
        }

        public async Task<bool> LogOffAsync()
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