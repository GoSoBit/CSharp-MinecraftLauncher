using System.Collections.Generic;
using System.Threading.Tasks;
using Launcher.Contracts;
using Launcher.Extensions;
using Launcher.Models;
using Launcher.Properties;
using Launcher.Services;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.IconPacks;

namespace Launcher.ViewModels
{
    public sealed class PacksViewModel : TabBase
    {
        private readonly IPacksService packsService;
        private readonly IMetroWindowManager windowManager;
        private readonly XmlSerializationService xmlService = new XmlSerializationService();
        private IEnumerable<Pack> allPacks;
        private IEnumerable<Pack> packs;

        public PacksViewModel(IMetroWindowManager windowManager, IPacksService packsService)
        {
            this.windowManager = windowManager;
            this.packsService = packsService;
            DisplayName = "Packs";
            DisplayIcon = PackIconMaterialKind.Package;
            DisplayOrder = 2;
            LoadPacks();
        }

        public IEnumerable<Pack> Packs
        {
            get => packs;
            set => this.Set(out packs, value);
        }

        public async Task OpenPackManagement()
        {
            await OpenPackManagement(new Pack());
        }

        public async Task OpenPackManagement(Pack pack)
        {
            await EnsureAllAvailablePackAreLoaded();

            IPackDialog dialog = await windowManager.ShowPackManagementDialogAsync(allPacks, pack);
            dialog.ClosedDialog += (s, result) =>
            {
                if (result == null) return;
                ApplyPack(result);
            };
        }

        public async Task DeleteAllPacks()
        {
            MessageDialogResult result = await windowManager.ShowMessageAsync("Deleting", "Are you sure?", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                Packs = null;
                Settings.Default.PacksListXml = null;
                Settings.Default.Save();
            }
        }

        private void LoadPacks()
        {
            string xml = Settings.Default.PacksListXml;
            if (!string.IsNullOrEmpty(xml))
            {
                var list = xmlService.Deserialize<List<Pack>>(xml);
                Packs = list.ToObservable();
            }
        }

        private async Task EnsureAllAvailablePackAreLoaded()
        {
            if (allPacks == null)
            {
                await windowManager.ShowProgressAndDoAsync(async ()
                    => allPacks = await packsService.GetAvailablePacksAsync());
            }
        }

        private void ApplyPack(Pack pack)
        {
            string savedXml = Settings.Default.PacksListXml;
            var cachedList = string.IsNullOrEmpty(savedXml) ? new List<Pack>() : xmlService.Deserialize<List<Pack>>(savedXml);
            cachedList.AddOrUpdate(pack, x => x.Guid == pack.Guid);
            Packs = cachedList;

            string xmlToSave = xmlService.Serialize(cachedList);
            Settings.Default.PacksListXml = xmlToSave;
            Settings.Default.Save();
        }
    }
}