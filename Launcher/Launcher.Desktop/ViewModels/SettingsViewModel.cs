using Launcher.Desktop.Models;
using MahApps.Metro.IconPacks;

namespace Launcher.Desktop.ViewModels
{
    public sealed class SettingsViewModel : TabBase
    {
        public SettingsViewModel()
        {
            DisplayName = "Settings";
            DisplayIcon = PackIconMaterialKind.Settings;
            DisplayOrder = 3;
        }
    }
}