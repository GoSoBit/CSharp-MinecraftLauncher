using Launcher.Models;
using MahApps.Metro.IconPacks;

namespace Launcher.ViewModels
{
    public sealed class PacksViewModel : TabBase
    {
        public PacksViewModel()
        {
            DisplayName = "Packs";
            DisplayIcon = PackIconMaterialKind.Package;
            DisplayOrder = 2;
        }
    }
}