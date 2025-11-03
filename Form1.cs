using System;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SdkDemoOled
{
    public partial class Form1 : Form
    {
        private int camScanNum = 0;
        private int retVal = -1;
        private bool isConnected = false;
        private IntPtr camHandle = IntPtr.Zero;

        // 参数变量
        double camTemp = 0.0;
        int camBin = 1;
        double camGain = 0.0;

        //控制id
        private const int CONTROL_GAIN = 6;      // 增益
        private const int CONTROL_CURTEMP = 14;  // 当前温度
        private const int CONTROL_BINX = 21;     // BinX
        private const int CONTROL_BINY = 22;     // BinY


        public Form1()
        {
            InitializeComponent();
        }

        // OLED相关P/Invoke
        [DllImport("qhyccd.dll", EntryPoint = "SendFourLine2QHYCCDInterCamOled",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 SendFourLine2QHYCCDInterCamOled(
            IntPtr handle,
            [MarshalAs(UnmanagedType.LPStr)] string messagetemp,
            [MarshalAs(UnmanagedType.LPStr)] string messageinfo,
            [MarshalAs(UnmanagedType.LPStr)] string messagetime,
            [MarshalAs(UnmanagedType.LPStr)] string messagemode
        );

        [DllImport("qhyccd.dll", EntryPoint = "QHYCCDInterCamOledOnOff",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 QHYCCDInterCamOledOnOff(IntPtr handle, byte onoff);

        [DllImport("qhyccd.dll", EntryPoint = "SetQHYCCDInterCamOledBrightness",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 SetQHYCCDInterCamOledBrightness(IntPtr handle, byte brightness);

        // 相机SDK基础P/Invoke
        [DllImport("qhyccd.dll", EntryPoint = "InitQHYCCDResource", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int InitQHYCCDResource();

        [DllImport("qhyccd.dll", EntryPoint = "ReleaseQHYCCDResource", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int ReleaseQHYCCDResource();

        [DllImport("qhyccd.dll", EntryPoint = "ScanQHYCCD", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int ScanQHYCCD();

        [DllImport("qhyccd.dll", EntryPoint = "GetQHYCCDId", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetQHYCCDId(int index, StringBuilder id);

        [DllImport("qhyccd.dll", EntryPoint = "OpenQHYCCD", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr OpenQHYCCD(StringBuilder id);

        [DllImport("qhyccd.dll", EntryPoint = "InitQHYCCD", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int InitQHYCCD(IntPtr handle);

        [DllImport("qhyccd.dll", EntryPoint = "CloseQHYCCD", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int CloseQHYCCD(IntPtr handle);

        // 参数获取
        [DllImport("qhyccd.dll", EntryPoint = "GetQHYCCDParam", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern double GetQHYCCDParam(IntPtr handle, int controlId);

        // FPGA版本获取
        [DllImport("qhyccd.dll", EntryPoint = "GetQHYCCDFPGAVersion", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern uint GetQHYCCDFPGAVersion(IntPtr handle, byte fpgaIndex, byte[] buf);

        private void Form1_Load(object sender, EventArgs e)
        {
            int sdkRet = InitQHYCCDResource();
            if (sdkRet != 0)
                lblStatus.Text = "SDK初始化失败";
            else
                lblStatus.Text = "SDK初始化成功";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ReleaseQHYCCDResource();
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            comboBoxCameraList.Items.Clear();
            camScanNum = ScanQHYCCD();
            if (camScanNum <= 0)
            {
                lblStatus.Text = "未检测到相机";
                return;
            }
            lblStatus.Text = $"发现 {camScanNum} 台相机";
            for (int i = 0; i < camScanNum; i++)
            {
                StringBuilder id = new StringBuilder(64);
                retVal = GetQHYCCDId(i, id);
                if (retVal == 0)
                {
                    comboBoxCameraList.Items.Add(id.ToString());
                }
            }
            if (comboBoxCameraList.Items.Count > 0)
                comboBoxCameraList.SelectedIndex = 0;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (comboBoxCameraList.SelectedIndex < 0)
            {
                lblStatus.Text = "请选择相机";
                return;
            }
            StringBuilder id = new StringBuilder(comboBoxCameraList.SelectedItem.ToString());
            camHandle = OpenQHYCCD(id);
            if (camHandle == IntPtr.Zero)
            {
                lblStatus.Text = "打开相机失败";
                return;
            }
            retVal = InitQHYCCD(camHandle);
            if (retVal != 0)
            {
                lblStatus.Text = "初始化相机失败";
                return;
            }
            isConnected = true;
            UpdateCameraParams();
            lblStatus.Text = "连接成功\r\n" + GetFpgaVersion();
            btnConnect.Enabled = false;
            btnDisconnect.Enabled = true;
            btnOledOn.Enabled = true;
            btnOledOff.Enabled = true;
            btnSend.Enabled = true;
            trackBarBrightness.Enabled = true;
            label3.Text = GetFpgaVersion();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (!isConnected || camHandle == IntPtr.Zero)
            {
                lblStatus.Text = "未连接相机";
                return;
            }
            retVal = CloseQHYCCD(camHandle);
            if (retVal != 0)
            {
                lblStatus.Text = "断开失败";
            }
            else
            {
                lblStatus.Text = "已断开";
                isConnected = false;
                camHandle = IntPtr.Zero;
                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
                btnOledOn.Enabled = false;
                btnOledOff.Enabled = false;
                btnSend.Enabled = false;
                trackBarBrightness.Enabled = false;
            }
        }

        private void UpdateCameraParams()
        {
            if (camHandle == IntPtr.Zero) return;
            camTemp = GetQHYCCDParam(camHandle, CONTROL_CURTEMP);
            camGain = GetQHYCCDParam(camHandle, CONTROL_GAIN);
            int binX = (int)GetQHYCCDParam(camHandle, CONTROL_BINX);
            int binY = (int)GetQHYCCDParam(camHandle, CONTROL_BINY);
            camBin = Math.Max(binX, binY);
        }

        private string GetFpgaVersion()
        {
            if (camHandle == IntPtr.Zero) return "未连接";
            byte[] buf = new byte[4];
            uint ret = GetQHYCCDFPGAVersion(camHandle, 0, buf);
            if (ret == 0)
            {
                return $"FPGA版本: {buf[0] + 2000}.{buf[1]}.{buf[2]} (subday:{buf[3]})";
            }
            else
            {
                return $"FPGA版本获取失败，错误码:{ret}";
            }
        }

        private string ReplaceVariables(string input)
        {
            return input
                .Replace("{temp}", camTemp.ToString("F1"))
                .Replace("{bin}", camBin.ToString())
                .Replace("{gain}", camGain.ToString("F1"))
                .Replace("{time}", DateTime.Now.ToString("HH:mm:ss"))
                .Replace("{date}", DateTime.Now.ToString("yyyy-MM-dd"));
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                lblStatus.Text = "请先连接相机";
                return;
            }
            UpdateCameraParams();
            string line1 = ReplaceVariables(txtLine1.Text);
            string line2 = ReplaceVariables(txtLine2.Text);
            string line3 = ReplaceVariables(txtLine3.Text);
            string line4 = ReplaceVariables(txtLine4.Text);

            UInt32 ret = SendFourLine2QHYCCDInterCamOled(camHandle, line1, line2, line3, line4);
            if (ret == 0)
                lblStatus.Text = "OLED发送成功";
            else
                lblStatus.Text = "OLED发送失败，错误码：" + ret;
        }

        private void btnOledOn_Click(object sender, EventArgs e)
        {
            if (!isConnected) return;
            QHYCCDInterCamOledOnOff(camHandle, 1);
            lblStatus.Text = "OLED已打开";
        }

        private void btnOledOff_Click(object sender, EventArgs e)
        {
            if (!isConnected) return;
            QHYCCDInterCamOledOnOff(camHandle, 0);
            lblStatus.Text = "OLED已关闭";
        }

        private void trackBarBrightness_Scroll(object sender, EventArgs e)
        {
            if (!isConnected) return;
            SetQHYCCDInterCamOledBrightness(camHandle, (byte)trackBarBrightness.Value);
            lblBrightness.Text = "亮度: " + trackBarBrightness.Value;
        }

        private void lblVars_Click(object sender, EventArgs e)
        {

        }

    }
}
