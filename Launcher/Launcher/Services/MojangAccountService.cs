using System;
using System.IO;
using System.Net;
using System.Net.Http;
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

        private readonly IRestClient authClient;
        private readonly IRestClient apiClient;
        private readonly string clientToken = Settings.Default.ClientToken;
        private string accessToken;

        public MojangAccountService(IRestClient authClient, IRestClient apiClient)
        {
            this.authClient = authClient;
            this.apiClient = apiClient;
            authClient.BaseUrl = new Uri(AuthServerUrl);
            apiClient.BaseUrl = new Uri(ApiServerUrl);
        }

        public bool Authenticate(string email, string password)
        {
            return Authenticate("/authenticate", new AuthenticationPayload(email, password, clientToken));
        }

        public bool Authenticate(string accessToken)
        {
            return Authenticate("/refresh", new TokenPayload(accessToken, clientToken));
        }

        public UserInfo GetUserInfo()
        {
            var request = new RestRequest("/user", Method.GET);
            request.AddHeader("Authorization", "Bearer " + accessToken);
            var result = apiClient.Execute<UserInfo>(request);

            return result.Data;
        }

        private bool Authenticate(string endpoint, IPayload payload)
        {
            try
            {
                var request = new RestRequest(endpoint, Method.POST);
                request.AddJsonBody(payload);
                var result = authClient.Execute<TokenPayload>(request);

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