using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.Common;

namespace BarcodeReader
{
    public partial class frm_Main : Form
    {
        public frm_Main()
        {
            InitializeComponent();
            Globals.MessageEvent += AddLogWindow;
            List<BarcodeFormat> possibleFormats = new List<BarcodeFormat>();

            switch (Config.Instance.BarcodeType)
            {
                case barcodeType.Barcode1D:
                    possibleFormats = new List<BarcodeFormat> { (BarcodeFormat)61918 };
                    break;
                case barcodeType.Barcode2D:
                    possibleFormats = new List<BarcodeFormat>
                {
                    (BarcodeFormat)32,
                    (BarcodeFormat)2048,
                    (BarcodeFormat)1,
                    (BarcodeFormat)512
                };
                    break;
                case barcodeType.AnyBarcode:
                    possibleFormats = new List<BarcodeFormat>
                {
                    (BarcodeFormat)61918,
                    (BarcodeFormat)32,
                    (BarcodeFormat)2048,
                    (BarcodeFormat)1,
                    (BarcodeFormat)512
                };
                    break;
            }


            barcodeReader = (IBarcodeReader)new ZXing.BarcodeReader
            {
                AutoRotate = true,
                Options = new DecodingOptions
                {
                    TryHarder = true,
                    PureBarcode = false,
                    ReturnCodabarStartEnd = true,
                    PossibleFormats = possibleFormats,
                    TryInverted = true
                }
            };

        }
        Bitmap image = null;
        private void frm_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (barcodeTcpServer.IsRunning())
            {
                barcodeTcpServer.Stop();
                Globals.ShowLog("Máy chủ TCP đã dừng.", Color.Green, ShowLogType.SaveLogToFile);
            }
            Globals.MessageEvent -= AddLogWindow;
        }

