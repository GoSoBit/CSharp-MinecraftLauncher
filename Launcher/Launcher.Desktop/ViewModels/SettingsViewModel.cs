using Launcher.Desktop.Contracts;
using Launcher.Desktop.Models;
using Launcher.Desktop.Properties;
using MahApps.Metro.IconPacks;

namespace Launcher.Desktop.ViewModels
{
    public sealed class SettingsViewModel : TabBase
    {
        private readonly IMetroWindowManager windowManager;

        public SettingsViewModel(IMetroWindowManager windowManager)
        {
            this.windowManager = windowManager;
            DisplayName = "Settings";
            DisplayIcon = PackIconMaterialKind.Settings;
            DisplayOrder = 3;
        }

        public bool AllowSnapshots
        {
            get => LoadSetting<bool>(nameof(AllowSnapshots));
            set => SaveSetting(nameof(AllowSnapshots), value);
        }

        public bool AllowBetaAndAlpha
        {
            get => LoadSetting<bool>(nameof(AllowBetaAndAlpha));
            set => SaveSetting(nameof(AllowBetaAndAlpha), value);
        }

        public string JavaBinDirectory
        {
            get => LoadSetting<string>(nameof(JavaBinDirectory));
            set => SaveSetting(nameof(JavaBinDirectory), value);
        }

        public short Memory
        {
            get => LoadSetting<short>(nameof(Memory));
            set => SaveSetting(nameof(Memory), value);
        }

        public void BrowseJavaBinDirectory()
        {
            string result = windowManager.ShowDirectoryBrowseDialog(JavaBinDirectory);
            JavaBinDirectory = result;
        }

        protected override void OnDeactivate(bool close)
        {
            Settings.Default.Save();
        }

        private void SaveSetting(string settingName, object value)
        {
            Settings.Default[settingName] = value;
            NotifyOfPropertyChange(settingName);
        }

        private T LoadSetting<T>(string settingName)
        {
            return (T)Settings.Default[settingName];
        }
    }
}