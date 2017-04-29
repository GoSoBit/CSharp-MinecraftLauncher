using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using Launcher.Desktop.Extensions;
using Launcher.Desktop.Models;
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

        [TestMethod]
        public void Collections_ShouldMakeObservableCollectionFromEnumerable()
        {
            IEnumerable<object> enumerable = new List<object>();
            ICollection<object> collection = new Collection<object>();
            ObservableCollection<object> observableCollection = new ObservableCollection<object>();

            var observableCollection1 = enumerable.ToObservable();
            var observableCollection2 = collection.ToObservable();
            var observableCollection3 = observableCollection.ToObservable();

            Assert.IsInstanceOfType(observableCollection1, typeof(ObservableCollection<object>));
            Assert.IsInstanceOfType(observableCollection2, typeof(ObservableCollection<object>));
            Assert.IsInstanceOfType(observableCollection3, typeof(ObservableCollection<object>));
        }

        [TestMethod]
        public void Collections_ShouldAddOrUpdate()
        {
            var list = new List<Profile>
            {
                new Profile("1", "mati", false),
                new Profile("2", "kapi", false),
                new Profile("3", "", true)
            };
            var updatedProfile = new Profile("1", "dada", false);
            var addedProfile = new Profile("4", "mati", false);

            list.AddOrUpdate(updatedProfile, x => x.Id == "1");
            list.AddOrUpdate(addedProfile, x => x.Id == "4");
            list.AddOrUpdate(null, x => x.Id == "2");

            Assert.AreEqual(updatedProfile, list.FirstOrDefault(x => x.Id == "1"));
            Assert.AreEqual(addedProfile, list.FirstOrDefault(x => x.Id == "4"));
            Assert.AreEqual(4, list.Count);
            Assert.IsTrue(list.Contains(null));
        }
    }
}
