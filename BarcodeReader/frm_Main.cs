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
            UpdateStatus("Initializing...", Color.Blue);


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

                            // Copy image processing settings
                            Config.Instance.EnableImageProcessing = config.EnableImageProcessing;
                            Config.Instance.ProcessingTimeoutSeconds = config.ProcessingTimeoutSeconds;
                            Config.Instance.EnableContrastEnhancement = config.EnableContrastEnhancement;
                            Config.Instance.ContrastLevel = config.ContrastLevel;
                            Config.Instance.EnableSharpening = config.EnableSharpening;
                            Config.Instance.SharpeningLevel = config.SharpeningLevel;
                            Config.Instance.EnableBrightnessAdjustment = config.EnableBrightnessAdjustment;
                            Config.Instance.BrightnessLevel = config.BrightnessLevel;
                            Config.Instance.EnableGammaCorrection = config.EnableGammaCorrection;
                            Config.Instance.GammaLevel = config.GammaLevel;
                            Config.Instance.EnableBlurUnsharp = config.EnableBlurUnsharp;
                            Config.Instance.BlurRadius = config.BlurRadius;
                            Config.Instance.EnableBinaryThreshold = config.EnableBinaryThreshold;
                            Config.Instance.ThresholdValue = config.ThresholdValue;
                            Config.Instance.EnableMorphological = config.EnableMorphological;
                            Config.Instance.MorphologySize = config.MorphologySize;
                            Config.Instance.EnableHistogramEqualization = config.EnableHistogramEqualization;

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
            barcodeTcpServer.OnBarcodeReadAsync = GetBarcodeFromTriggerAsync;
            //Globals.ShowLog("Máy chủ TCP đã khởi động thành công.", Color.Green, ShowLogType.SaveLogToFile);
            Globals.ShowLog($"Máy chủ TCP đã khởi động trên cổng {Config.Instance.Port}.", Color.Green, ShowLogType.SaveLogToFile);
            UpdateStatus("Ready", Color.Green);

        }

        private void btn_config_Click(object sender, EventArgs e)
        {
            Form frmConfig = new frm_Config();
            frmConfig.ShowDialog();
        }
        private async Task<string> GetBarcodeFromTriggerAsync()
        {
            if (!barcodeTcpServer.IsRunning())
            {
                UpdateStatus("TCP server not running", Color.Red);
                Globals.ShowLog("Máy chủ TCP không hoạt động.", Color.Red, ShowLogType.SaveLogToFile);
                return string.Empty;
            }

            if (string.IsNullOrWhiteSpace(Config.Instance.ImagePath) || !Directory.Exists(Config.Instance.ImagePath))
            {
                UpdateStatus("Invalid image path", Color.Red);
                Globals.ShowLog("Đường dẫn hình ảnh không hợp lệ hoặc không tồn tại.", Color.Red, ShowLogType.SaveLogToFile);
                return string.Empty;
            }

            // Đọc hình ảnh từ đường folder
            string[] pngFiles = Directory.GetFiles(Config.Instance.ImagePath, "*.png");
            if (pngFiles.Length == 0)
            {
                UpdateStatus("No PNG files found", Color.Red);
                Globals.ShowLog("Không tìm thấy tệp hình ảnh PNG nào trong thư mục đã chỉ định.", Color.Red, ShowLogType.SaveLogToFile);
                return string.Empty;
            }

            string imagePath = pngFiles[0]; // Lấy tệp hình ảnh đầu tiên
            string result = string.Empty;
            Bitmap originalImage = null;

            try
            {
                UpdateStatus("Loading image...", Color.Blue);
                
                // Đọc hình ảnh bằng MemoryStream để tránh lock file
                byte[] imageBytes = File.ReadAllBytes(imagePath);
                using (var ms = new MemoryStream(imageBytes))
                {
                    originalImage = new Bitmap(ms);
                }

                Globals.ShowLog($"Đang xử lý hình ảnh: {Path.GetFileName(imagePath)}", Color.Blue, ShowLogType.SaveLogToFile);

                // Thử đọc ảnh gốc trước
                UpdateStatus("Reading original image...", Color.Blue);
                result = await TryReadBarcodeAsync(originalImage, "ảnh gốc");
                
                if (!string.IsNullOrEmpty(result))
                {
                    UpdateStatus("Barcode found!", Color.Green);
                    return result;
                }

                // Nếu không thành công và image processing được bật
                if (Config.Instance.EnableImageProcessing)
                {
                    UpdateStatus("Starting advanced processing...", Color.Orange);
                    Globals.ShowLog("Bắt đầu xử lý ảnh nâng cao...", Color.Orange, ShowLogType.SaveLogToFile);
                    result = await ProcessImageWithEnhancementsAsync(originalImage);
                }

                if (string.IsNullOrEmpty(result))
                {
                    UpdateStatus("No barcode found after all processing", Color.Red);
                    Globals.ShowLog("Không thể đọc mã vạch sau tất cả các phương pháp xử lý.", Color.Red, ShowLogType.SaveLogToFile);
                }
                else
                {
                    UpdateStatus("Barcode found with processing!", Color.Green);
                }

                return result;
            }
            catch (Exception ex)
            {
                UpdateStatus("Error reading image", Color.Red);
                Globals.ShowLog($"Lỗi khi đọc hình ảnh: {ex.Message}", Color.Red, ShowLogType.SaveLogToFile);
                return string.Empty;
            }
            finally
            {
                originalImage?.Dispose();
                UpdateStatus("Ready", Color.Blue);
            }
        }

        private async Task<string> ProcessImageWithEnhancementsAsync(Bitmap originalImage)
        {
            var processingMethods = GetEnabledProcessingMethods();
            string result = string.Empty;

            foreach (var method in processingMethods)
            {
                if (!string.IsNullOrEmpty(result))
                    break;

                try
                {
                    Bitmap processedImage = null;
                    
                    try
                    {
                        // Áp dụng phương pháp xử lý
                        processedImage = await Task.Run(() => method.ProcessMethod(originalImage));
                        
                        if (processedImage != null)
                        {
                            UpdateStatus($"Trying: {method.Name}", Color.Blue);
                            Globals.ShowLog($"Đang thử đọc với phương pháp: {method.Name}", Color.Blue, ShowLogType.SaveLogToFile);
                            result = await TryReadBarcodeAsync(processedImage, method.Name);
                            
                            if (!string.IsNullOrEmpty(result))
                            {
                                UpdateStatus($"Success with: {method.Name}", Color.Green);
                                Globals.ShowLog($"Thành công với phương pháp: {method.Name}", Color.Green, ShowLogType.SaveLogToFile);
                                break;
                            }
                        }
                    }
                    finally
                    {
                        processedImage?.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Globals.ShowLog($"Lỗi khi áp dụng {method.Name}: {ex.Message}", Color.Red, ShowLogType.SaveLogToFile);
                }
            }

            return result;
        }

        private async Task<string> TryReadBarcodeAsync(Bitmap image, string methodName)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (Config.Instance.ReadType == ReadType.SingleCode)
                    {
                        Result barcode = barcodeReader.Decode(image);
                        if (barcode != null)
                        {
                            string result = barcode.Text;
                            Globals.ShowLog($"Đã đọc mã vạch với {methodName}: {result}", Color.Green, ShowLogType.SaveLogToFile);
                            return result;
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
                                string result = string.Join(Config.Instance.MiddleCharacter, barcodeTexts);
                                Globals.ShowLog($"Đã đọc {barcodeTexts.Count} mã vạch với {methodName}: {result}", Color.Green, ShowLogType.SaveLogToFile);
                                return result;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Globals.ShowLog($"Lỗi khi đọc mã vạch với {methodName}: {ex.Message}", Color.Red, ShowLogType.SaveLogToFile);
                }
                
                return string.Empty;
            });
        }

        private List<ProcessingMethod> GetEnabledProcessingMethods()
        {
            var methods = new List<ProcessingMethod>();

            if (Config.Instance.EnableContrastEnhancement)
            {
                methods.Add(new ProcessingMethod
                {
                    Name = "Tăng độ tương phản",
                    ProcessMethod = img => ImageProcessor.EnhanceContrast(img, Config.Instance.ContrastLevel)
                });
            }

            if (Config.Instance.EnableSharpening)
            {
                methods.Add(new ProcessingMethod
                {
                    Name = "Làm nét ảnh",
                    ProcessMethod = img => ImageProcessor.SharpenImage(img, Config.Instance.SharpeningLevel)
                });
            }

            if (Config.Instance.EnableBrightnessAdjustment)
            {
                methods.Add(new ProcessingMethod
                {
                    Name = "Điều chỉnh độ sáng",
                    ProcessMethod = img => ImageProcessor.AdjustBrightness(img, Config.Instance.BrightnessLevel)
                });
            }

            if (Config.Instance.EnableGammaCorrection)
            {
                methods.Add(new ProcessingMethod
                {
                    Name = "Gamma correction",
                    ProcessMethod = img => ImageProcessor.ApplyGammaCorrection(img, Config.Instance.GammaLevel)
                });
            }

            if (Config.Instance.EnableBlurUnsharp)
            {
                methods.Add(new ProcessingMethod
                {
                    Name = "Gaussian blur",
                    ProcessMethod = img => ImageProcessor.ApplyGaussianBlur(img, Config.Instance.BlurRadius)
                });
            }

            if (Config.Instance.EnableBinaryThreshold)
            {
                methods.Add(new ProcessingMethod
                {
                    Name = "Binary threshold",
                    ProcessMethod = img => ImageProcessor.ApplyBinaryThreshold(img, Config.Instance.ThresholdValue)
                });
            }

            if (Config.Instance.EnableMorphological)
            {
                methods.Add(new ProcessingMethod
                {
                    Name = "Erosion",
                    ProcessMethod = img => ImageProcessor.ApplyErosion(img, Config.Instance.MorphologySize)
                });

                methods.Add(new ProcessingMethod
                {
                    Name = "Dilation",
                    ProcessMethod = img => ImageProcessor.ApplyDilation(img, Config.Instance.MorphologySize)
                });
            }

            if (Config.Instance.EnableHistogramEqualization)
            {
                methods.Add(new ProcessingMethod
                {
                    Name = "Histogram equalization",
                    ProcessMethod = img => ImageProcessor.ApplyHistogramEqualization(img)
                });
            }

            return methods;
        }

        private class ProcessingMethod
        {
            public string Name { get; set; }
            public Func<Bitmap, Bitmap> ProcessMethod { get; set; }
        }

        private void UpdateStatus(string message, Color color)
        {
            if (lbl_status.InvokeRequired)
            {
                lbl_status.BeginInvoke(new Action(() => UpdateStatus(message, color)));
                return;
            }
            lbl_status.Text = message;
            lbl_status.ForeColor = color;
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
