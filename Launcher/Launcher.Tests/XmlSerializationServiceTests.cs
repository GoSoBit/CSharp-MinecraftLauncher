using System;
using Launcher.Desktop.Models;
using Launcher.Desktop.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;

namespace Launcher.Tests
{
    [TestClass]
    public class XmlSerializationServiceTests
    {
        private const string XmlPackFormat = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<Pack xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <Guid>{0}</Guid>\r\n  <Id>{1}</Id>\r\n  <Type>{2}</Type>\r\n  <Url>{3}</Url>\r\n</Pack>";
        private IFixture fixture = new Fixture();

        [TestMethod]
        public void ShouldSerializeObject()
        {
            var serializationService = new XmlSerializationService();
            var pack = fixture.Create<Pack>();

            string generatedXml = serializationService.Serialize(pack);

            string packInXmlFormat = string.Format(XmlPackFormat, pack.Guid, pack.Id, pack.Type, pack.Url);
            Assert.AreEqual(packInXmlFormat, generatedXml);
        }

        [TestMethod]
        public void ShouldDeserializeObject()
        {
            var serizalizationService = new XmlSerializationService();
            var pack = fixture.Create<Pack>();
            string xml = string.Format(XmlPackFormat, pack.Guid, pack.Id, pack.Type, pack.Url);

            Pack generatedPack = serizalizationService.Deserialize<Pack>(xml);

            Assert.AreEqual(pack.Id, generatedPack.Id);
            Assert.AreEqual(pack.Guid, generatedPack.Guid);
            Assert.AreEqual(pack.Type, generatedPack.Type);
            Assert.AreEqual(pack.Url, generatedPack.Url);
        }
    }
}
