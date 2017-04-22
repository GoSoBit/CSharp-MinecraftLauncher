using System.Threading.Tasks;
using Launcher.Desktop.Models;
using MahApps.Metro.Controls.Dialogs;

namespace Launcher.Desktop.Contracts
{
    public interface IAccountService
    {
        Task<bool> AuthenticateAsync(string username, string password);
        Task<(bool success, bool userCanceled)> AuthenticateAsync(LoginDialogData credentials);
        Task<bool> RefreshAuthenticationAsync();
        Task<bool> LogOffAsync();
        Task<UserInfo> GetUserInfoAsync();
    }
}