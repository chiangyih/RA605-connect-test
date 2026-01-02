# RA605-710-GB 連線測試 & LIN 運動測試

## 概覽
`RA605-connect-test2` 一個 WinForms 用戶端應用程式，用來測試和控制 HIWIN RA605 機械手臂。透過 HRSDK（Hiwin Robot SDK）的 `HRobot.dll`，應用程式可以實現連線、馬達控制、位置查詢和 LIN 直線運動等功能。工程師無需進入示教器就能驗證通訊協議、運動軌跡和安全邏輯。

---

## ⚠️ 重要注意事項

### 🔴 **模式切換必須設定為 EXT（外部模式）**

在使用本應用程式控制機械手臂前，**必須**在示教器上將操作模式切換到 **EXT（外部模式）**，否則所有外部命令都會被拒絕。


#### ⚠️ 常見問題排除
| 問題 | 原因 | 解決方案 |
|------|------|--------|
| 執行 LIN 時命令被拒絕 | 模式不是 EXT | 確認示教器操作模式為 EXT |
| 馬達無法開啟 | 模式設為 Manual（手動） | 切換到 EXT 模式並清除任何警報 |
| 命令返回錯誤碼 -1 | 可能的模式不匹配 | 檢查控制器和應用程式的模式設置 |

---

## 🖥️ 適用環境

本應用程式支援以下兩種環境：

### 環境 1️⃣：HRSS（虛擬模擬器）

**適用場景**：
- ✅ 開發和測試代碼邏輯
- ✅ 調試 UI 和通訊協議
- ✅ 驗證命令序列無需實機
- ✅ 離線開發和演示

**HRSS 特性**：
- 虛擬化的機械手臂環境
- 完整模擬 SDK 接口
- 無需連接實際硬體
- 可快速反覆迭代

**配置步驟**：
1. 在 Hiwin 官方網站下載並安裝 HRSS（Hiwin Robot Simulation System）
2. 啟動 HRSS 應用程式
3. 加載 RA605 機械手臂模型
4. 在 HRSS 中將操作模式設為 **EXT**
5. 記下 HRSS 中控制器的 IP 位址（通常為 localhost 或 127.0.0.1）
6. 在本應用中輸入該 IP 並連接

**HRSS 中的 EXT 模式設置**：

<img width="1201" height="490" alt="image" src="https://github.com/user-attachments/assets/4c98f308-c70a-4e82-a8f7-f9bda822d9d2" />



**注意**：
- HRSS 默認監聽 `localhost:9090` 或 `127.0.0.1`
- 可在 HRSS 設置中查看或修改監聽 IP 和 PORT
- 虛擬手臂的運動約束與實機相同

---

### 環境 2️⃣：手臂實機（實際硬體）

**適用場景**：
- ✅ 生產環境驗證
- ✅ 真實軌跡和速度測試
- ✅ 安全認證和驗收測試
- ✅ 工廠部署前的最後確認

**硬體需求**：
- HIWIN RA605 機械手臂控制器
- 網路連接到控制器（Ethernet）
- 匹配的 HRSDK.dll 版本
- 符合 RA605 規格的 .NET 10 環境

**配置步驟**：
1. 確認機械手臂和控制器已啟動
2. 在示教器上將操作模式切換到 **EXT**
3. 確認網路連接到控制器（可 ping 測試 IP）
4. 在本應用中輸入控制器的 IP 位址
5. 點擊 Connect 進行連接
6. 開啟馬達（Motor ON）進行測試

**實機環境的 EXT 模式設置**：
<img width="623" height="444" alt="image" src="https://github.com/user-attachments/assets/7a334812-62bd-4c6a-bc1f-5e54b61c8c60" />


**安全考量**：
- ⚠️ 確保人員在安全距離外
- ⚠️ 啟用緊急停止按鈕
- ⚠️ 檢查工作範圍無障礙物
- ⚠️ 驗證所有軟件限制設置正確
- ⚠️ 第一次測試應使用低速度

**實機特有的檢查**：
| 檢查項 | 重要性 | 操作 |
|------|-------|------|
| 物理安全 | ⚠️⚠️⚠️ 必須 | 檢查工作範圍、障礙物、人員位置 |
| 控制器電源 | ⚠️⚠️ 必須 | 確認控制器已通電且指示燈正常 |
| 網路連接 | ⚠️⚠️ 必須 | ping 控制器 IP 確認網路可達 |
| EXT 模式 | ⚠️⚠️ 必須 | 在示教器上驗證操作模式為 EXT |
| 馬達狀態 | ⚠️⚠️ 建議 | 在示教器上檢查馬達是否已啟動 |
| 警報清除 | ⚠️⚠️ 建議 | 清除任何存在的警報代碼 |
| 速度設置 | ⚠️ 建議 | 第一次測試設定較低速度（如 50 mm/s） |

