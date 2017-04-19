using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Launcher.Models;
using MahApps.Metro.Controls.Dialogs;

namespace Launcher.Contracts
{
    /// <summary>
    /// A window manager adapted to <see cref="MahApps.Metro.Controls.MetroWindow" />.
    /// </summary>
    public interface IMetroWindowManager
    {
        Task<MessageDialogResult> ShowMessageAsync(string title, string message, MessageDialogStyle dialogStyle = MessageDialogStyle.Affirmative,
            MetroDialogSettings settings = null);

        Task<LoginDialogData> ShowLoginAsync();
        Task<ProgressDialogController> ShowProgressAsync(string title, string message, bool isCancelable = false);
        Task<IPackDialog> ShowChoosePackDialogAsync(IEnumerable<Pack> packs);
        Task ShowProgressAndDoAsync(Func<Task> action);
    }
}