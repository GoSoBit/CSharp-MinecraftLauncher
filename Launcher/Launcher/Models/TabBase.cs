using Caliburn.Micro;
using Launcher.Contracts;
using MahApps.Metro.IconPacks;

namespace Launcher.Models
{
    /// <summary>
    /// The base class for all tab-like view models.
    /// Properties should be initialized in the constructor.
    /// </summary>
    public abstract class TabBase : Screen, ITab
    {
        public PackIconMaterialKind DisplayIcon { get; protected set; }
        public bool IsShortTab { get; protected set; }
        public int? DisplayOrder { get; protected set; }
        public bool IsHomeTab { get; protected set; }
    }
}