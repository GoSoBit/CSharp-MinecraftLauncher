using RestSharp;
using RestSharp.Serializers;

namespace Launcher.Tests.Helpers
{
    public static class RestRequestExtensions
    {
        public static bool JsonBodyEquals(this IRestRequest request, object toCompare)
        {
            var (body, value) = GetJson(request, toCompare);
            return body.Equals(value);
        }

        public static bool JsonBodyContains(this IRestRequest request, object toCompare)
        {
            var(body, value) = GetJson(request, toCompare);
            return body.Contains(value);
        }

        private static (string body, string value) GetJson(IRestRequest request, object value)
        {
            var serializer = new JsonSerializer();
            string valueJson = serializer.Serialize(value);
            string bodyJson = request.Parameters[0].Value.ToString();

            return (bodyJson, valueJson);
        }
    }
}