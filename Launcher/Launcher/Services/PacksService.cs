using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Launcher.Contracts;
using Launcher.Models;
using RestSharp;
using RestSharp.Deserializers;

namespace Launcher.Services
{
    public class PacksService : IPacksService
    {
        private const string ApiServerUrl = "https://launchermeta.mojang.com/mc/game";
        private readonly IRestClient client;

        public PacksService(IRestClient client)
        {
            this.client = client;
            client.BaseUrl = new Uri(ApiServerUrl);
            //This api returns a json in application/octet-stream content type
            client.AddHandler("application/octet-stream", new JsonDeserializer());
        }

        public async Task<IEnumerable<Pack>> GetAvailablePacksAsync()
        {
            var request = new RestRequest("version_manifest.json") { RootElement = "versions" };
            var result = await client.ExecuteTaskAsync<List<Pack>>(request);
            return result.Data;
        }
    }
}