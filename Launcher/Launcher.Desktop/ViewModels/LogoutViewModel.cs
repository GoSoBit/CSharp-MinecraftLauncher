using System.Windows;
using Launcher.Desktop.Contracts;
using Launcher.Desktop.Models;
using MahApps.Metro.IconPacks;

namespace Launcher.Desktop.ViewModels
{
    public sealed class LogoutViewModel : TabBase
    {
        private readonly IAccountService accountService;
        private readonly IMetroWindowManager windowManager;

        public LogoutViewModel(IAccountService accountService, IMetroWindowManager windowManager)
        {
            this.accountService = accountService;
            this.windowManager = windowManager;
            DisplayName = "Log Off";
            DisplayIcon = PackIconMaterialKind.LogoutVariant;
            DisplayOrder = 1;
            IsShortTab = true;
        }

        protected override async void OnViewLoaded(object view)
        {
            if (!await accountService.LogOffAsync())
            {
                await windowManager.ShowMessageAsync("Error", "Could not log off");
            }

            Application.Current.Shutdown();
        }
    }
}