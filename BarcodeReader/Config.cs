using System;
using System.Xml.Serialization;
using ZXing;

namespace BarcodeReader
{
    [Serializable]
    public class Config
    {
        private static Config instance = new Config();

        public static Config Instance
        {
            get { return instance; }
        }

        public Config()
        {
            // Default values
            ImagePath = "";
            MiddleCharacter = "_";
            EndCharacter = "|";
            Port = 2000;
            ReadType = ReadType.SingleCode;
            BarcodeType = barcodeType.Barcode1D;
        }

        [XmlElement]
        public string ImagePath { get; set; }

        [XmlElement]
        public string MiddleCharacter { get; set; }

        [XmlElement]
        public string EndCharacter { get; set; }

        [XmlElement]
        public int Port { get; set; }

        [XmlElement]
        public ReadType ReadType { get; set; }

        [XmlElement]
        public barcodeType BarcodeType { get; set; }
    }
}