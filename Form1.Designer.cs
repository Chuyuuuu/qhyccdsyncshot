namespace SdkDemoOled
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.ComboBox comboBoxCamera1;
        private System.Windows.Forms.ComboBox comboBoxCamera2;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.NumericUpDown numericFrequency;
        private System.Windows.Forms.NumericUpDown numericExposure;
        private System.Windows.Forms.NumericUpDown numericGain;
        private System.Windows.Forms.TextBox txtOutputDirectory;
        private System.Windows.Forms.Button btnBrowseOutput;
        private System.Windows.Forms.Button btnStartCapture;
        private System.Windows.Forms.Button btnStopCapture;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblFrequency;
        private System.Windows.Forms.Label lblExposure;
        private System.Windows.Forms.Label lblGain;
        private System.Windows.Forms.Label lblOutput;
        private System.Windows.Forms.Label lblCamera1;
        private System.Windows.Forms.Label lblCamera2;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            btnScan = new Button();
            comboBoxCamera1 = new ComboBox();
            comboBoxCamera2 = new ComboBox();
            btnConnect = new Button();
            btnDisconnect = new Button();
            numericFrequency = new NumericUpDown();
            numericExposure = new NumericUpDown();
            numericGain = new NumericUpDown();
            txtOutputDirectory = new TextBox();
            btnBrowseOutput = new Button();
            btnStartCapture = new Button();
            btnStopCapture = new Button();
            lblStatus = new Label();
            lblFrequency = new Label();
            lblExposure = new Label();
            lblGain = new Label();
            lblOutput = new Label();
            lblCamera1 = new Label();
            lblCamera2 = new Label();
            ((System.ComponentModel.ISupportInitialize)numericFrequency).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericExposure).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericGain).BeginInit();
            SuspendLayout();
            //
            // btnScan
            //
            btnScan.Location = new Point(30, 79);
            btnScan.Name = "btnScan";
            btnScan.Size = new Size(136, 44);
            btnScan.TabIndex = 10;
            btnScan.Text = "扫描相机";
            btnScan.UseVisualStyleBackColor = true;
            btnScan.Click += btnScan_Click;
            //
            // comboBoxCamera1
            //
            comboBoxCamera1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxCamera1.FormattingEnabled = true;
            comboBoxCamera1.Location = new Point(175, 40);
            comboBoxCamera1.Name = "comboBoxCamera1";
            comboBoxCamera1.Size = new Size(293, 32);
            comboBoxCamera1.TabIndex = 0;
            //
            // comboBoxCamera2
            //
            comboBoxCamera2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxCamera2.FormattingEnabled = true;
            comboBoxCamera2.Location = new Point(175, 88);
            comboBoxCamera2.Name = "comboBoxCamera2";
            comboBoxCamera2.Size = new Size(293, 32);
            comboBoxCamera2.TabIndex = 1;
            //
            // btnConnect
            //
            btnConnect.Location = new Point(488, 40);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(136, 39);
            btnConnect.TabIndex = 2;
            btnConnect.Text = "连接两台相机";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            //
            // btnDisconnect
            //
            btnDisconnect.Enabled = false;
            btnDisconnect.Location = new Point(488, 88);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(136, 39);
            btnDisconnect.TabIndex = 3;
            btnDisconnect.Text = "断开连接";
            btnDisconnect.UseVisualStyleBackColor = true;
            btnDisconnect.Click += btnDisconnect_Click;
            //
            // numericFrequency
            //
            numericFrequency.DecimalPlaces = 2;
            numericFrequency.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            numericFrequency.Location = new Point(175, 148);
            numericFrequency.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            numericFrequency.Minimum = new decimal(new int[] { 1, 0, 0, 131072 });
            numericFrequency.Name = "numericFrequency";
            numericFrequency.Size = new Size(120, 30);
            numericFrequency.TabIndex = 4;
            numericFrequency.Value = new decimal(new int[] { 1, 0, 0, 0 });
            //
            // numericExposure
            //
            numericExposure.Location = new Point(175, 194);
            numericExposure.Maximum = new decimal(new int[] { 600000, 0, 0, 0 });
            numericExposure.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericExposure.Name = "numericExposure";
            numericExposure.Size = new Size(120, 30);
            numericExposure.TabIndex = 5;
            numericExposure.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            //
            // numericGain
            //
            numericGain.Location = new Point(175, 240);
            numericGain.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            numericGain.Name = "numericGain";
            numericGain.Size = new Size(120, 30);
            numericGain.TabIndex = 6;
            numericGain.Value = new decimal(new int[] { 10, 0, 0, 0 });
            numericGain.ValueChanged += numericGain_ValueChanged;
            //
            // txtOutputDirectory
            //
            txtOutputDirectory.Location = new Point(175, 286);
            txtOutputDirectory.Name = "txtOutputDirectory";
            txtOutputDirectory.Size = new Size(309, 30);
            txtOutputDirectory.TabIndex = 7;
            //
            // btnBrowseOutput
            //
            btnBrowseOutput.Location = new Point(488, 284);
            btnBrowseOutput.Name = "btnBrowseOutput";
            btnBrowseOutput.Size = new Size(136, 34);
            btnBrowseOutput.TabIndex = 8;
            btnBrowseOutput.Text = "选择目录";
            btnBrowseOutput.UseVisualStyleBackColor = true;
            btnBrowseOutput.Click += btnBrowseOutput_Click;
            //
            // btnStartCapture
            //
            btnStartCapture.Enabled = false;
            btnStartCapture.Location = new Point(175, 332);
            btnStartCapture.Name = "btnStartCapture";
            btnStartCapture.Size = new Size(136, 44);
            btnStartCapture.TabIndex = 9;
            btnStartCapture.Text = "开始同步拍摄";
            btnStartCapture.UseVisualStyleBackColor = true;
            btnStartCapture.Click += btnStartCapture_Click;
            //
            // btnStopCapture
            //
            btnStopCapture.Enabled = false;
            btnStopCapture.Location = new Point(317, 332);
            btnStopCapture.Name = "btnStopCapture";
            btnStopCapture.Size = new Size(136, 44);
            btnStopCapture.TabIndex = 10;
            btnStopCapture.Text = "停止拍摄";
            btnStopCapture.UseVisualStyleBackColor = true;
            btnStopCapture.Click += btnStopCapture_Click;
            //
            // lblStatus
            //
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(30, 396);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(62, 24);
            lblStatus.TabIndex = 18;
            lblStatus.Text = "状态：";
            //
            // lblFrequency
            //
            lblFrequency.AutoSize = true;
            lblFrequency.Location = new Point(30, 150);
            lblFrequency.Name = "lblFrequency";
            lblFrequency.Size = new Size(123, 24);
            lblFrequency.TabIndex = 19;
            lblFrequency.Text = "同步频率 (Hz)";
            //
            // lblExposure
            //
            lblExposure.AutoSize = true;
            lblExposure.Location = new Point(30, 196);
            lblExposure.Name = "lblExposure";
            lblExposure.Size = new Size(139, 24);
            lblExposure.TabIndex = 20;
            lblExposure.Text = "曝光时间 (ms)";
            //
            // lblGain
            //
            lblGain.AutoSize = true;
            lblGain.Location = new Point(30, 242);
            lblGain.Name = "lblGain";
            lblGain.Size = new Size(86, 24);
            lblGain.TabIndex = 21;
            lblGain.Text = "增益 (dB)";
            //
            // lblOutput
            //
            lblOutput.AutoSize = true;
            lblOutput.Location = new Point(30, 289);
            lblOutput.Name = "lblOutput";
            lblOutput.Size = new Size(107, 24);
            lblOutput.TabIndex = 22;
            lblOutput.Text = "输出目录";
            //
            // lblCamera1
            //
            lblCamera1.AutoSize = true;
            lblCamera1.Location = new Point(30, 43);
            lblCamera1.Name = "lblCamera1";
            lblCamera1.Size = new Size(86, 24);
            lblCamera1.TabIndex = 23;
            lblCamera1.Text = "相机1 ID";
            //
            // lblCamera2
            //
            lblCamera2.AutoSize = true;
            lblCamera2.Location = new Point(30, 91);
            lblCamera2.Name = "lblCamera2";
            lblCamera2.Size = new Size(86, 24);
            lblCamera2.TabIndex = 24;
            lblCamera2.Text = "相机2 ID";
            //
            // Form1
            //
            AutoScaleDimensions = new SizeF(10F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(651, 446);
            Controls.Add(lblCamera2);
            Controls.Add(lblCamera1);
            Controls.Add(lblOutput);
            Controls.Add(lblGain);
            Controls.Add(lblExposure);
            Controls.Add(lblFrequency);
            Controls.Add(lblStatus);
            Controls.Add(btnStopCapture);
            Controls.Add(btnStartCapture);
            Controls.Add(btnBrowseOutput);
            Controls.Add(txtOutputDirectory);
            Controls.Add(numericGain);
            Controls.Add(numericExposure);
            Controls.Add(numericFrequency);
            Controls.Add(btnDisconnect);
            Controls.Add(btnConnect);
            Controls.Add(comboBoxCamera2);
            Controls.Add(comboBoxCamera1);
            Controls.Add(btnScan);
            Name = "Form1";
            Text = "QHYCCD 双相机同步拍摄";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)numericFrequency).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericExposure).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericGain).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.ComponentModel.IContainer components = null;
    }
}
