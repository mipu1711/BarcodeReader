namespace BarcodeReader
{
    partial class frm_Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txt_Log = new System.Windows.Forms.RichTextBox();
            this.btn_config = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.Controls.Add(this.txt_Log, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btn_config, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1100, 721);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // txt_Log
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.txt_Log, 3);
            this.txt_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_Log.Font = new System.Drawing.Font("Courier New", 10F);
            this.txt_Log.Location = new System.Drawing.Point(3, 53);
            this.txt_Log.Name = "txt_Log";
            this.txt_Log.ReadOnly = true;
            this.txt_Log.Size = new System.Drawing.Size(1094, 665);
            this.txt_Log.TabIndex = 2;
            this.txt_Log.TabStop = false;
            this.txt_Log.Text = "";
            // 
            // btn_config
            // 
            this.btn_config.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_config.Location = new System.Drawing.Point(983, 3);
            this.btn_config.Name = "btn_config";
            this.btn_config.Size = new System.Drawing.Size(114, 44);
            this.btn_config.TabIndex = 3;
            this.btn_config.Text = "Config";
            this.btn_config.UseVisualStyleBackColor = true;
            this.btn_config.Click += new System.EventHandler(this.btn_config_Click);
            // 
            // frm_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 721);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "frm_Main";
            this.Text = "BarcodeReader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_Main_FormClosing);
            this.Load += new System.EventHandler(this.frm_Main_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RichTextBox txt_Log;
        private System.Windows.Forms.Button btn_config;
    }
}

