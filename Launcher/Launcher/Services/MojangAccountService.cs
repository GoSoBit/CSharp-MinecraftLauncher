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
        private const string ApiUrl = "https://authserver.mojang.com";

        private readonly IRestClient client;
        private readonly string clientToken = Settings.Default.ClientToken;
        private string accessToken;

        public MojangAccountService(IRestClient client)
        {
            this.client = client;
            client.BaseUrl = new Uri(ApiUrl);
        }

        public bool Authenticate(string email, string password)
        {
            return Authenticate("/authenticate", new AuthenticationPayload(email, password, clientToken));
        }

        public bool Authenticate(string accessToken)
        {
            return Authenticate("/refresh", new TokenPayload(accessToken, clientToken));
        }

        private bool Authenticate(string endpoint, object payload)
        {
            try
            {
                var request = new RestRequest(endpoint, Method.POST);
                request.AddJsonBody(payload);
                var result = client.Execute<TokenPayload>(request);

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