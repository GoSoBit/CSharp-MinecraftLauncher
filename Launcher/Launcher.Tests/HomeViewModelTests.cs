using System;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Launcher.Desktop.Contracts;
using Launcher.Desktop.Models;
using Launcher.Desktop.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;

namespace Launcher.Tests
{
    [TestClass]
    public class HomeViewModelTests
    {
        private readonly IFixture fixture = new Fixture();
        private readonly Mock<IAccountService> accountServiceEmptyMock = new Mock<IAccountService>();
        private readonly Mock<INewsService> newsServiceEmptyMock = new Mock<INewsService>();
        private readonly Mock<IMetroWindowManager> windowManagerEmptyMock = new Mock<IMetroWindowManager>();

        [TestMethod]
        public void ShouldSetGetPropertiesAndNotifyPropertyChanged()
        {
            var viewModel = new HomeViewModel(accountServiceEmptyMock.Object, newsServiceEmptyMock.Object, windowManagerEmptyMock.Object);
            var notifyCount = 0;
            viewModel.PropertyChanged += (s, e) => notifyCount++;
            var email1 = fixture.Create<string>();
            var email2 = fixture.Create<string>();
            var news = fixture.CreateMany<News>().ToList();

            viewModel.Email = email1;
            viewModel.Email = email2;
            viewModel.News = null;
            viewModel.News = null;
            viewModel.News = news;

            Assert.AreEqual(5, notifyCount);
            Assert.AreEqual(email2, viewModel.Email);
            Assert.AreEqual(viewModel.News, news);
        }

        [TestMethod]
        public void ShouldShowNewsMessageBox()
        {
            var viewModel = new HomeViewModel(accountServiceEmptyMock.Object, newsServiceEmptyMock.Object, windowManagerEmptyMock.Object);
            var news = fixture.Create<News>();

            viewModel.ShowNews(news);
            try
            {
                viewModel.ShowNews(null);
            }
            catch
            {
                Assert.Fail();
            }

            windowManagerEmptyMock.Verify(x => x.ShowMessageAsync(It.Is<string>(s => s == news.Title), It.Is<string>(s => s == news.Content),
                It.IsAny<MessageDialogStyle>(),
                It.IsAny<MetroDialogSettings>()), Times.Once);
        }

        [TestMethod]
        public void ShouldTryLoadingEmailAndNews()
        {
            var windowManagerMock = new Mock<IMetroWindowManager>();
            windowManagerMock.Setup(x => x.ShowProgressAndDoAsync(It.IsAny<Func<Task>>()));
            var viewModel = new HomeViewModel(accountServiceEmptyMock.Object, newsServiceEmptyMock.Object, windowManagerMock.Object);

            ScreenExtensions.TryActivate(viewModel);

            windowManagerMock.Verify(x => x.ShowProgressAndDoAsync(It.IsAny<Func<Task>>()), Times.Once);
        }
    }
}