using Caliburn.Micro;
using Launcher.Desktop.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;

// ReSharper disable ExplicitCallerInfoArgument

namespace Launcher.Tests
{
    [TestClass]
    public class ExtensionsTests
    {
        private readonly IFixture fixture = new Fixture();

        [TestMethod]
        public void NotifyPropertyChangedEx_ShouldSetAndNotify()
        {
            var value = fixture.Create<string>();
            var value2 = fixture.Create<string>();
            var propertyName = fixture.Create<string>();
            var viewModelMock = new Mock<INotifyPropertyChangedEx>();
            viewModelMock.Setup(x => x.NotifyOfPropertyChange(It.Is<string>(s => s == propertyName)));
            var viewModel = viewModelMock.Object;

            viewModel.Set(out string field, value, propertyName);
            viewModel.Set(out field, value2, propertyName);

            Assert.AreEqual(field, value2);
            viewModelMock.Verify(x => x.NotifyOfPropertyChange(propertyName), Times.Exactly(2));
        }
    }
}
