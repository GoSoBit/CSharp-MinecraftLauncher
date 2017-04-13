using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Launcher.Contracts;

namespace Launcher.Models
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
