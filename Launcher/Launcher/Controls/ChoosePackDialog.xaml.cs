using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
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

        private async void PacksBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var result = PacksBox.SelectedItems[0] as Pack;
            await dialogCoordinator.HideMetroDialogAsync(context, this);
            ClosedDialog?.Invoke(this, result);
        }
    }
}