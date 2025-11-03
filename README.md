# qhyccdsyncshot

## Overview / 项目简介

**qhyccdsyncshot** is a Windows Forms utility for synchronising image capture across two QHYCCD scientific cameras. It wraps the official `qhyccd.dll` SDK to scan for connected cameras, pair two selected devices, keep their gain/exposure settings in sync, and record simultaneous single-frame exposures as FITS images with aligned timestamps. The application is designed for laboratory or observatory setups where deterministic dual-camera workflows are required.

**qhyccdsyncshot** 是一款用于在两台 QHYCCD 科学相机之间实现同步拍摄的 Windows 窗体应用。程序基于官方 `qhyccd.dll` SDK，负责扫描连接的相机、配对两台设备、保持增益与曝光参数一致，并按统一时间戳保存同步曝光的单帧图像（FITS 格式），适用于实验室或台站等需要确定性双机流程的场景。

## Features / 功能特点

- Dual-camera discovery and pairing with validation to prevent duplicate selections.
- Live configuration of exposure (ms), gain, and capture frequency shared by both cameras.
- Coordinated single-frame exposures driven by a high-resolution timer and protected by concurrency controls.
- Automatic FITS generation (including headers such as `EXPTIME`, `DATE-OBS`) with byte-order handling for 16-bit data.
- Graceful resource cleanup for the QHYCCD SDK when the UI closes or cameras disconnect.

- 支持双相机扫描与配对，并避免选择相同设备。
- 实时设置曝光时间（毫秒）、增益和拍摄频率，自动同步至两台相机。
- 使用高精度定时器驱动同步单帧曝光，并通过并发锁保障线程安全。
- 自动生成 FITS 文件（包含 `EXPTIME`、`DATE-OBS` 等头信息），并处理 16 位数据的字节序。
- 在窗口关闭或断开连接时，可靠释放 QHYCCD SDK 资源。

## Repository layout / 代码结构

| Path | Description |
|------|-------------|
| `Form1.cs` | Main Windows Forms logic, camera management, capture loop, FITS writer. |
| `Form1.Designer.cs` / `Form1.resx` | Auto-generated UI layout and resources. |
| `Program.cs` | Application entry point using .NET 8 Windows Forms bootstrap. |
| `qhyccd_c_wrapper/` | Native C/C++ wrapper project (Visual C++), ships headers and import library from the QHYCCD SDK. |
| `bin/`, `obj/` | Build outputs (can be cleaned). |

| 路径 | 说明 |
|------|------|
| `Form1.cs` | 主要业务逻辑：相机管理、采集流程、FITS 写入。 |
| `Form1.Designer.cs` / `Form1.resx` | 自动生成的界面布局与资源文件。 |
| `Program.cs` | .NET 8 Windows 窗体应用入口。 |
| `qhyccd_c_wrapper/` | 原生 C/C++ 包装项目，包含 QHYCCD SDK 的头文件和导入库。 |
| `bin/`、`obj/` | 编译输出（可按需清理）。 |

## Prerequisites / 环境依赖

1. **Windows 10/11 64-bit** with USB 3.0 ports for QHYCCD hardware.
2. **.NET SDK 8.0 (or newer)** – required for building the WinForms project.
3. **Visual Studio 2022** with the “.NET Desktop Development” workload (optional but recommended for GUI development and debugging).
4. **QHYCCD SDK for Windows** – download from QHYCCD support. Obtain the 64-bit package matching your camera models.
5. (Optional) **Visual Studio 2022 with Desktop development using C++** if you plan to rebuild the native wrapper under `qhyccd_c_wrapper`.

