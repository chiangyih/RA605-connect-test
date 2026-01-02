# RA605-710-GB 連線測試 & LIN 運動測試

## 概覽
`RA605-connect-test2` 一個 WinForms 用戶端應用程式，用來測試和控制 HIWIN RA605 機械手臂。透過 HRSDK（Hiwin Robot SDK）的 `HRobot.dll`，應用程式可以實現連線、馬達控制、位置查詢和 LIN 直線運動等功能。工程師無需進入示教器就能驗證通訊協議、運動軌跡和安全邏輯。

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

## 📊 代碼統計

| 項目 | 數值 |
|------|------|
| **Form1.cs** | 680+ 行 |
| **方法數量** | 45+ 個 |
| **常數定義** | 12 個 |
| **區域分組** | 7 個主區域 |
| **註解覆蓋率** | 95%+ |
| **總代碼行數** | 1000+ 行（含註解） |

---

## 🔄 工作流程圖

```
┌─────────────┐
│  啟動應用   │ → Program.cs (Main)
└─────┬───────┘
      │
      ↓
┌─────────────────────┐
│  初始化主表單       │ → Form1_Load (自動調用)
│  InitializeDefaults │
└─────┬───────────────┘
      │
      ↓
┌──────────────────────┐
│  用戶操作 UI 元件    │ → Form1.Designer.cs (UI 定義)
│  Click 事件觸發      │
└─────┬────────────────┘
      │
      ↓
┌──────────────────────────────┐
│  Form1.cs 事件處理器執行     │
│  btnConnect_Click            │
│  btnMotorOn_Click            │
│  btnLinTest_Click 等         │
└─────┬────────────────────────┘
      │
      ↓
┌──────────────────────────────┐
│  呼叫 SDK 函式 (P/Invoke)    │
│  HRobot.open_connection()    │ → sdk/HRobot.cs (P/Invoke)
│  HRobot.set_motor_state()    │
│  HRobot.lin_pos() 等         │ → HRSDK.dll (本機代碼)
└─────┬────────────────────────┘
      │
      ↓
┌──────────────────────────────┐
│  取得返回值和回呼訊息       │
│  更新 UI（日誌、狀態）      │
│  AppendLog() 或              │ → Form1.resx (資源)
│  MessageBox.Show()           │
└──────────────────────────────┘
```

---

## 🔗 檔案相互依賴

```
Form1.cs
├── 引用 → HRobot.cs (SDK P/Invoke 聲明)
├── 使用 → Form1.Designer.cs (UI 元件)
├── 使用 → Form1.resx (資源)
└── 呼叫 → HRSDK.dll (外部本機 DLL)

Program.cs
└── 啟動 → Form1() (主表單)

HRobot.cs
├── 聲明 → HRSDK.dll 函式
└── 提供給 → Form1.cs 使用

Form1.Designer.cs（自動生成）
├── 初始化 → Form1 的 UI 元件
└── 由 → Visual Studio 設計器管理

.csproj（專案配置）
├── 定義 → 目標框架（.NET 10）
├── 啟用 → WinForms 支援
└── 配置 → 編譯和輸出設置
```

---

## 📦 依賴項目

### 內部依賴
```
RA605-connect-test2
├── System（.NET 標準庫）
├── System.Windows.Forms（WinForms）
├── System.Globalization（區域設定）
├── System.Text（字符串処理）
└── System.Threading（多執行緒）
```

### 外部依賴
```
HRSDK.dll
├── Hiwin Robot SDK 本機庫
├── 必須位於 bin 目錄或 PATH 中
└── 提供機械手臂控制接口
```

---

## 🔧 編譯流程

```
源代碼 (.cs)
    ↓
[Visual Studio / dotnet build]
    ↓
中間語言 (MSIL) - obj/
    ↓
[JIT 編譯]
    ↓
機械碼 (.exe 和 .dll)
    ↓
輸出 (bin/Debug/net10.0-windows/ 或 bin/Release/)
    ↓
執行 (RA605-connect-test2.exe)
```

---

## 💾 檔案大小參考

| 檔案 | 大小 | 說明 |
|------|------|------|
| Form1.cs | ~25 KB | 主邏輯代碼 |
| Form1.Designer.cs | ~15 KB | UI 生成代碼 |
| Form1.resx | ~5 KB | 資源定義 |
| Program.cs | ~1 KB | 進入點 |
| HRobot.cs | ~8 KB | SDK 包裝類 |
| .csproj | ~2 KB | 專案配置 |
| **編譯輸出** | ~5-10 MB | exe 和依賴 DLL |

---

## 📈 項目成長時間線

| 版本 | 日期 | 主要改進 |
|------|------|--------|
| 1.0.0 | 2024-12-15 | 初始版本（基礎功能） |
| 1.1.0 | 2026-01-02 | 代碼優化、中文註解、完整文檔 |

---

## 🚀 快速開始對應檔案

```
開始開發？
    ↓
1️⃣ 檢查 Program.cs
   └─ 確保進入點正確

2️⃣ 查看 Form1.cs
   └─ 主要業務邏輯在這裡

3️⃣ 修改 UI？
   └─ 編輯 Form1.Designer.cs（透過設計器）

4️⃣ 添加 SDK 函式？
   └─ 編輯 sdk/HRobot.cs（P/Invoke 聲明）

5️⃣ 配置編譯？
   └─ 編輯 .csproj（專案設置）

6️⃣ 更新文檔？
   └─ 編輯 README.md（本檔案）
```

---

## 📌 最佳實踐

### ✅ 應該做的事
1. 編輯 `Form1.cs` 中的業務邏輯
2. 透過 Visual Studio 設計器修改 UI
3. 在 `HRobot.cs` 中添加新的 SDK 函式聲明
4. 保持常數的統一定義
5. 添加詳細的中文註解
6. 執行單元測試（非關鍵部分）

### ❌ 不應該做的事
1. ❌ 手動編輯 `Form1.Designer.cs`（由設計器管理）
2. ❌ 在代碼中使用魔數（應提取為常數）
3. ❌ 忽視執行緒安全性問題
4. ❌ 不處理 SDK 返回的錯誤碼
5. ❌ 移除或修改 SDK DLL 的路徑

---

## 🔍 檔案導航速查表

| 我想要... | 應該編輯... | 位置 |
|---------|-----------|------|
| 修改按鈕文字 | Form1.Designer.cs | 透過設計器 |
| 添加新按鈕事件 | Form1.cs | Connection/Motor/Position/LIN Motion |
| 修改 UI 佈局 | Form1 | Visual Studio 設計器 |
| 添加常數 | Form1.cs | Constants 區域 |
| 調用新 SDK 函式 | HRobot.cs | 添加 [DllImport] |
| 修改預設值 | Form1.cs | InitializeDefaults() |
| 改進日誌 | Form1.cs | AppendLog() 或 AppendLogThreadSafe() |
| 配置編譯選項 | .csproj | PropertyGroup |
| 更新文檔 | README.md | 本檔案 |

---

## ℹ️ 更多信息

- **原始倉庫**：https://github.com/chiangyih/RA605-connect-test
- **HRSDK 手冊**：參閱 Hiwin 官方文檔
- **.NET 10 文檔**：https://learn.microsoft.com/zh-tw/dotnet/
- **WinForms 指南**：https://learn.microsoft.com/zh-tw/dotnet/desktop/winforms/

---

**更新日期** - 2026-01-02  
**版本** - 1.1.0  
**維護者** - 新化高工 資訊科 曾鏹毅
