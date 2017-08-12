using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Launcher.Desktop.Contracts;
using Launcher.Desktop.Models;
using Launcher.Desktop.Properties;
using RestSharp;
using RestSharp.Deserializers;

namespace Launcher.Desktop.Services
{
    public class PacksService : IPacksService
    {
        private const string ApiServerUrl = "https://launchermeta.mojang.com/mc/game";
        private readonly IRestClient client;
        private readonly XmlSerializationService xmlService = new XmlSerializationService();

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

        public IEnumerable<Pack> GetSavedPacks()
        {
            string xml = Settings.Default.PacksListXml;
            IEnumerable<Pack> list =  new List<Pack>();

            if (!string.IsNullOrEmpty(xml))
            {
                list = xmlService.Deserialize<List<Pack>>(xml);
            }

            return list;
        }
    }
}