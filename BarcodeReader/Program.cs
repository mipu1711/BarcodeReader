using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarcodeReader
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var name = Process.GetCurrentProcess().ProcessName;
            var names = Process.GetProcessesByName(name);
            if (names.Count() > 1)
            {
                MessageBox.Show("The current program already exists in the process！\r About to close the current program...", "Serious error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Application.Run(new frm_Main());
        }
    }
}
