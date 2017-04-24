using MahApps.Metro.IconPacks;

namespace Launcher.Desktop.Contracts
{
    /// <summary>
    /// Implemented by a view model of a tab-like view.
    /// For example, can be used as a <seealso cref="System.Windows.Controls.TabControl" /> item.
    /// </summary>
    public interface ITab
    {
        string DisplayName { get; set; }
        PackIconMaterialKind DisplayIcon { get; }
        int? DisplayOrder { get; }
        bool IsHomeTab { get; }
    }
}