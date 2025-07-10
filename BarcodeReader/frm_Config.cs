using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

namespace BarcodeReader
{
    public partial class frm_Config : Form
    {
        public frm_Config()
        {

            InitializeComponent();
            // Set default values for controls
            cb_barcodeFormat.DataSource = Enum.GetValues(typeof(barcodeType));
            cb_readType.DataSource = Enum.GetValues(typeof(ReadType));
            // Load configuration
            txt_path.Text = Config.Instance.ImagePath;
            txt_middle.Text = Config.Instance.MiddleCharacter;
            txt_end.Text = Config.Instance.EndCharacter;
            txt_port.Text = Config.Instance.Port.ToString();
            cb_readType.SelectedIndex = (int)Config.Instance.ReadType;
            cb_barcodeFormat.SelectedItem = (int)Config.Instance.BarcodeType;
            
            // Load image processing settings
            chk_enableImageProcessing.Checked = Config.Instance.EnableImageProcessing;
            txt_timeout.Text = Config.Instance.ProcessingTimeoutSeconds.ToString();
        }

        private void btn_cancle_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(txt_path.Text))
            {
                Globals.ShowLog("Đường dẫn hình ảnh không được để trống.", Color.Red, ShowLogType.SaveLogToFile);
                return;
            }
            if (string.IsNullOrWhiteSpace(txt_middle.Text))
            {
                Globals.ShowLog("Ký tự giữa không được để trống.", Color.Red, ShowLogType.SaveLogToFile);
                return;
            }
            if (string.IsNullOrWhiteSpace(txt_end.Text))
            {
                Globals.ShowLog("Ký tự kết thúc không được để trống.", Color.Red, ShowLogType.SaveLogToFile);
                return;
            }
            if (string.IsNullOrWhiteSpace(txt_port.Text))
            {
                Globals.ShowLog("Cổng không được để trống.", Color.Red, ShowLogType.SaveLogToFile);
                return;
            }
            if (!int.TryParse(txt_port.Text, out int port) || port <= 0)
            {
                Globals.ShowLog("Cổng phải là một số nguyên dương.", Color.Red, ShowLogType.SaveLogToFile);
                return;
            }
            if (!int.TryParse(txt_timeout.Text, out int timeout) || timeout <= 0)
            {
                Globals.ShowLog("Timeout phải là một số nguyên dương.", Color.Red, ShowLogType.SaveLogToFile);
                return;
            }
            // Save configuration
            Config.Instance.ImagePath = txt_path.Text;
            Config.Instance.MiddleCharacter = txt_middle.Text;
            Config.Instance.EndCharacter = txt_end.Text;
            Config.Instance.Port = Convert.ToInt32(txt_port.Text);
            Config.Instance.ReadType = (ReadType)cb_readType.SelectedItem;
            Config.Instance.BarcodeType = (barcodeType)cb_barcodeFormat.SelectedItem;
            
            // Save image processing settings
            Config.Instance.EnableImageProcessing = chk_enableImageProcessing.Checked;
            Config.Instance.ProcessingTimeoutSeconds = Convert.ToInt32(txt_timeout.Text);
            // Save to file
            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(Globals.configFilePath)))
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(Globals.configFilePath));
            }
            if (System.IO.File.Exists(Globals.configFilePath))
            {
                System.IO.File.Delete(Globals.configFilePath);
            }
            // Save the configuration to the XML file

            ConfigIO.Save(Globals.configFilePath, Config.Instance);
            Globals.ShowLog("Cấu hình đã được lưu thành công.", Color.Green, ShowLogType.SaveLogToFile);
        }
    }
}
