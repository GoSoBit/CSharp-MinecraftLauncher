using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Launcher.Contracts;
using Launcher.Controls;
using Launcher.Models;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Launcher.Services
{
    /// <summary>
    /// An implementation of <see cref="IMetroWindowManager" />. Basically it's extended <see cref="WindowManager" />.
    /// <para>
    /// Window manager adapted to <see cref="MetroWindow" />. The main window must be <see cref="MetroWindow" /> or it
    /// will throw an exception.
    /// </para>
    /// </summary>
    public class MetroWindowManager : WindowManager, IMetroWindowManager
    {
        private readonly IDialogCoordinator dialogCoordinator;
        private IShell shell;

        public MetroWindowManager(IDialogCoordinator dialogCoordinator, MetroWindow window)
        {
            this.dialogCoordinator = dialogCoordinator;
            window.Loaded += (s, e) => shell = window.DataContext as IShell;
        }

        /// <summary>
        /// Shows a message dialog inside the current window.
        /// </summary>
        /// <param name="title">The title</param>
        /// <param name="message">The message text</param>
        /// <param name="dialogStyle">The additional style</param>
        /// <param name="settings">The additional settings</param>
        /// <returns>The async task with dialog result</returns>
        public async Task<MessageDialogResult> ShowMessageAsync(string title, string message,
            MessageDialogStyle dialogStyle = MessageDialogStyle.Affirmative, MetroDialogSettings settings = null)
        {
            return await dialogCoordinator.ShowMessageAsync(shell, title, message, dialogStyle, settings);
        }

        /// <summary>
        /// Shows a login dialog inside the current window.
        /// </summary>
        /// <returns>The async task with login result</returns>
        public async Task<LoginDialogData> ShowLoginAsync()
        {
            return await dialogCoordinator.ShowLoginAsync(shell, "Log on", "Please specify your login data", new LoginDialogSettings
            {
                UsernameWatermark = "Email",
                NegativeButtonText = "Cancel",
                NegativeButtonVisibility = Visibility.Visible
            });
        }

        /// <summary>
        /// Shows a progress dialog inside the current window.
        /// </summary>
        /// <param name="title">The dialog title</param>
        /// <param name="message">The message</param>
        /// <param name="isCancelable">Indicates if can cancel</param>
        /// <returns>The dialog controller</returns>
        public async Task<ProgressDialogController> ShowProgressAsync(string title, string message, bool isCancelable = false)
        {
            return await dialogCoordinator.ShowProgressAsync(shell, title, message, isCancelable);
        }

        /// <summary>
        /// Shows a <see cref="PackManagementDialog" /> dialog inside the current window.
        /// </summary>
        /// <param name="packs">All packs available to choose from.</param>
        /// <returns>The dialog</returns>
        public async Task<IPackDialog> ShowPackManagementDialogAsync(IEnumerable<Pack> packs, Pack packToEdit)
        {
            await dialogCoordinator.ShowMetroDialogAsync(shell, new PackManagementDialog(packs, dialogCoordinator, shell, packToEdit));
            IPackDialog dialog = await dialogCoordinator.GetCurrentDialogAsync<PackManagementDialog>(shell);
            return dialog;
        }

        /// <summary>
        /// Shows a indeterminate progress dialog inside the current window.
        /// </summary>
        /// <param name="action">Async action to do while showing the progress.</param>
        /// <returns>The async task.</returns>
        public async Task ShowProgressAndDoAsync(Func<Task> action)
        {
            ProgressDialogController progress = await dialogCoordinator.ShowProgressAsync(shell, "Please wait", "Loading data");
            progress?.SetIndeterminate();
            await action();
            Task closeAsync = progress?.CloseAsync();
            if (closeAsync != null) await closeAsync;
        }
    }
}