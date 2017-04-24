using Launcher.Desktop.Extensions;
using Launcher.Desktop.Models;
using Launcher.Desktop.Properties;
using MahApps.Metro.IconPacks;

namespace Launcher.Desktop.ViewModels
{
    public sealed class SettingsViewModel : TabBase
    {
        private bool allowSnapshots;
        private bool allowBetaAndAlpha;

        public SettingsViewModel()
        {
            DisplayName = "Settings";
            DisplayIcon = PackIconMaterialKind.Settings;
            DisplayOrder = 3;
        }

        public bool AllowSnapshots
        {
            get => allowSnapshots;
            set => this.Set(out allowSnapshots, value);
        }

        public bool AllowBetaAndAlpha
        {
            get => allowBetaAndAlpha;
            set => this.Set(out allowBetaAndAlpha, value);
        }

        protected override void OnActivate()
        {
            AllowSnapshots = Settings.Default.AllowSnapshots;
            AllowBetaAndAlpha = Settings.Default.AllowBetaAndAlpha;
        }

        protected override void OnDeactivate(bool close)
        {
            Settings.Default.AllowSnapshots = AllowSnapshots;
            Settings.Default.AllowBetaAndAlpha = AllowBetaAndAlpha;
            Settings.Default.Save();
        }
    }
}