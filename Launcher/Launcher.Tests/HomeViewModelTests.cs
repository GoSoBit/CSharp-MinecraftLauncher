using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Launcher.Contracts;
using Launcher.Models;
using Launcher.ViewModels;
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
        public void ShouldLoadEmailAndNews()
        {
            var newsList = fixture.CreateMany<News>().ToList();
            var userInfo = fixture.Create<UserInfo>();
            var newsServiceMock = new Mock<INewsService>();
            newsServiceMock.Setup(x => x.GetNewsAsync()).ReturnsAsync(newsList);
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock.Setup(x => x.GetUserInfoAsync()).ReturnsAsync(userInfo);
            var viewModel = new HomeViewModel(accountServiceMock.Object, newsServiceMock.Object, windowManagerEmptyMock.Object);

            ScreenExtensions.TryActivate(viewModel);

            Assert.AreEqual(newsList, viewModel.News);
            Assert.AreEqual(userInfo.Email, viewModel.Email);
            newsServiceMock.Verify(x => x.GetNewsAsync(), Times.Once);
        }
    }
}