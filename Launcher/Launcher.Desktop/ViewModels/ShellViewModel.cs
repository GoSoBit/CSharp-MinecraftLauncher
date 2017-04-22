using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using Launcher.Desktop.Contracts;
using Launcher.Desktop.Properties;
using Launcher.Desktop.Extensions;

namespace Launcher.Desktop.ViewModels
{
    public sealed class ShellViewModel : Conductor<ITab>, IShell
    {
        private readonly IAccountService accountService;
        private readonly IMetroWindowManager windowManager;
        private IEnumerable<ITab> tabs;

        public ShellViewModel(IMetroWindowManager windowManager, IAccountService accountService, IEnumerable<ITab> tabs)
        {
            this.windowManager = windowManager;
            this.accountService = accountService;
            Tabs = tabs.OrderByDescending(tab => tab.DisplayOrder.HasValue).ThenBy(tab => tab.DisplayOrder);
            DisplayName = "Minecraft Launcher";
        }

        /// <summary>
        /// Binded to the tab control in the window
        /// </summary>
        public IEnumerable<ITab> Tabs
        {
            get => tabs;
            set => this.Set(out tabs, value);
        }

        /// <summary>
        /// Invoked when the user changes a tab in the tab control.
        /// </summary>
        /// <param name="actionContext">The action execution context</param>
        public void OnTabSwitched(ActionExecutionContext actionContext)
        {
            var selectionArgs = (SelectionChangedEventArgs)actionContext.EventArgs;

            ExecuteActionIfTabIsPresent(selectionArgs.AddedItems, ActivateItem);
            ExecuteActionIfTabIsPresent(selectionArgs.RemovedItems, DeactivateItem);
        }

        /// <summary>
        /// Called when an attached view's Loaded event fires.
        /// Application tries to refresh authentication. If fails, asks for the login data.
        /// </summary>
        protected override async void OnViewLoaded(object view)
        {
            bool canceled = false;
            bool authenticated = await accountService.RefreshAuthenticationAsync();
            if (!authenticated)
            {
                var (success, userCanceled) = await accountService.AuthenticateAsync(await windowManager.ShowLoginAsync());
                authenticated = success;
                canceled = userCanceled;
            }

            if (canceled)
            {
                Application.Current.Shutdown();
            }

            if (!authenticated)
            {
                Settings.Default.AccessToken = "";
                Settings.Default.Save();
                await windowManager.ShowMessageAsync("Error", "Could not log on using specified login data");
                OnViewLoaded(view);
            }

            ActivateHomeTab();
        }

        private void ActivateHomeTab()
        {
            ITab homeTab = Tabs.FirstOrDefault(tab => tab.IsHomeTab);
            ActivateItem(homeTab ?? Tabs.First());
        }

        private void DeactivateItem(ITab obj)
        {
            DeactivateItem(obj, false);
        }

        private static void ExecuteActionIfTabIsPresent(IEnumerable list, Action<ITab> action)
        {
            ITab tab = list.OfType<ITab>().FirstOrDefault();
            if (tab != null)
            {
                action.Invoke(tab);
            }
        }
    }
}