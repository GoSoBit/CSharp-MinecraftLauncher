using System;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using Launcher.Contracts;
using Launcher.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Launcher.Tests
{
    [TestClass]
    public class LogoutViewModelTests
    {
        [TestMethod]
        public void ShouldLogOffUsingServiceOnViewLoadedAndClosesWindow()
        {
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.LogOffAsync()).ReturnsAsync(true);
            var windowManagerMock = new Mock<IMetroWindowManager>();
            var viewModel = new LogoutViewModel(accountServiceMock.Object, windowManagerMock.Object);
            var fakeView = new UserControl();
            ViewModelBinder.Bind(viewModel, fakeView, null);

            fakeView.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));

            accountServiceMock.Verify(x => x.LogOffAsync(), Times.Once);
            windowManagerMock.Verify(x => x.CloseWindow(), Times.Once);
        }
    }
}