1. **Windows 10/11 64 位系统**，并提供 USB 3.0 接口连接 QHYCCD 相机。
2. **.NET SDK 8.0（或更新版本）**——用于编译 WinForms 项目。
3. 推荐安装 **Visual Studio 2022** 并勾选“.NET 桌面开发”工作负载，以便调试和可视化设计界面。
4. **QHYCCD Windows SDK**——前往 QHYCCD 官网下载与相机型号匹配的 64 位版本。
5. （可选）若需重新编译 `qhyccd_c_wrapper` 原生库，请在 Visual Studio 中安装“使用 C++ 的桌面开发”工作负载。

## Installation / 安装步骤

### 1. Install development tools / 安装开发工具

- Install the .NET 8 SDK from <https://dotnet.microsoft.com/download> (make sure the installer adds `dotnet` to your PATH).
- Install Visual Studio 2022 and select the **.NET Desktop Development** workload. If you plan to modify the native wrapper, also add **Desktop development with C++**.

- 前往 <https://dotnet.microsoft.com/download> 安装 .NET 8 SDK（确保安装后可以在命令行使用 `dotnet`）。
- 安装 Visual Studio 2022，并勾选 **.NET 桌面开发** 工作负载；若要编译原生包装器，请额外勾选 **使用 C++ 的桌面开发**。

### 2. Acquire the QHYCCD SDK / 获取 QHYCCD SDK

1. Download the latest Windows SDK package from QHYCCD’s official website or support portal.
2. Extract the archive; locate `qhyccd.dll`, `qhyccd.lib`, and accompanying configuration files.
3. Copy `qhyccd.dll` (and any other runtime DLLs it depends on) into the `bin/Debug/net8.0-windows/` and/or `bin/Release/net8.0-windows/` folders after building, or into a central location that is part of your system PATH.

1. 从 QHYCCD 官网或客服支持下载最新的 Windows SDK 压缩包。
2. 解压后找到 `qhyccd.dll`、`qhyccd.lib` 及相关配置文件。
3. 将 `qhyccd.dll`（及其依赖 DLL）复制到编译输出目录 `bin/Debug/net8.0-windows/`、`bin/Release/net8.0-windows/`，或放置在系统 PATH 能访问的位置。

### 3. (Optional) Build the native wrapper / （可选）编译原生包装器

The repository includes `qhyccd_c_wrapper.vcxproj` alongside headers provided by QHYCCD. If you need to regenerate the import library or adjust native interfaces:

1. Open `qhyccd_c_wrapper/qhyccd_c_wrapper.vcxproj` in Visual Studio 2022.
2. Configure the project for your target (e.g., **Release | x64**).
3. Ensure the project’s library search paths point to the extracted QHYCCD SDK.
4. Build the project to produce an updated `qhyccd_c_wrapper.lib`/DLL, and copy the outputs next to the WinForms executable.

仓库中附带的 `qhyccd_c_wrapper.vcxproj` 可以在需要时重新生成导入库或调整原生接口：

1. 在 Visual Studio 2022 中打开 `qhyccd_c_wrapper/qhyccd_c_wrapper.vcxproj`。
2. 选择目标配置（如 **Release | x64**）。
3. 确认库目录指向解压后的 QHYCCD SDK。
4. 编译项目，生成新的 `qhyccd_c_wrapper.lib`/DLL，并复制到 WinForms 可执行文件旁。

## Build & Run / 编译与运行

### Using Visual Studio / 使用 Visual Studio

1. Double-click `qhyccdsyncshot.sln` to open the solution.
2. Restore NuGet packages automatically (no external packages are required beyond the .NET SDK).
3. Select the desired build configuration (`Debug` or `Release`) and target (`x64` recommended for modern cameras).
4. Press **F5** to build and run. Ensure `qhyccd.dll` is discoverable at runtime.

1. 双击 `qhyccdsyncshot.sln` 打开解决方案。
2. 等待自动还原依赖（本项目仅依赖 .NET SDK）。
3. 选择需要的编译配置（`Debug` 或 `Release`）及平台（推荐 `x64`）。
4. 按 **F5** 编译并启动，确保运行时能够找到 `qhyccd.dll`。

### Using the .NET CLI / 使用 .NET 命令行

