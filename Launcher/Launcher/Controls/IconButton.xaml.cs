using System.Windows;
using MahApps.Metro.IconPacks;

namespace Launcher.Controls
{
    public partial class IconButton
    {
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.RegisterAttached("Icon", typeof(PackIconModernKind), typeof(IconButton),
                new FrameworkPropertyMetadata(PackIconModernKind.None, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public IconButton()
        {
            InitializeComponent();
            DataContext = this;
        }

        public PackIconModernKind Icon
        {
            get => (PackIconModernKind)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
    }
}