---

## 🌐 環境選擇指南

### 我應該選擇哪個環境？

```
┌─────────────────────────────────────────────┐
│         你有實際的機械手臂嗎？               │
└────────────────────┬────────────────────────┘
                     │
         ┌───────────┴───────────┐
         │                       │
        否                       是
         │                       │
         ↓                       ↓
    ┌─────────┐           ┌──────────┐
    │ 使用 HRSS│           │ 使用 手臂│
    │ 虛擬環境 │           │ 實機環境 │
    └─────────┘           └──────────┘
         │                     │
         ├─ 快速測試           ├─ 完整驗證
         ├─ 離線開發           ├─ 安全認證
         └─ 低成本             └─ 生產部署

選擇建議：
    第一次使用 → HRSS
    代碼完成   → 手臂實機
    持續開發   → 兩者都用（HRSS 開發，實機驗證）
```

---

## 主要功能

### 1. 連線管理
- **預設 IP**：`192.168.203.200`（可自訂）
- **連線驗證**：自動顯示 SDK 版本及連線狀態
- **錯誤診斷**：詳細的連線失敗原因提示（網路、回呼機制、版本不符等）
- **安全斷開**：應用程式結束或按下結束按鈕時自動斷開連接

### 2. 馬達控制
- **馬達開啟 (Motor ON)**
  - 檢查目前馬達狀態
  - 若已開啟則提示用戶
  - 若開啟失敗會自動檢查警報代碼並提示原因
  
- **馬達關閉 (Motor OFF)**
  - 要求用戶確認（防止誤操作）
  - 關閉後驗證狀態變更
  - 若失敗會提示可能的原因（安全門、警報、狀態不正確）

- **狀態確認機制**
  - 發送命令後等待 500ms 再次檢查狀態
  - 若狀態未變更會顯示警告並檢查警報
  - 所有操作都會在日誌中記錄

### 3. 位置管理
- **取得當前位置 (Get Current Position)**
  - 一鍵查詢機械手臂即時座標（X, Y, Z, A, B, C）
  - 同時取得六個關節的角度（J1~J6）
  - 自動填入 LIN 運動測試的輸入框
  - 方便進行增量調整和預測性測試

### 4. LIN 直線運動測試
完整的運動流程包括：

#### 輸入驗證
- 座標輸入（X, Y, Z, A, B, C）必須為有效數字
- 速度輸入必須 > 0（mm/s）
- 運動模式和平滑值可選

#### 前置檢查
1. **馬達狀態檢查** - 確保馬達已開啟
2. **操作模式檢查** - 驗證手臂在自動模式（若不是會嘗試自動切換）
3. **運動狀態查詢** - 記錄目前的運動狀態
4. **可達性檢查** - 驗證目標點在工作範圍內且不會產生奇異點
5. **路徑檢查** - 驗證從目前位置到目標位置的直線路徑無障礙

#### 命令執行
- 先設定速度（mm/s）
- 執行 `lin_pos` 絕對座標運動
- 若命令失敗會自動檢查警報代碼

#### 完成等待
- 監控命令佇列直到為空
- 等效於示教器上「到點後才下一行」的效果
- 每 100ms 檢查一次佇列狀態

### 5. 日誌系統
- **時間戳記** - 每則訊息都帶有 HH:mm:ss.fff 格式的精確時間
- **多執行緒安全** - SDK 回呼函式的日誌透過 `BeginInvoke` 確保執行緒安全
- **診斷信息** - 包括返回碼、狀態值和警報代碼等詳細資訊
- **可滾動視窗** - 支援將日誌匯出以協助除錯

---

## 🏗️ 專案架構

