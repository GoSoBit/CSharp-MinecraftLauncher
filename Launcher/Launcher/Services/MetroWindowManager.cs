using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Launcher.Contracts;
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
        private readonly MetroWindow window;

        public MetroWindowManager(MetroWindow window)
        {
            this.window = window;
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
            return await window.ShowMessageAsync(title, message, dialogStyle);
        }

        /// <summary>
        /// Shows a login dialog inside the current window.
        /// </summary>
        /// <returns>The async task with login result</returns>
        public async Task<LoginDialogData> ShowLoginAsync()
        {
            return await window.ShowLoginAsync("Log on", "Please specify your login data", new LoginDialogSettings
            {
                UsernameWatermark = "Email",
                NegativeButtonText = "Cancel",
                NegativeButtonVisibility = Visibility.Visible
            });
        }

        /// <summary>
        /// Close the current window.
        /// </summary>
        public void CloseWindow()
        {
            window.Close();
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
            return await window.ShowProgressAsync(title, message, isCancelable);
        }
    }
}