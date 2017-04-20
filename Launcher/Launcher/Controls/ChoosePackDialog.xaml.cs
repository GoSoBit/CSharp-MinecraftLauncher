using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Launcher.Contracts;
using Launcher.Models;
using MahApps.Metro.Controls.Dialogs;

namespace Launcher.Controls
{
    public partial class ChoosePackDialog : IPackDialog
    {
        private readonly object context;
        private readonly IDialogCoordinator dialogCoordinator;

        public ChoosePackDialog(IEnumerable<Pack> packs, IDialogCoordinator dialogCoordinator, object context)
        {
            this.dialogCoordinator = dialogCoordinator;
            this.context = context;
            InitializeComponent();
            PacksBox.ItemsSource = packs.Where(x => x.Type == "release");
        }

        public event EventHandler<Pack> ClosedDialog;

        private async void ButtonOK_OnClick(object sender, RoutedEventArgs e)
        {
            var result = PacksBox.SelectedItem as Pack;
            await CloseDialog(result);
        }

        private async void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            await CloseDialog(null);
        }

        private async Task CloseDialog(Pack result)
        {
            await dialogCoordinator.HideMetroDialogAsync(context, this);
            ClosedDialog?.Invoke(this, result);
        }
    }
}