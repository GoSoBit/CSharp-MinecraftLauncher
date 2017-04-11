using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Caliburn.Micro;
using Launcher.Contracts;
using Launcher.Extensions;

namespace Launcher.ViewModels
{
    public sealed class ShellViewModel : Conductor<ITab>, IShell
    {
        private IEnumerable<ITab> tabs;

        public ShellViewModel(IEnumerable<ITab> tabs)
        {
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

        protected override void OnViewLoaded(object view)
        {
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