```powershell
# Restore dependencies / 还原依赖
 dotnet restore

# Build the WinForms app (x64 example) / 编译 WinForms 应用（以 x64 为例）
 dotnet build qhyccdsyncshot.sln -c Release -p:Platform=x64

# Run directly from CLI / 直接运行
 dotnet run --project qhyccdsyncshot.csproj -c Release -p:Platform=x64
```

> ℹ️ When running from the CLI, copy `qhyccd.dll` into `bin/Release/net8.0-windows/` (or the configuration you built). Without the native DLL, the application will fail to initialize the SDK.

> ℹ️ 通过命令行运行时，请将 `qhyccd.dll` 复制到对应的输出目录（如 `bin/Release/net8.0-windows/`），否则程序无法初始化 SDK。

## Usage workflow / 使用流程

1. Connect two QHYCCD cameras to the host PC.
2. Launch the application and wait for “SDK 初始化成功 / SDK initialized successfully” in the status bar.
3. Click **“扫描相机 / Scan”** to populate both camera dropdowns.
4. Choose two different camera IDs, then press **“连接 / Connect”**.
5. Adjust exposure (ms), gain, and capture frequency as needed.
6. Select an output directory for FITS files.
7. Press **“开始拍摄 / Start Capture”**. The program will trigger synchronized exposures at the selected frequency and write FITS pairs stamped with the same timestamp key.
8. Press **“停止拍摄 / Stop Capture”** before disconnecting hardware or exiting.

1. 将两台 QHYCCD 相机连接至主机。
2. 打开软件，确认状态栏显示“SDK 初始化成功”。
3. 点击 **“扫描相机”**，填充两个下拉列表。
4. 分别选择两台不同的相机，点击 **“连接”**。
5. 根据需要调整曝光时间（毫秒）、增益与拍摄频率。
6. 指定 FITS 文件的输出目录。
7. 点击 **“开始拍摄”**，程序会按照设定频率同步触发曝光，并以相同时间戳保存成对的 FITS 图像。
8. 在断开设备或退出软件前，请先点击 **“停止拍摄”**。

## Troubleshooting / 故障排查

- **SDK initialization failure** – Verify that `qhyccd.dll` and its dependencies (e.g., Cypress USB drivers) are installed and accessible. Run the executable from a directory containing the DLL.
- **Camera list is empty** – Confirm cameras are powered, drivers installed, and no other application is locking the devices.
- **Capture errors** – The status bar will display specific error codes from the SDK; refer to QHYCCD documentation for the corresponding definitions.
- **FITS writing exceptions** – Ensure the selected output directory is writable and has sufficient disk space. Only single-channel (mono) data is currently supported.

- **SDK 初始化失败**：确认 `qhyccd.dll` 及其依赖（如 Cypress USB 驱动）已正确安装，并位于可访问的路径中。
- **未扫描到相机**：检查相机电源、驱动安装情况，以及是否有其他软件占用设备。
- **拍摄报错**：状态栏会显示 SDK 返回的具体错误码，可对照 QHYCCD 官方文档排查。
- **保存 FITS 异常**：确认输出目录可写且磁盘空间充足。目前仅支持保存单通道（Mono）数据。

## License / 许可证

The repository inherits the original project license (see [`LICENSE`](LICENSE)). Ensure you also comply with the QHYCCD SDK license terms when redistributing the native libraries.

本仓库沿用原项目许可证（参见 [`LICENSE`](LICENSE)）。请同时遵循 QHYCCD SDK 的授权条款，避免未经允许分发原生库文件。

## Acknowledgements / 致谢

- QHYCCD for providing the camera SDK and technical documentation.
- Open-source FITS tooling references that informed the header-writing implementation.

- 感谢 QHYCCD 提供的相机 SDK 与技术资料。
- 感谢开源 FITS 工具带来的启发，使本项目得以实现 FITS 头写入功能。
