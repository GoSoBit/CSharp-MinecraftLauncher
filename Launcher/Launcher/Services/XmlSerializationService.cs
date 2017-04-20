using System.IO;
using System.Xml.Serialization;

namespace Launcher.Services
{
    public class XmlSerializationService
    {
        public string Serialize<T>(T list)
        {
            var serializer = new XmlSerializer(typeof(T));
            var writer = new StringWriter();
            using (writer)
            {
                serializer.Serialize(writer, list);
                return writer.ToString();
            }
        }

        public T Deserialize<T>(string xml)
        {
            var serializer = new XmlSerializer(typeof(T));
            var reader = new StringReader(xml);
            using (reader)
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}