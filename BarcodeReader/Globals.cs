using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodeReader
{
    public static class Globals
    {
        public delegate void MessageEventHandler(string message, ShowLogType type, Color MessageColor);

        public static event MessageEventHandler MessageEvent;
        public static void ShowLog(string message, Color? messagecolor = null, ShowLogType? type = ShowLogType.None)
        {
            Color finalColor = messagecolor ?? Color.Black; // Gán mặc định nếu messagecolor là null
            ShowLogType index = type ?? ShowLogType.None; // Gán mặc định nếu type là null
            MessageEvent?.Invoke(message, index, finalColor);
        }
        public static string configFilePath = "Config\\config.xml";

    }

    public enum ReadType
    {
        SingleCode = 0,
        MultiCode = 1,
    }
    public enum barcodeType
    {
        Barcode1D,
        Barcode2D,
        AnyBarcode
    }
}
