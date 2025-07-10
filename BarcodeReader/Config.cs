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

            // Image processing default values
            EnableImageProcessing = true;
            ProcessingTimeoutSeconds = 30;
            EnableContrastEnhancement = true;
            ContrastLevel = 1.5f;
            EnableSharpening = true;
            SharpeningLevel = 1.0f;
            EnableBrightnessAdjustment = true;
            BrightnessLevel = 0.1f;
            EnableGammaCorrection = true;
            GammaLevel = 1.2f;
            EnableBlurUnsharp = true;
            BlurRadius = 1.0f;
            EnableBinaryThreshold = true;
            ThresholdValue = 128;
            EnableMorphological = true;
            MorphologySize = 3;
            EnableHistogramEqualization = true;
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

        // Image Processing Settings
        [XmlElement]
        public bool EnableImageProcessing { get; set; }

        [XmlElement]
        public int ProcessingTimeoutSeconds { get; set; }

        [XmlElement]
        public bool EnableContrastEnhancement { get; set; }

        [XmlElement]
        public float ContrastLevel { get; set; }

        [XmlElement]
        public bool EnableSharpening { get; set; }

        [XmlElement]
        public float SharpeningLevel { get; set; }

        [XmlElement]
        public bool EnableBrightnessAdjustment { get; set; }

        [XmlElement]
        public float BrightnessLevel { get; set; }

        [XmlElement]
        public bool EnableGammaCorrection { get; set; }

        [XmlElement]
        public float GammaLevel { get; set; }

        [XmlElement]
        public bool EnableBlurUnsharp { get; set; }

        [XmlElement]
        public float BlurRadius { get; set; }

        [XmlElement]
        public bool EnableBinaryThreshold { get; set; }

        [XmlElement]
        public int ThresholdValue { get; set; }

        [XmlElement]
        public bool EnableMorphological { get; set; }

        [XmlElement]
        public int MorphologySize { get; set; }

        [XmlElement]
        public bool EnableHistogramEqualization { get; set; }
    }
}