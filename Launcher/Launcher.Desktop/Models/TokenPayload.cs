using Launcher.Desktop.Contracts;

namespace Launcher.Desktop.Models
{
    public class TokenPayload : IPayload
    {
        public TokenPayload()
        {
        }

        public TokenPayload(string accessToken, string clientToken)
        {
            AccessToken = accessToken;
            ClientToken = clientToken;
        }

        public string AccessToken { get; set; }
        public string ClientToken { get; set; }
    }
}
