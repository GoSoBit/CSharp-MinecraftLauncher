using Launcher.Models;
using MahApps.Metro.IconPacks;

namespace Launcher.ViewModels
{
    public sealed class LogoutViewModel : TabBase
    {
        public LogoutViewModel()
        {
            DisplayName = "Log Off";
            DisplayIcon = PackIconMaterialKind.LogoutVariant;
            DisplayOrder = 1;
            IsShortTab = true;
        }
    }
}