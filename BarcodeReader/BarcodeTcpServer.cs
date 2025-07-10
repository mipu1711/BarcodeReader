using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarcodeReader
{
    public class BarcodeTcpServer
    {
        private TcpListener listener;
        private Thread listenThread;
        private bool isRunning = false;
        private int port;
        private bool hasRestarted = false; // Flag để đảm bảo chỉ restart 1 lần

        public Func<Task<string>> OnBarcodeReadAsync; // Hàm delegate async trả về barcode khi có trigger

        public void Start(int port)
        {
            this.port = port;
            this.hasRestarted = false;
            StartInternal();
        }

        private void StartInternal()
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                isRunning = true;
                listenThread = new Thread(ListenForClients);
                listenThread.IsBackground = true;
                listenThread.Start();

                Globals.ShowLog($"Server started on port {port}", Color.Green, ShowLogType.SaveLogToFile);
            }
            catch (Exception ex)
            {
                Globals.ShowLog($"Failed to start server on port {port}: {ex.Message}", Color.Red, ShowLogType.SaveLogToFile);
                MessageBox.Show($"Không thể khởi động server trên port {port}: {ex.Message}",
                               "Lỗi Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isRunning = false;
            }
        }

        public void Stop()
        {
            isRunning = false;
            listener?.Stop();
            hasRestarted = false; // Reset flag khi stop manual
            Globals.ShowLog("Server stopped", Color.Orange, ShowLogType.SaveLogToFile);
        }

        public bool IsRunning()
        {
            return isRunning;
        }

        private void ListenForClients()
        {
            try
            {
                listener.Start();

                while (isRunning)
                {
                    try
                    {
                        // Chấp nhận client mới
                        TcpClient client = listener.AcceptTcpClient();
                        Globals.ShowLog("Client connected", Color.Blue, ShowLogType.SaveLogToFile);

                        // Xử lý client trong thread riêng để có thể chấp nhận nhiều client
                        Thread clientThread = new Thread(async () => await HandleClientAsync(client));
                        clientThread.IsBackground = true;
                        clientThread.Start();
                    }
                    catch (ObjectDisposedException)
                    {
                        // Server đã được dừng
                        break;
                    }
                    catch (SocketException ex)
                    {
                        if (isRunning)
                        {
                            Globals.ShowLog($"Socket error: {ex.Message}", Color.Red, ShowLogType.SaveLogToFile);
                            HandleServerError(ex);
                        }
                        break;
                    }
                    catch (Exception ex)
                    {
                        Globals.ShowLog($"Error accepting client: {ex.Message}", Color.Red, ShowLogType.SaveLogToFile);
                        if (isRunning)
                        {
                            HandleServerError(ex);
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                if (isRunning)
                {
                    Globals.ShowLog($"Fatal server error: {ex.Message}", Color.Red, ShowLogType.SaveLogToFile);
                    HandleServerError(ex);
                }
            }
            finally
            {
                try
                {
                    listener?.Stop();
                }
                catch { }
            }
        }

        private void HandleServerError(Exception ex)
        {
            if (!hasRestarted && isRunning)
            {
                hasRestarted = true;
                Globals.ShowLog("Attempting to restart server...", Color.Orange, ShowLogType.SaveLogToFile);

                // Dừng server hiện tại
                try
                {
                    listener?.Stop();
                }
                catch { }

                // Đợi một chút trước khi restart
                Thread.Sleep(1000);

                // Thử restart
                try
                {
                    listener = new TcpListener(IPAddress.Any, port);
                    Thread restartThread = new Thread(ListenForClients);
                    restartThread.IsBackground = true;
                    restartThread.Start();

                    Globals.ShowLog("Server restarted successfully", Color.Green, ShowLogType.SaveLogToFile);

                    // Hiển thị thông báo restart thành công
                    MessageBox.Show("Server đã được khởi động lại thành công sau lỗi.",
                                   "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception restartEx)
                {
                    isRunning = false;

                    string errorMsg = $"Server gặp lỗi và không thể khởi động lại:\n\n" +
                                     $"Lỗi gốc: {ex.Message}\n" +
                                     $"Lỗi restart: {restartEx.Message}\n\n" +
                                     $"Vui lòng khởi động lại server thủ công.";

                    Globals.ShowLog($"Failed to restart server: {restartEx.Message}", Color.Red, ShowLogType.SaveLogToFile);

                    MessageBox.Show(errorMsg, "Lỗi Server",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (isRunning)
            {
                // Đã restart rồi mà vẫn lỗi
                isRunning = false;

                string errorMsg = $"Server gặp lỗi sau khi đã restart:\n\n{ex.Message}\n\n" +
                                 "Vui lòng kiểm tra và khởi động lại server thủ công.";

                Globals.ShowLog($"Server failed after restart: {ex.Message}", Color.Red, ShowLogType.SaveLogToFile);

                MessageBox.Show(errorMsg, "Lỗi Server",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();
                byte[] buffer = new byte[1024];

                while (isRunning && client.Connected)
                {
                    try
                    {
                        // Kiểm tra xem có dữ liệu available không
                        if (stream.DataAvailable)
                        {
                            int bytesRead = stream.Read(buffer, 0, buffer.Length);
                            if (bytesRead > 0)
                            {
                                string request = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                                if (request == "TRIGGER")
                                {
                                    // Khi nhận trigger, đọc barcode với async
                                    string barcode = "NO_BARCODE";
                                    if (OnBarcodeReadAsync != null)
                                    {
                                        try
                                        {
                                            barcode = await OnBarcodeReadAsync.Invoke();
                                            if (string.IsNullOrEmpty(barcode))
                                            {
                                                barcode = "NO_BARCODE";
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Globals.ShowLog($"Lỗi khi đọc barcode: {ex.Message}", Color.Red, ShowLogType.SaveLogToFile);
                                            barcode = "ERROR";
                                        }
                                    }
                                    
                                    byte[] data = Encoding.UTF8.GetBytes(barcode + "\n");
                                    stream.Write(data, 0, data.Length);
                                    stream.Flush();

                                    Globals.ShowLog($"Barcode sent: {barcode}", Color.Green, ShowLogType.SaveLogToFile);
                                }
                            }
                        }
                        else
                        {
                            // Không có dữ liệu, nghỉ một chút để tránh CPU cao
                            Thread.Sleep(10);
                        }
                    }
                    catch (Exception)
                    {
                        // Client đã ngắt kết nối
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Globals.ShowLog($"Error handling client: {ex.Message}", Color.Red, ShowLogType.SaveLogToFile);
            }
            finally
            {
                // Đảm bảo đóng kết nối
                try
                {
                    stream?.Close();
                    client?.Close();
                    Globals.ShowLog("Client disconnected", Color.Black, ShowLogType.SaveLogToFile);
                }
                catch { }
            }
        }
    }
}