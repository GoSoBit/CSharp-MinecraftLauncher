using System.Threading.Tasks;
using Launcher.Models;

namespace Launcher.Contracts
{
    public interface IAccountService
    {
        Task<bool> AuthenticateAsync(string username, string password);
        Task<bool> RefreshAuthenticationAsync();
        Task<bool> LogOffAsync();
        Task<UserInfo> GetUserInfoAsync();
    }
}