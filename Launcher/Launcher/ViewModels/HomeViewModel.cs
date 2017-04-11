using Launcher.Models;
using MahApps.Metro.IconPacks;

namespace Launcher.ViewModels
{
    public sealed class HomeViewModel : TabBase
    {
        public HomeViewModel()
        {
            DisplayName = "Home";
            DisplayIcon = PackIconMaterialKind.HomeOutline;
            DisplayOrder = 1;
            IsHomeTab = true;
        }
    }
}