        IBarcodeReader barcodeReader;
        private void AddLogWindow(string text, ShowLogType type, Color txt_color)
        {
            if (txt_Log.InvokeRequired)
            {
                txt_Log.BeginInvoke(new Action(() => AddLogWindow(text, type, txt_color)));
                return;
            }
            string datetime = "[" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + "]: ";
            text = text.Replace(";", "\r\n" + "".PadLeft(datetime.Length, ' '));
            txt_Log.SelectionStart = txt_Log.TextLength;
            txt_Log.SelectionColor = txt_color;
            if (txt_Log.Text.Split('\n').Length > 500)
            {
                txt_Log.Text = txt_Log.Text.Substring(txt_Log.Text.IndexOf("\n"));
            }
            txt_Log.AppendText($"{datetime}{text}\r\n");
            txt_Log.ScrollToCaret();
            if ((int)type == 1)
            {
                // lưu log vào file
                string logFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "log.txt");
                if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(logFilePath)))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(logFilePath));
                }
                try
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(logFilePath, true))
                    {
                        sw.WriteLine($"{datetime}{text}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lưu log vào file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if ((int)type == 2)
            {
                // lưu log vào database
            }
        }

        BarcodeTcpServer barcodeTcpServer = new BarcodeTcpServer();
        private void frm_Main_Load(object sender, EventArgs e)
        {


            Globals.configFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Globals.configFilePath);
            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(Globals.configFilePath)))
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(Globals.configFilePath));
            }
            if (System.IO.File.Exists(Globals.configFilePath))
            {
                try
                {
                    Config config = ConfigIO.Load(Globals.configFilePath);
                    if (config != null)
                    {
                        if (config != null)
                        {
                            // Copy từng property sang instance hiện tại
                            Config.Instance.ImagePath = config.ImagePath;
                            Config.Instance.MiddleCharacter = config.MiddleCharacter;
                            Config.Instance.EndCharacter = config.EndCharacter;
                            Config.Instance.Port = config.Port;
                            Config.Instance.ReadType = config.ReadType;
                            Config.Instance.BarcodeType = config.BarcodeType;

                            Globals.ShowLog("Cấu hình đã được tải thành công.", Color.Green, ShowLogType.SaveLogToFile);
                        }
                    }
                    else
                    {
                        Globals.ShowLog("Cấu hình không hợp lệ, sử dụng cấu hình mặc định.", Color.Red, ShowLogType.SaveLogToFile);
                        Config defaultConfig = Config.Instance;
                        ConfigIO.Save(Globals.configFilePath, defaultConfig);
                        Globals.ShowLog("Cấu hình mặc định đã được lưu.", Color.Green, ShowLogType.SaveLogToFile);

                    }
                }
                catch (Exception ex)
                {
                    Globals.ShowLog($"Lỗi khi tải cấu hình: {ex.Message}", Color.Red, ShowLogType.SaveLogToFile);
                }
            }
            else
            {
                Globals.ShowLog("Cấu hình không tồn tại, sử dụng cấu hình mặc định.", Color.Blue, ShowLogType.SaveLogToFile);
                Config defaultConfig = Config.Instance;
                try
                {
                    ConfigIO.Save(Globals.configFilePath, defaultConfig);
                    Globals.ShowLog("Cấu hình mặc định đã được lưu.", Color.Green, ShowLogType.SaveLogToFile);
                }
                catch (Exception ex)
                {
                    Globals.ShowLog($"Lỗi khi lưu cấu hình mặc định: {ex.Message}", Color.Red, ShowLogType.SaveLogToFile);
                }

            }
            barcodeTcpServer.Start(Config.Instance.Port);
            if(!barcodeTcpServer.IsRunning())
            {
                Globals.ShowLog("Không thể khởi động máy chủ TCP.", Color.Red, ShowLogType.SaveLogToFile);
                MessageBox.Show("Không thể khởi động máy chủ TCP. Vui lòng kiểm tra cài đặt và thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            barcodeTcpServer.OnBarcodeRead += GetBarcodeFromTrigger;
            //Globals.ShowLog("Máy chủ TCP đã khởi động thành công.", Color.Green, ShowLogType.SaveLogToFile);
            Globals.ShowLog($"Máy chủ TCP đã khởi động trên cổng {Config.Instance.Port}.", Color.Green, ShowLogType.SaveLogToFile);

        }

        private void btn_config_Click(object sender, EventArgs e)
        {
            Form frmConfig = new frm_Config();
            frmConfig.ShowDialog();
        }
        private string GetBarcodeFromTrigger()
        {
            if (!barcodeTcpServer.IsRunning())
            {
                Globals.ShowLog("Máy chủ TCP không hoạt động.", Color.Red, ShowLogType.SaveLogToFile);
                return string.Empty;
            }

            if (string.IsNullOrWhiteSpace(Config.Instance.ImagePath) || !Directory.Exists(Config.Instance.ImagePath))
            {
                Globals.ShowLog("Đường dẫn hình ảnh không hợp lệ hoặc không tồn tại.", Color.Red, ShowLogType.SaveLogToFile);
                return string.Empty;
            }

            // Đọc hình ảnh từ đường folder
            string[] pngFiles = Directory.GetFiles(Config.Instance.ImagePath, "*.png");
            if (pngFiles.Length == 0)
            {
                Globals.ShowLog("Không tìm thấy tệp hình ảnh PNG nào trong thư mục đã chỉ định.", Color.Red, ShowLogType.SaveLogToFile);
                return string.Empty;
            }

            string imagePath = pngFiles[0]; // Lấy tệp hình ảnh đầu tiên
            string result = string.Empty;

            // Dispose image cũ trước khi tạo mới
            if (image != null)
            {
                image.Dispose();
                image = null;
            }

            try
            {
                // Đọc hình ảnh bằng MemoryStream để tránh lock file
                byte[] imageBytes = File.ReadAllBytes(imagePath);
                using (var ms = new MemoryStream(imageBytes))
                {
                    image = new Bitmap(ms);
                }

                Globals.ShowLog($"Đang xử lý hình ảnh: {Path.GetFileName(imagePath)}", Color.Blue, ShowLogType.SaveLogToFile);
            }
            catch (Exception ex)
            {
                Globals.ShowLog($"Lỗi khi đọc hình ảnh: {ex.Message}", Color.Red, ShowLogType.SaveLogToFile);
                return string.Empty;
            }

            try
            {
                if (Config.Instance.ReadType == ReadType.SingleCode)
                {
                    // Đọc mã vạch từ hình ảnh
                    Result barcode = barcodeReader.Decode(image);
                    if (barcode != null)
                    {
                        result = barcode.Text;
                        Globals.ShowLog($"Đã đọc mã vạch: {result}", Color.Green, ShowLogType.SaveLogToFile);
                    }
                    else
                    {
                        Globals.ShowLog("Không tìm thấy mã vạch trong hình ảnh.", Color.Orange, ShowLogType.SaveLogToFile);
                        result = string.Empty;
                    }
                }
                else if (Config.Instance.ReadType == ReadType.MultiCode)
                {
                    Result[] barcodes = barcodeReader.DecodeMultiple(image);
                    if (barcodes != null && barcodes.Length > 0)
                    {
                        List<string> barcodeTexts = barcodes
                            .Where(b => b != null && !string.IsNullOrWhiteSpace(b.Text))
                            .Select(b => b.Text.Trim())
                            .ToList();

                        if (barcodeTexts.Count > 0)
                        {
                            result = string.Join(Config.Instance.MiddleCharacter, barcodeTexts);
                            Globals.ShowLog($"Đã đọc {barcodeTexts.Count} mã vạch: {result}", Color.Green, ShowLogType.SaveLogToFile);
                        }
                        else
                        {
                            Globals.ShowLog("Tất cả mã vạch đọc được đều không hợp lệ.", Color.Orange, ShowLogType.SaveLogToFile);
                            result = string.Empty;
                        }
                    }
                    else
                    {
                        Globals.ShowLog("Không tìm thấy mã vạch nào trong hình ảnh.", Color.Orange, ShowLogType.SaveLogToFile);
                        result = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Globals.ShowLog($"Lỗi khi đọc mã vạch: {ex.Message}", Color.Red, ShowLogType.SaveLogToFile);
                result = string.Empty;
            }
            finally
            {
                // Xóa toàn bộ ảnh sau khi đọc xong (bất kể thành công hay thất bại)
                //DeleteAllImages();
            }

            return result;
        }

        private void DeleteAllImages()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Config.Instance.ImagePath) || !Directory.Exists(Config.Instance.ImagePath))
                {
                    return;
                }

                // Lấy tất cả file ảnh (PNG, JPG, JPEG, BMP, GIF)
                string[] imageExtensions = { "*.png", "*.jpg", "*.jpeg", "*.bmp", "*.gif" };
                List<string> allImageFiles = new List<string>();

                foreach (string extension in imageExtensions)
                {
                    allImageFiles.AddRange(Directory.GetFiles(Config.Instance.ImagePath, extension));
                }

                if (allImageFiles.Count == 0)
                {
                    return; // Không có file nào để xóa
                }

                int deletedCount = 0;
                int failedCount = 0;

                foreach (string filePath in allImageFiles)
                {
                    try
                    {
                        // Đảm bảo file không bị lock trước khi xóa
                        if (IsFileLocked(filePath))
                        {
                            Globals.ShowLog($"File đang được sử dụng, không thể xóa: {Path.GetFileName(filePath)}",
                                          Color.Orange, ShowLogType.SaveLogToFile);
                            failedCount++;
                            continue;
                        }

                        File.Delete(filePath);
                        deletedCount++;
                    }
                    catch (Exception ex)
                    {
                        Globals.ShowLog($"Lỗi khi xóa file '{Path.GetFileName(filePath)}': {ex.Message}",
                                      Color.Red, ShowLogType.SaveLogToFile);
                        failedCount++;
                    }
                }

                // Log kết quả
                if (deletedCount > 0)
                {
                    Globals.ShowLog($"Đã xóa {deletedCount} file ảnh.", Color.Green, ShowLogType.SaveLogToFile);
                }

                if (failedCount > 0)
                {
                    Globals.ShowLog($"Không thể xóa {failedCount} file ảnh.", Color.Orange, ShowLogType.SaveLogToFile);
                }
            }
            catch (Exception ex)
            {
                Globals.ShowLog($"Lỗi khi xóa file ảnh: {ex.Message}", Color.Red, ShowLogType.SaveLogToFile);
            }
        }

        private bool IsFileLocked(string filePath)
        {
            try
            {
                using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                return true;
            }
            catch
            {
                return false;
            }
            return false;
        }


    }
}
