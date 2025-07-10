using System.IO;
using System.Xml.Serialization;

namespace BarcodeReader
{
    public static class ConfigIO
    {
        public static void Save(string filePath, Config config)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Config));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, config);
            }
        }

        public static Config Load(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Config));
            using (StreamReader reader = new StreamReader(filePath))
            {
                return (Config)serializer.Deserialize(reader);
            }
        }
    }
}