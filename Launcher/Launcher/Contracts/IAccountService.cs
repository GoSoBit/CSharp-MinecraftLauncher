using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Launcher.Models;

namespace Launcher.Contracts
{
    public interface IAccountService
    {
        bool Authenticate(string username, string password);
        bool Authenticate(string accessToken);
        UserInfo GetUserInfo();
    }
}
