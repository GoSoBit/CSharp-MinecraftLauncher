using Launcher.Contracts;

namespace Launcher.Models
{
    public class AuthenticationPayload : IPayload
    {
        public AuthenticationPayload()
        {
        }

        public AuthenticationPayload(string username, string password, string clientToken)
        {
            Username = username;
            Password = password;
            ClientToken = clientToken;
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string ClientToken { get; set; }
    }
}