```
RA605-connect-test/
│
├── RA605-connect-test2.csproj          # 專案配置檔（.NET 10 WinForms）
│
├── README.md                            # 本檔案（專案文檔）
├── .gitignore                           # Git 忽略清單
├── LICENSE                              # 授權條款（MIT）
│
└── 原始碼目錄
    ├── Form1.cs                         # ⭐ 主表單邏輯（680+ 行）
    ├── Form1.Designer.cs                # 自動生成的 UI 設計代碼
    ├── Form1.resx                       # 資源檔（圖標、本地化等）
    │
    ├── Program.cs                       # 應用程式進入點
    │
    ├── sdk/
    │   └── HRobot.cs                    # Hiwin SDK 包裝類（P/Invoke）
    │
    └── obj/, bin/                       # 編譯輸出目錄（自動生成）
```

### 目錄結構說明

| 目錄/檔案 | 說明 |
|---------|------|
| **根目錄** | 專案根資料夾，包含專案配置和文檔 |
| **RA605-connect-test2.csproj** | 專案檔，定義依賴、版本、編譯設置 |
| **README.md** | 專案文檔（你正在閱讀） |
| **.gitignore** | Git 版本控制的忽略檔案清單 |
| **LICENSE** | MIT 開源許可證 |
| **Form1.cs** | 主表單的業務邏輯和事件處理 |
| **Form1.Designer.cs** | WinForms 自動生成的 UI 元件代碼 |
| **Form1.resx** | WinForms 資源檔（本地化字符串、圖標等） |
| **Program.cs** | 應用程式進入點（Main 方法） |
| **sdk/HRobot.cs** | Hiwin SDK C# 包裝類 |
| **obj/** | 中間編譯檔案（自動生成，可刪除） |
| **bin/** | 最終輸出檔案（exe、dll 等） |

---

## 📄 檔案詳細說明

### 🔴 Form1.cs（主要邏輯檔案）

**職責**：應用程式的核心邏輯和 UI 事件處理

**行數**：680+ 行

**代碼結構** - 使用 `#region` 分組：

```csharp
#region Constants（常數）
    // 狀態碼、延遲時間、預設值等

#region Fields（欄位）
    // 機械手臂 ID、連線狀態

#region Connection Management（連接管理）
    private bool IsConnected
    private void EnsureConnected(string operationName)
    private void CallBackFun(...)
    private void btnConnect_Click(...)
    private void DisplaySdkVersion()
    private void LogConnectionErrorHints(...)
    private void SetConnectionUIState(...)
    private void btnDisconnect_Click(...)
    private void Disconnect()
    private void btnExit_Click(...)

#region Motor Control（馬達控制）
    private void btnMotorOn_Click(...)
    private void btnMotorOff_Click(...)
    private void SetMotorState(int state)
    private void HandleMotorStateNotConfirmed(...)
    private void HandleMotorCommandFailed(...)
    private void TryShowAlarms()

#region Position Management（位置管理）
    private void btnGetCurrentPos_Click(...)
    private void LoadCurrentPosition()
    private void SetPositionTextBoxes(...)
    private void LogCurrentPosition(...)
    private void LogCurrentJoints()

#region LIN Motion（直線運動）
    private void btnLinTest_Click(...)
    private void ExecuteLinMotion()
    private bool TryReadPositionAndSpeed(...)
    private int ParseLinMode()
    private bool CheckMotorState()
    private bool CheckAndSetOperationMode()
    private bool CheckTargetReachability(...)
    private bool CheckLinPath(...)
    private bool ExecuteLinCommand(...)
    private void WaitForMotionComplete()

#region Helper Methods（輔助方法）
    private bool TryReadDouble(...)
    private void AppendLog(string msg)
    private void AppendLogThreadSafe(string msg)
    protected override void OnFormClosing(...)
```

**關鍵特性**：
- ✅ 所有常數都有命名（無魔數）
- ✅ 每個方法單一職責
- ✅ 詳細的中文註解和 XML 文檔註釋
- ✅ 統一的錯誤處理和用戶提示
- ✅ 執行緒安全（使用 `volatile` 和 `BeginInvoke`）

**主要方法數量**：45+ 個（包括事件處理、邏輯、輔助方法）

### 🔵 Form1.Designer.cs

**職責**：WinForms 設計器自動生成的 UI 元件代碼

**特點**：
- 自動生成，**請勿手動編輯**
- 包含所有按鈕、文字框、標籤的初始化代碼
- 包含 Layout、字體、大小等設置

**典型的 UI 元件**：
```csharp
// 按鈕
private Button btnConnect;
private Button btnDisconnect;
private Button btnMotorOn;
private Button btnMotorOff;
private Button btnGetCurrentPos;
private Button btnLinTest;
private Button btnExit;

// 輸入框
private TextBox txtIP;
private TextBox txtX, txtY, txtZ, txtA, txtB, txtC;
private TextBox txtLinSpeed;
private TextBox txtSmooth;

// 下拉選單
private ComboBox cboLinMode;

// 顯示控件
private TextBox txtLog;
private Label lblStatus;

// 其他
private TableLayoutPanel layoutMain;
```

**生成方式**：透過 Visual Studio 的 WinForms 設計器生成

### 🟢 Form1.resx（資源檔）

**職責**：存儲 UI 相關的資源（本地化字符串、圖標、字體等）

**常見內容**：
- UI 元件的屬性序列化
- 本地化的字符串資源
- 圖標和圖像資源
- 字體和顏色定義

**格式**：XML 格式，由 Visual Studio 自動管理

**編輯方式**：
- 透過 Visual Studio 設計器（推薦）
- 或直接編輯 XML（不推薦）

### 🟡 Program.cs（應用程式進入點）

**職責**：應用程式的啟動點

**典型代碼**：
```csharp
using System;
using System.Windows.Forms;

namespace RA605_connect_test2
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 初始化 WinForms 應用程式
            ApplicationConfiguration.Initialize();
            
            // 啟動主表單
            Application.Run(new Form1());
        }
    }
}
```

**重要說明**：
- `[STAThread]` - Single-Threaded Apartment，WinForms 必需
- `ApplicationConfiguration.Initialize()` - .NET 新式初始化方式
- `Application.Run(new Form1())` - 啟動主表單並進入消息循環

**編輯提示**：通常無需修改此檔案

### 🟣 sdk/HRobot.cs（SDK 包裝類）

**職責**：Hiwin Robot SDK 的 C# 包裝和 P/Invoke 聲明

**內容示例**：
```csharp
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SDKHrobot
{
    /// <summary>
    /// Hiwin Robot SDK 的 P/Invoke 包裝類
    /// </summary>
    public static class HRobot
    {
        // P/Invoke DLL 名稱
        private const string DLL_NAME = "HRSDK.dll";
        
        // SDK 函式聲明
        [DllImport(DLL_NAME)]
        public static extern void get_hrsdk_version(StringBuilder ver);
        
        [DllImport(DLL_NAME)]
        public static extern int open_connection(
            string ip, int level, 
            HRobotSDK_Callback callback);
        
        [DllImport(DLL_NAME)]
        public static extern int disconnect(int robot_id);
        
        [DllImport(DLL_NAME)]
        public static extern int get_motor_state(int robot_id);
        
        [DllImport(DLL_NAME)]
        public static extern int set_motor_state(int robot_id, int state);
        
        // ... 更多 SDK 函式
        
        // 回呼函式委托
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void HRobotSDK_Callback(
            ushort cmd, ushort rlt, ref ushort msg, int len);
    }
}
```

**關鍵概念**：
- **P/Invoke** - 呼叫本機 DLL（C++ 編寫的 HRSDK.dll）
- **DllImport** - 聲明要導入的 DLL 函式
- **UnmanagedFunctionPointer** - 聲明本機回呼函式簽名

**常見的 SDK 函式**：
| 函式 | 說明 |
|------|------|
| `get_hrsdk_version()` | 取得 SDK 版本 |
| `open_connection()` | 連接手臂控制器 |
| `disconnect()` | 斷開連接 |
| `get_motor_state()` | 查詢馬達狀態 |
| `set_motor_state()` | 設定馬達狀態 |
| `get_current_position()` | 取得當前座標 |
| `lin_pos()` | 執行直線運動 |
| `get_command_count()` | 查詢命令佇列數量 |
| `get_alarm_code()` | 取得警報代碼 |

### 📋 RA605-connect-test2.csproj（專案配置）

**職責**：.NET 專案配置和依賴管理

**關鍵內容**：
```xml
<Project Sdk="Microsoft.NET.Sdk.WindowsDesktop">
  
  <!-- 專案屬性 -->
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net10.0-windows</TargetFramework>
    <RootNamespace>RA605_connect_test2</RootNamespace>
    <AssemblyName>RA605-connect-test2</AssemblyName>
    <LangVersion>latest</LangVersion>
    <UseWindowsForms>true</UseWindowsForms>
  </PropertyGroup>
  
  <!-- NuGet 套件依賴 -->
  <ItemGroup>
    <!-- WinForms 依賴會自動包含 -->
  </ItemGroup>
  
</Project>
```

**重要設置**：| 設定 | 值 | 說明 |
|------|----|----|
| `OutputType` | WinExe | 輸出 Windows 可執行檔 |
| `TargetFramework` | net10.0-windows | 目標 .NET 10（Windows 特定） |
| `UseWindowsForms` | true | 啟用 WinForms 支援 |
| `LangVersion` | latest | 使用最新 C# 語言特性 |

### 📝 README.md（專案文檔）

**職責**：專案文檔和使用指南

**包含內容**：
- ✅ 項目概覽和主要功能
- ✅ 適用環境說明（HRSS 和實機）
- ✅ 重要注意事項（EXT 模式設置）
- ✅ 技術架構說明
- ✅ 使用需求
- ✅ 建置和執行步驟
- ✅ 故障排除指南
- ✅ 開發指南

**格式**：Markdown（GitHub 相容）

### 🔒 LICENSE（許可證）

**內容**：MIT 開源許可證

**含義**：
- ✅ 可自由使用、修改、發佈
- ✅ 必須保留許可聲明
- ❌ 不提供任何保證

---

## 技術架構

### 代碼組織
應用程式使用 `#region` 進行邏輯分區：

| 區域 | 功能 |
|------|------|
| **Constants** | 所有常數定義（狀態碼、延遲時間等） |
| **Fields** | 私有欄位（機械手臂 ID、連線狀態） |
| **Connection Management** | 連線、斷開、SDK 版本查詢 |
| **Motor Control** | 馬達開關、狀態檢查、警報回報 |
| **Position Management** | 位置查詢、座標載入 |
| **LIN Motion** | 運動驗證、執行和完成等待 |
| **Helper Methods** | 日誌、參數讀取等輔助函式 |

### 設計原則
- **常數化** - 所有魔數都提取為命名常數便於維護
- **方法分解** - 大型方法拆分為單一職責的小方法
- **錯誤處理** - 統一的異常處理和用戶提示
- **執行緒安全** - 使用 `volatile` 和 `BeginInvoke` 確保多執行緒安全

---

## 使用需求

### 軟體環境
- **.NET 版本** - .NET 10
- **開發環境** - Visual Studio 2022 或更新版本（啟用 Windows Forms 工作負載）
- **Windows 版本** - Windows 7 或更新

### 硬體和外部依賴

#### 對於 HRSS（虛擬環境）：
- 任何可運行 HRSS 的 Windows PC
- Hiwin HRSS（Hiwin Robot Simulation System）已安裝
- RA605 機械手臂模型已在 HRSS 中加載
- **HRSDK.dll**（虛擬版本）

#### 對於手臂實機：
- **HRSDK** - 需安裝 Hiwin Robot SDK（實機版）
- **SDK 程式庫** - `HRSDK.dll` 必須在可執行檔旁或在 PATH 中
- **網路** - 能透過設定的 IP 位址存取手臂控制器
- **手臂控制器** - HIWIN RA605 或相容型號
- 控制器網路介面卡配置（確保與 PC 在同一子網）

---

## 建置步驟

### 1. 環境準備
```bash
# 確保已安裝 .NET 10 SDK
dotnet --version

# 確保已安裝 Visual Studio 2022
# 並啟用 Windows Forms 工作負載
```

### 2. 開啟專案
```bash
# 使用 Visual Studio 開啟
start RA605-connect-test2.csproj

# 或使用 Visual Studio Code
code .
```

### 3. 建置應用程式
```bash
# 在 Visual Studio 中：
# 選單 → Build → Build Solution（或按 Ctrl+Shift+B）

# 或在命令列中：
dotnet build
```

### 4. 配置 HRSDK
- 將 `HRSDK.dll` 複製到輸出目錄（`bin/Debug/net10.0-windows/` 或 `bin/Release/net10.0-windows/`）
- 或確保 HRSDK 的安裝路徑已加入系統 PATH

---

## 執行說明

### 💻 使用 HRSS（虛擬環境）的步驟

#### 第 1 步：啟動 HRSS
1. 開啟 HRSS 應用程式
2. 加載 RA605 機械手臂模型
3. **在 HRSS 中將操作模式設為 EXT**（重要！）
4. 記下 HRSS 中的控制器 IP（通常為 localhost 或 127.0.0.1，PORT 通常為 9090）

#### 第 2 步：啟動本應用
```bash
# 在 Visual Studio 中按 F5（Debug）或 Ctrl+F5（Release）
# 或直接執行輸出的 .exe 檔案
