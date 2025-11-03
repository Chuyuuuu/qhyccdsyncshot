namespace SdkDemoOled
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtLine1;
        private System.Windows.Forms.TextBox txtLine2;
        private System.Windows.Forms.TextBox txtLine3;
        private System.Windows.Forms.TextBox txtLine4;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblVars;
        private System.Windows.Forms.Button btnOledOn;
        private System.Windows.Forms.Button btnOledOff;
        private System.Windows.Forms.TrackBar trackBarBrightness;
        private System.Windows.Forms.Label lblBrightness;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.ComboBox comboBoxCameraList;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            txtLine1 = new TextBox();
            txtLine2 = new TextBox();
            txtLine3 = new TextBox();
            txtLine4 = new TextBox();
            btnSend = new Button();
            lblStatus = new Label();
            lblVars = new Label();
            btnOledOn = new Button();
            btnOledOff = new Button();
            trackBarBrightness = new TrackBar();
            lblBrightness = new Label();
            btnScan = new Button();
            comboBoxCameraList = new ComboBox();
            btnConnect = new Button();
            btnDisconnect = new Button();
            label1 = new Label();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)trackBarBrightness).BeginInit();
            SuspendLayout();
            // 
            // txtLine1
            // 
            txtLine1.Location = new Point(33, 144);
            txtLine1.Name = "txtLine1";
            txtLine1.PlaceholderText = "第1行内容";
            txtLine1.Size = new Size(351, 30);
            txtLine1.TabIndex = 0;
            // 
            // txtLine2
            // 
            txtLine2.Location = new Point(33, 189);
            txtLine2.Name = "txtLine2";
            txtLine2.PlaceholderText = "第2行内容";
            txtLine2.Size = new Size(351, 30);
            txtLine2.TabIndex = 1;
            // 
            // txtLine3
            // 
            txtLine3.Location = new Point(33, 234);
            txtLine3.Name = "txtLine3";
            txtLine3.PlaceholderText = "第3行内容";
            txtLine3.Size = new Size(351, 30);
            txtLine3.TabIndex = 2;
            // 
            // txtLine4
            // 
            txtLine4.Location = new Point(33, 284);
            txtLine4.Name = "txtLine4";
            txtLine4.PlaceholderText = "第4行内容";
            txtLine4.Size = new Size(351, 30);
            txtLine4.TabIndex = 3;
            // 
            // btnSend
            // 
            btnSend.Enabled = false;
            btnSend.Location = new Point(401, 129);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(185, 45);
            btnSend.TabIndex = 4;
            btnSend.Text = "发送到OLED";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // lblStatus
            // 
            lblStatus.Location = new Point(401, 287);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(185, 36);
            lblStatus.TabIndex = 5;
            lblStatus.Text = "状态：";
            // 
            // lblVars
            // 
            lblVars.Location = new Point(30, 10);
            lblVars.Name = "lblVars";
            lblVars.Size = new Size(365, 30);
            lblVars.TabIndex = 6;
            lblVars.Text = "可用变量: {temp} 温度, {bin} Bin模式";
            lblVars.Click += lblVars_Click;
            // 
            // btnOledOn
            // 
            btnOledOn.Enabled = false;
            btnOledOn.Location = new Point(401, 40);
            btnOledOn.Name = "btnOledOn";
            btnOledOn.Size = new Size(91, 38);
            btnOledOn.TabIndex = 7;
            btnOledOn.Text = "OLED开";
            btnOledOn.UseVisualStyleBackColor = true;
            btnOledOn.Click += btnOledOn_Click;
            // 
            // btnOledOff
            // 
            btnOledOff.Enabled = false;
            btnOledOff.Location = new Point(498, 40);
            btnOledOff.Name = "btnOledOff";
            btnOledOff.Size = new Size(88, 38);
            btnOledOff.TabIndex = 8;
            btnOledOff.Text = "OLED关";
            btnOledOff.UseVisualStyleBackColor = true;
            btnOledOff.Click += btnOledOff_Click;
            // 
            // trackBarBrightness
            // 
            trackBarBrightness.Enabled = false;
            trackBarBrightness.Location = new Point(390, 214);
            trackBarBrightness.Maximum = 255;
            trackBarBrightness.Name = "trackBarBrightness";
            trackBarBrightness.Size = new Size(196, 69);
            trackBarBrightness.TabIndex = 9;
            trackBarBrightness.Value = 128;
            trackBarBrightness.Scroll += trackBarBrightness_Scroll;
            // 
            // lblBrightness
            // 
            lblBrightness.Location = new Point(401, 182);
            lblBrightness.Name = "lblBrightness";
            lblBrightness.Size = new Size(117, 37);
            lblBrightness.TabIndex = 10;
            lblBrightness.Text = "亮度: 128";
            // 
            // btnScan
            // 
            btnScan.Location = new Point(30, 79);
            btnScan.Name = "btnScan";
            btnScan.Size = new Size(136, 44);
            btnScan.TabIndex = 11;
            btnScan.Text = "扫描相机";
            btnScan.UseVisualStyleBackColor = true;
            btnScan.Click += btnScan_Click;
            // 
            // comboBoxCameraList
            // 
            comboBoxCameraList.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxCameraList.FormattingEnabled = true;
            comboBoxCameraList.Location = new Point(175, 86);
            comboBoxCameraList.Name = "comboBoxCameraList";
            comboBoxCameraList.Size = new Size(220, 32);
            comboBoxCameraList.TabIndex = 12;
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(401, 84);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(91, 39);
            btnConnect.TabIndex = 13;
            btnConnect.Text = "连接";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // btnDisconnect
            // 
            btnDisconnect.Enabled = false;
            btnDisconnect.Location = new Point(498, 84);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(88, 39);
            btnDisconnect.TabIndex = 14;
            btnDisconnect.Text = "断开";
            btnDisconnect.UseVisualStyleBackColor = true;
            btnDisconnect.Click += btnDisconnect_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(30, 40);
            label1.Name = "label1";
            label1.Size = new Size(303, 24);
            label1.TabIndex = 15;
            label1.Text = "{gain} 增益, {time} 时间, {date} 日期";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(33, 331);
            label3.Name = "label3";
            label3.Size = new Size(0, 24);
            label3.TabIndex = 17;
            // 
            // Form1
            // 
            ClientSize = new Size(639, 377);
            Controls.Add(label3);
            Controls.Add(label1);
            Controls.Add(btnDisconnect);
            Controls.Add(btnConnect);
            Controls.Add(comboBoxCameraList);
            Controls.Add(btnScan);
            Controls.Add(lblBrightness);
            Controls.Add(trackBarBrightness);
            Controls.Add(btnOledOff);
            Controls.Add(btnOledOn);
            Controls.Add(lblVars);
            Controls.Add(lblStatus);
            Controls.Add(btnSend);
            Controls.Add(txtLine4);
            Controls.Add(txtLine3);
            Controls.Add(txtLine2);
            Controls.Add(txtLine1);
            Name = "Form1";
            Text = "QHYCCD 695A OLED";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)trackBarBrightness).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label label1;
        private Label label3;
    }
}
