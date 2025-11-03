using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace SdkDemoOled
{
    public partial class Form1 : Form
    {
        private class CameraSession
        {
            public CameraSession(string id, IntPtr handle)
            {
                Id = id;
                Handle = handle;
                Buffer = Array.Empty<byte>();
            }

            public string Id { get; }
            public IntPtr Handle { get; }
            public uint BufferLength { get; set; }
            public byte[] Buffer { get; set; }
            public uint LastWidth { get; set; }
            public uint LastHeight { get; set; }
            public uint LastBpp { get; set; }
            public uint LastChannels { get; set; }
            public object SyncRoot { get; } = new object();
        }

        private const int CONTROL_GAIN = 6;
        private const int CONTROL_EXPOSURE = 8;

        private readonly List<CameraSession> connectedCameras = new();
        private bool sdkInitialized;
        private Timer? captureTimer;
        private CancellationTokenSource? captureCts;
        private readonly object captureLock = new();
        private bool isCapturing;
        private double currentExposureSeconds;
        private string? captureOutputDirectory;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int sdkRet = InitQHYCCDResource();
            sdkInitialized = sdkRet == 0;
            UpdateStatus(sdkInitialized ? "SDK初始化成功" : $"SDK初始化失败: {sdkRet}");
            if (!sdkInitialized)
            {
                btnScan.Enabled = false;
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = false;
                btnStartCapture.Enabled = false;
                btnStopCapture.Enabled = false;
            }

            string defaultDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "QHYCCD");
            txtOutputDirectory.Text = defaultDirectory;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                StopCapturingInternal();
                DisconnectAll();
            }
            finally
            {
                if (sdkInitialized)
                {
                    ReleaseQHYCCDResource();
                }
            }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            if (!sdkInitialized)
            {
                UpdateStatus("SDK未初始化");
                return;
            }

            comboBoxCamera1.Items.Clear();
            comboBoxCamera2.Items.Clear();
            int cameraCount = ScanQHYCCD();
            if (cameraCount <= 0)
            {
                UpdateStatus("未找到任何相机");
                return;
            }

            for (int i = 0; i < cameraCount; i++)
            {
                StringBuilder id = new StringBuilder(64);
                if (GetQHYCCDId(i, id) == 0)
                {
                    string cameraId = id.ToString();
                    comboBoxCamera1.Items.Add(cameraId);
                    comboBoxCamera2.Items.Add(cameraId);
                }
            }

            if (comboBoxCamera1.Items.Count > 0)
            {
                comboBoxCamera1.SelectedIndex = 0;
            }

            if (comboBoxCamera2.Items.Count > 1)
            {
                comboBoxCamera2.SelectedIndex = 1;
            }
            else if (comboBoxCamera2.Items.Count > 0)
            {
                comboBoxCamera2.SelectedIndex = 0;
            }

            UpdateStatus($"扫描到 {comboBoxCamera1.Items.Count} 台相机");
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (isCapturing)
            {
                UpdateStatus("正在拍摄，请先停止");
                return;
            }

            string? camera1Id = comboBoxCamera1.SelectedItem as string;
            string? camera2Id = comboBoxCamera2.SelectedItem as string;

            if (string.IsNullOrWhiteSpace(camera1Id) || string.IsNullOrWhiteSpace(camera2Id))
            {
                UpdateStatus("请选择两台需要连接的相机");
                return;
            }

            if (camera1Id == camera2Id)
            {
                UpdateStatus("请选择不同的两台相机");
                return;
            }

            try
            {
                DisconnectAll();
                ConnectCamera(camera1Id);
                ConnectCamera(camera2Id);
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
                btnStartCapture.Enabled = true;
                UpdateStatus("两台相机连接成功");

                ApplyGainToConnectedCameras(showSuccessMessage: false);
            }
            catch (Exception ex)
            {
                DisconnectAll();
                UpdateStatus($"连接失败: {ex.Message}");
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            StopCapturingInternal();
            DisconnectAll();
            btnConnect.Enabled = true;
            btnDisconnect.Enabled = false;
            btnStartCapture.Enabled = false;
            btnStopCapture.Enabled = false;
            UpdateStatus("已断开所有相机");
        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                Description = "选择保存FITS文件的目标目录",
                SelectedPath = Directory.Exists(txtOutputDirectory.Text) ? txtOutputDirectory.Text : Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                txtOutputDirectory.Text = dialog.SelectedPath;
            }
        }

        private void btnStartCapture_Click(object sender, EventArgs e)
        {
            if (connectedCameras.Count != 2)
            {
                UpdateStatus("请先连接两台相机");
                return;
            }

            if (isCapturing)
            {
                UpdateStatus("拍摄已经在进行中");
                return;
            }

            string? outputDirectory = GetOutputDirectory();
            if (outputDirectory == null)
            {
                return;
            }

            double frequency = (double)numericFrequency.Value;
            if (frequency <= 0.0)
            {
                UpdateStatus("频率必须大于0");
                return;
            }

            currentExposureSeconds = (double)numericExposure.Value / 1000.0;

            if (!ApplyGainToConnectedCameras(showSuccessMessage: false))
            {
                return;
            }

            double exposureMicros = currentExposureSeconds * 1_000_000.0;

            foreach (CameraSession session in connectedCameras)
            {
                uint ret = SetQHYCCDParam(session.Handle, CONTROL_EXPOSURE, exposureMicros);
                if (ret != 0)
                {
                    UpdateStatus($"设置曝光失败: {ret}");
                    return;
                }
            }

            captureOutputDirectory = outputDirectory;
            StartCapturingInternal(frequency);
        }

        private void btnStopCapture_Click(object sender, EventArgs e)
        {
            StopCapturingInternal();
        }

        private void StartCapturingInternal(double frequency)
        {
            StopCapturingInternal();

            double intervalMs = 1000.0 / frequency;
            captureCts = new CancellationTokenSource();
            captureTimer = new Timer(intervalMs)
            {
                AutoReset = true
            };
            captureTimer.Elapsed += CaptureTimerElapsed;
            captureTimer.Start();
            isCapturing = true;
            btnStartCapture.Enabled = false;
            btnStopCapture.Enabled = true;
            UpdateStatus($"同步拍摄已开始，频率 {frequency:F2} Hz");

            // 立即进行一次拍摄，确保初始时刻同步
            Task.Run(() => CaptureFrameSequence(DateTime.UtcNow));
        }

        private void StopCapturingInternal()
        {
            if (!isCapturing)
            {
                return;
            }

            captureTimer?.Stop();
            captureTimer?.Dispose();
            captureTimer = null;
            captureCts?.Cancel();
            captureCts?.Dispose();
            captureCts = null;
            isCapturing = false;
            captureOutputDirectory = null;
            btnStartCapture.Enabled = connectedCameras.Count == 2;
            btnStopCapture.Enabled = false;
            UpdateStatus("拍摄已停止");
        }

        private void CaptureTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (captureCts?.IsCancellationRequested ?? true)
            {
                return;
            }

            CaptureFrameSequence(DateTime.UtcNow);
        }

        private void CaptureFrameSequence(DateTime timestampUtc)
        {
            if (captureCts?.IsCancellationRequested ?? false)
            {
                return;
            }

            if (!Monitor.TryEnter(captureLock))
            {
                return;
            }

            try
            {
                string? outputDirectory = captureOutputDirectory;
                if (string.IsNullOrEmpty(outputDirectory))
                {
                    return;
                }

                string timestampKey = timestampUtc.ToString("yyyyMMdd_HHmmss_fff");
                CancellationToken token = captureCts?.Token ?? CancellationToken.None;

                Task[] tasks = new Task[connectedCameras.Count];
                for (int i = 0; i < connectedCameras.Count; i++)
                {
                    CameraSession session = connectedCameras[i];
                    tasks[i] = Task.Run(() => CaptureSingleFrame(session, timestampUtc, timestampKey, token, outputDirectory), token);
                }

                Task.WaitAll(tasks);
                UpdateStatus($"已完成 {timestampKey} 的同步拍摄");
            }
            catch (Exception ex)
            {
                UpdateStatus($"拍摄失败: {ex.Message}");
                BeginInvoke(new Action(StopCapturingInternal));
            }
            finally
            {
                Monitor.Exit(captureLock);
            }
        }

        private void CaptureSingleFrame(CameraSession session, DateTime timestampUtc, string timestampKey, CancellationToken token, string outputDirectory)
        {
            if (token.IsCancellationRequested)
            {
                return;
            }

            lock (session.SyncRoot)
            {
                uint expRet = ExpQHYCCDSingleFrame(session.Handle);
                if (expRet != 0)
                {
                    throw new InvalidOperationException($"相机 {session.Id} 曝光失败: {expRet}");
                }

                uint width = 0;
                uint height = 0;
                uint bpp = 0;
                uint channels = 0;

                if (session.Buffer.Length < session.BufferLength || session.BufferLength == 0)
                {
                    session.BufferLength = Math.Max(session.BufferLength, GetQHYCCDMemLength(session.Handle));
                    if (session.BufferLength == 0)
                    {
                        session.BufferLength = 1024 * 1024;
                    }
                    session.Buffer = new byte[session.BufferLength];
                }

                uint getRet = GetQHYCCDSingleFrame(session.Handle, ref width, ref height, ref bpp, ref channels, session.Buffer);
                if (getRet != 0)
                {
                    throw new InvalidOperationException($"相机 {session.Id} 读取帧失败: {getRet}");
                }

                session.LastWidth = width;
                session.LastHeight = height;
                session.LastBpp = bpp;
                session.LastChannels = channels;

                SaveFits(session, timestampUtc, timestampKey, session.Buffer, width, height, bpp, channels, outputDirectory);
            }
        }

        private void SaveFits(CameraSession session, DateTime timestampUtc, string timestampKey, byte[] buffer, uint width, uint height, uint bpp, uint channels, string outputDirectory)
        {
            if (channels != 1)
            {
                throw new NotSupportedException("当前仅支持单通道图像保存为FITS");
            }

            int bytesPerPixel = (int)(bpp / 8);
            if (bytesPerPixel <= 0)
            {
                bytesPerPixel = 1;
            }

            long dataSize = (long)width * height * bytesPerPixel;
            if (buffer.LongLength < dataSize)
            {
                throw new InvalidOperationException("图像数据长度不足，无法保存为FITS");
            }

            if (dataSize > int.MaxValue)
            {
                throw new InvalidOperationException("图像数据过大，无法保存为FITS");
            }

            int dataLength = (int)dataSize;

            Directory.CreateDirectory(outputDirectory);
            string fileName = $"{timestampKey}_{MakeSafeFileName(session.Id)}.fits";
            string filePath = Path.Combine(outputDirectory, fileName);

            using FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            WriteFits(stream, buffer, (int)width, (int)height, bytesPerPixel, dataLength, timestampUtc);
        }

        private void WriteFits(Stream stream, byte[] buffer, int width, int height, int bytesPerPixel, int dataLength, DateTime timestampUtc)
        {
            List<string> cards = new List<string>
            {
                FormatFitsLogical("SIMPLE", true),
                FormatFitsInteger("BITPIX", bytesPerPixel == 2 ? 16 : 8),
                FormatFitsInteger("NAXIS", 2),
                FormatFitsInteger("NAXIS1", width),
                FormatFitsInteger("NAXIS2", height),
                FormatFitsLogical("EXTEND", true),
                FormatFitsString("DATE-OBS", timestampUtc.ToString("yyyy-MM-ddTHH:mm:ss.fff")),
                FormatFitsDouble("EXPTIME", currentExposureSeconds, 6),
                FormatFitsComment("Generated by QHYCCD Dual Camera Capture")
            };
            cards.Add("END".PadRight(80, ' '));

            byte[] header = BuildFitsHeader(cards);
            stream.Write(header, 0, header.Length);

            if (bytesPerPixel == 2)
            {
                for (int i = 0; i < dataLength; i += 2)
                {
                    stream.WriteByte(buffer[i + 1]);
                    stream.WriteByte(buffer[i]);
                }
            }
            else
            {
                stream.Write(buffer, 0, dataLength);
            }

            long padding = 2880 - (stream.Length % 2880);
            if (padding > 0 && padding < 2880)
            {
                byte[] pad = new byte[padding];
                stream.Write(pad, 0, pad.Length);
            }
        }

        private static byte[] BuildFitsHeader(IReadOnlyList<string> cards)
        {
            string concatenated = string.Concat(cards);
            int blocks = (concatenated.Length + 2879) / 2880;
            byte[] header = new byte[blocks * 2880];
            Encoding.ASCII.GetBytes(concatenated, 0, concatenated.Length, header, 0);
            return header;
        }

        private static string FormatFitsLogical(string key, bool value)
        {
            return BuildFitsCard(key, value ? "T" : "F", alignRight: true);
        }

        private static string FormatFitsInteger(string key, int value)
        {
            return BuildFitsCard(key, value.ToString(CultureInfo.InvariantCulture), alignRight: true);
        }

        private static string FormatFitsDouble(string key, double value, int precision)
        {
            string formatted = value.ToString($"F{precision}", CultureInfo.InvariantCulture);
            return BuildFitsCard(key, formatted, alignRight: true);
        }

        private static string FormatFitsString(string key, string value)
        {
            string sanitized = SanitizeToAscii(value, preserveQuotes: true).Trim();
            sanitized = sanitized.Replace("'", "''");
            if (sanitized.Length > 68)
            {
                sanitized = sanitized.Substring(0, 68);
            }
            string quoted = $"'{sanitized}'";

            return BuildFitsCard(key, quoted, alignRight: false);
        }

        private static string FormatFitsComment(string comment)
        {
            string sanitized = SanitizeToAscii(comment);
            if (sanitized.Length > 72)
            {
                sanitized = sanitized.Substring(0, 72);
            }

            return $"COMMENT {sanitized}".PadRight(80);
        }

        private static string BuildFitsCard(string key, string valueField, bool alignRight)
        {
            string normalizedKey = NormalizeFitsKey(key);
            string asciiValue = SanitizeToAscii(valueField, preserveQuotes: true).TrimEnd();
            string prefix = normalizedKey + "= ";

            if (alignRight && asciiValue.Length > 20)
            {
                asciiValue = asciiValue.Substring(0, 20);
            }

            string valuePortion = alignRight ? asciiValue.Trim().PadLeft(20) : asciiValue;
            string card = prefix + valuePortion;

            if (card.Length > 80)
            {
                card = card.Substring(0, 80);
            }
            else if (card.Length < 80)
            {
                card = card.PadRight(80);
            }

            return card;
        }

        private static string NormalizeFitsKey(string key)
        {
            string ascii = SanitizeToAscii(key).Trim().ToUpperInvariant();
            if (ascii.Length > 8)
            {
                ascii = ascii.Substring(0, 8);
            }

            return ascii.PadRight(8, ' ');
        }

        private static string SanitizeToAscii(string input, bool preserveQuotes = false)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder(input.Length);
            foreach (char c in input)
            {
                if (c >= 32 && c <= 126)
                {
                    if (!preserveQuotes && c == '\'')
                    {
                        builder.Append(' ');
                    }
                    else
                    {
                        builder.Append(c);
                    }
                }
                else if (preserveQuotes && c == '\'')
                {
                    builder.Append('\'');
                }
                else
                {
                    builder.Append(' ');
                }
            }

            return builder.ToString();
        }

        private bool ApplyGainToConnectedCameras(bool showSuccessMessage = true)
        {
            if (connectedCameras.Count == 0)
            {
                return true;
            }

            double gainValue = (double)numericGain.Value;

            foreach (CameraSession session in connectedCameras)
            {
                uint ret = SetQHYCCDParam(session.Handle, CONTROL_GAIN, gainValue);
                if (ret != 0)
                {
                    UpdateStatus($"设置增益失败: {ret}");
                    return false;
                }
            }

            if (showSuccessMessage)
            {
                UpdateStatus($"增益已设置为 {gainValue:F0}");
            }

            return true;
        }

        private void numericGain_ValueChanged(object? sender, EventArgs e)
        {
            ApplyGainToConnectedCameras();
        }

        private string? GetOutputDirectory()
        {
            string directory = txtOutputDirectory.Text.Trim();
            if (string.IsNullOrWhiteSpace(directory))
            {
                UpdateStatus("请先选择有效的输出目录");
                return null;
            }

            try
            {
                Directory.CreateDirectory(directory);
                return directory;
            }
            catch (Exception ex)
            {
                UpdateStatus($"创建输出目录失败: {ex.Message}");
                return null;
            }
        }

        private void ConnectCamera(string cameraId)
        {
            StringBuilder idBuilder = new StringBuilder(cameraId);
            IntPtr handle = OpenQHYCCD(idBuilder);
            if (handle == IntPtr.Zero)
            {
                throw new InvalidOperationException($"相机 {cameraId} 打开失败");
            }

            uint streamRet = SetQHYCCDStreamMode(handle, 0);
            if (streamRet != 0)
            {
                CloseQHYCCD(handle);
                throw new InvalidOperationException($"设置流模式失败: {streamRet}");
            }

            int initRet = InitQHYCCD(handle);
            if (initRet != 0)
            {
                CloseQHYCCD(handle);
                throw new InvalidOperationException($"相机初始化失败: {initRet}");
            }

            CameraSession session = new CameraSession(cameraId, handle);

            double chipW = 0;
            double chipH = 0;
            uint imageW = 0;
            uint imageH = 0;
            double pixelW = 0;
            double pixelH = 0;
            uint bpp = 0;
            uint chipRet = GetQHYCCDChipInfo(handle, ref chipW, ref chipH, ref imageW, ref imageH, ref pixelW, ref pixelH, ref bpp);
            if (chipRet == 0 && imageW > 0 && imageH > 0)
            {
                SetQHYCCDResolution(handle, 0, 0, imageW, imageH);
            }

            session.BufferLength = GetQHYCCDMemLength(handle);
            if (session.BufferLength == 0)
            {
                session.BufferLength = imageW * imageH * Math.Max(1u, bpp / 8);
            }

            if (session.BufferLength == 0)
            {
                session.BufferLength = 1024 * 1024;
            }

            session.Buffer = new byte[session.BufferLength];
            connectedCameras.Add(session);
        }

        private void DisconnectAll()
        {
            foreach (CameraSession session in connectedCameras)
            {
                try
                {
                    if (session.Handle != IntPtr.Zero)
                    {
                        CloseQHYCCD(session.Handle);
                    }
                }
                catch
                {
                    // 忽略关闭过程中的异常，确保资源释放
                }
            }

            connectedCameras.Clear();
        }

        private void UpdateStatus(string message)
        {
            string text = $"状态：{message}";
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => lblStatus.Text = text));
            }
            else
            {
                lblStatus.Text = text;
            }
        }

        private static string MakeSafeFileName(string name)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(c, '_');
            }

            return name;
        }

        [DllImport("qhyccd.dll", EntryPoint = "InitQHYCCDResource", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int InitQHYCCDResource();

        [DllImport("qhyccd.dll", EntryPoint = "ReleaseQHYCCDResource", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ReleaseQHYCCDResource();

        [DllImport("qhyccd.dll", EntryPoint = "ScanQHYCCD", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ScanQHYCCD();

        [DllImport("qhyccd.dll", EntryPoint = "GetQHYCCDId", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int GetQHYCCDId(int index, StringBuilder id);

        [DllImport("qhyccd.dll", EntryPoint = "OpenQHYCCD", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr OpenQHYCCD(StringBuilder id);

        [DllImport("qhyccd.dll", EntryPoint = "SetQHYCCDStreamMode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern uint SetQHYCCDStreamMode(IntPtr handle, uint mode);

        [DllImport("qhyccd.dll", EntryPoint = "InitQHYCCD", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int InitQHYCCD(IntPtr handle);

        [DllImport("qhyccd.dll", EntryPoint = "CloseQHYCCD", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int CloseQHYCCD(IntPtr handle);

        [DllImport("qhyccd.dll", EntryPoint = "SetQHYCCDParam", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern uint SetQHYCCDParam(IntPtr handle, int controlId, double value);

        [DllImport("qhyccd.dll", EntryPoint = "GetQHYCCDMemLength", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern uint GetQHYCCDMemLength(IntPtr handle);

        [DllImport("qhyccd.dll", EntryPoint = "GetQHYCCDChipInfo", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern uint GetQHYCCDChipInfo(IntPtr handle, ref double chipWidth, ref double chipHeight, ref uint imageWidth, ref uint imageHeight, ref double pixelWidth, ref double pixelHeight, ref uint bpp);

        [DllImport("qhyccd.dll", EntryPoint = "SetQHYCCDResolution", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern uint SetQHYCCDResolution(IntPtr handle, uint x, uint y, uint xsize, uint ysize);

        [DllImport("qhyccd.dll", EntryPoint = "ExpQHYCCDSingleFrame", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern uint ExpQHYCCDSingleFrame(IntPtr handle);

        [DllImport("qhyccd.dll", EntryPoint = "GetQHYCCDSingleFrame", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern uint GetQHYCCDSingleFrame(IntPtr handle, ref uint width, ref uint height, ref uint bpp, ref uint channels, byte[] dataBuffer);
    }
}
