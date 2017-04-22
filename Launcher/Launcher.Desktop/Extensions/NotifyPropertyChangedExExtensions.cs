using System.Runtime.CompilerServices;
using Caliburn.Micro;

namespace Launcher.Desktop.Extensions
{
    public static class NotifyPropertyChangedExExtensions
    {
        public static void Set<T>(this INotifyPropertyChangedEx viewModel, out T field, T value, [CallerMemberName] string propertyName = null)
        {
            field = value;
            if (propertyName != null)
            {
                viewModel.NotifyOfPropertyChange(propertyName);
            }
        }
    }
}