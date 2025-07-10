namespace BarcodeReader
{
    partial class frm_Config
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
            this.txt_path = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_middle = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_cancle = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.txt_end = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_port = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cb_readType = new System.Windows.Forms.ComboBox();
            this.cb_barcodeFormat = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.chk_enableImageProcessing = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_timeout = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txt_path
            // 
            this.txt_path.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_path.Location = new System.Drawing.Point(236, 57);
            this.txt_path.Name = "txt_path";
            this.txt_path.Size = new System.Drawing.Size(529, 35);
            this.txt_path.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(91, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Image Path";
            // 
            // txt_middle
            // 
            this.txt_middle.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_middle.Location = new System.Drawing.Point(236, 124);
            this.txt_middle.Name = "txt_middle";
            this.txt_middle.Size = new System.Drawing.Size(529, 35);
            this.txt_middle.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(91, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "MildChar";
            // 
            // btn_cancle
            // 
            this.btn_cancle.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_cancle.Location = new System.Drawing.Point(280, 574);
            this.btn_cancle.Name = "btn_cancle";
            this.btn_cancle.Size = new System.Drawing.Size(153, 39);
            this.btn_cancle.TabIndex = 2;
            this.btn_cancle.Text = "Cancle";
            this.btn_cancle.UseVisualStyleBackColor = true;
            this.btn_cancle.Click += new System.EventHandler(this.btn_cancle_Click);
            // 
            // btn_save
            // 
            this.btn_save.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_save.Location = new System.Drawing.Point(472, 574);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(153, 39);
            this.btn_save.TabIndex = 2;
            this.btn_save.Text = "Save";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // txt_end
            // 
            this.txt_end.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_end.Location = new System.Drawing.Point(236, 191);
            this.txt_end.Name = "txt_end";
            this.txt_end.Size = new System.Drawing.Size(529, 35);
            this.txt_end.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(91, 194);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 24);
            this.label3.TabIndex = 1;
            this.label3.Text = "EndChar";
            // 
            // txt_port
            // 
            this.txt_port.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_port.Location = new System.Drawing.Point(236, 258);
            this.txt_port.Name = "txt_port";
            this.txt_port.Size = new System.Drawing.Size(529, 35);
            this.txt_port.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(91, 261);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 24);
            this.label4.TabIndex = 1;
            this.label4.Text = "Port";
            // 
            // cb_readType
            // 
            this.cb_readType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_readType.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb_readType.FormattingEnabled = true;
            this.cb_readType.Location = new System.Drawing.Point(236, 325);
            this.cb_readType.Name = "cb_readType";
            this.cb_readType.Size = new System.Drawing.Size(529, 32);
            this.cb_readType.TabIndex = 3;
            // 
            // cb_barcodeFormat
            // 
            this.cb_barcodeFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_barcodeFormat.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb_barcodeFormat.FormattingEnabled = true;
            this.cb_barcodeFormat.Location = new System.Drawing.Point(236, 389);
            this.cb_barcodeFormat.Name = "cb_barcodeFormat";
            this.cb_barcodeFormat.Size = new System.Drawing.Size(529, 32);
            this.cb_barcodeFormat.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(91, 328);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 24);
            this.label5.TabIndex = 1;
            this.label5.Text = "ReadType";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(91, 392);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 24);
            this.label6.TabIndex = 1;
            this.label6.Text = "CodeType";
            // 
            // chk_enableImageProcessing
            // 
            this.chk_enableImageProcessing.AutoSize = true;
            this.chk_enableImageProcessing.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chk_enableImageProcessing.Location = new System.Drawing.Point(236, 456);
            this.chk_enableImageProcessing.Name = "chk_enableImageProcessing";
            this.chk_enableImageProcessing.Size = new System.Drawing.Size(262, 28);
            this.chk_enableImageProcessing.TabIndex = 4;
            this.chk_enableImageProcessing.Text = "Enable Image Processing";
            this.chk_enableImageProcessing.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(91, 514);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(118, 24);
            this.label7.TabIndex = 1;
            this.label7.Text = "Timeout(s)";
            // 
            // txt_timeout
            // 
            this.txt_timeout.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_timeout.Location = new System.Drawing.Point(236, 511);
            this.txt_timeout.Name = "txt_timeout";
            this.txt_timeout.Size = new System.Drawing.Size(529, 35);
            this.txt_timeout.TabIndex = 0;
            // 
            // frm_Config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 659);
            this.Controls.Add(this.chk_enableImageProcessing);
            this.Controls.Add(this.cb_barcodeFormat);
            this.Controls.Add(this.cb_readType);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.btn_cancle);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_timeout);
            this.Controls.Add(this.txt_port);
            this.Controls.Add(this.txt_end);
            this.Controls.Add(this.txt_middle);
            this.Controls.Add(this.txt_path);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frm_Config";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frm_Config";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_path;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_middle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_cancle;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.TextBox txt_end;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_port;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cb_readType;
        private System.Windows.Forms.ComboBox cb_barcodeFormat;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chk_enableImageProcessing;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_timeout;
    }
}