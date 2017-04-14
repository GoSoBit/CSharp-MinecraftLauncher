using System.Threading.Tasks;
using Launcher.Models;

namespace Launcher.Contracts
{
    public interface IAccountService
    {
        Task<bool> AuthenticateAsync(string username, string password);
        Task<bool> AuthenticateAsync(string accessToken);
        Task<bool> LogOff();
        Task<UserInfo> GetUserInfoAsync();
    }
}