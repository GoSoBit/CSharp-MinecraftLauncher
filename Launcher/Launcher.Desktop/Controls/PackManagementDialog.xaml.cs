using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Launcher.Desktop.Contracts;
using Launcher.Desktop.Models;
using MahApps.Metro.Controls.Dialogs;

namespace Launcher.Desktop.Controls
{
    public partial class PackManagementDialog : IPackDialog
    {
        private readonly object context;
        private readonly Pack packToEdit;
        private readonly IEnumerable<Pack> packs;
        private readonly IDialogCoordinator dialogCoordinator;

        public PackManagementDialog(IEnumerable<Pack> packs, IDialogCoordinator dialogCoordinator, object context, Pack packToEdit)
        {
            this.packs = packs;
            this.dialogCoordinator = dialogCoordinator;
            this.context = context;
            this.packToEdit = packToEdit;
            InitializeComponent();
            PacksBox.ItemsSource = packs.Where(x => x.Type == "release");
            PacksBox.SelectedItem = packs.FirstOrDefault(x => x.Id == packToEdit.Id);
        }

        public event EventHandler<Pack> ClosedDialog;

        private async void ButtonOK_OnClick(object sender, RoutedEventArgs e)
        {
            string selectedId = PacksBox.Text;
            Pack result = packs.FirstOrDefault(x => x.Id == selectedId);
            result.Guid = packToEdit.Guid;
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