# RA605-connect-test2

## 概覽
`RA605-connect-test2` 是一個簡易的 WinForms 用戶端，用來測試 HIWIN RA605 HRSDK (`HRobot.dll`)。它可以連線到控制器、顯示遙測日誌，並提供 LIN 運動與馬達控制功能，讓工程師不用進入示教器就能驗證通訊與軌跡邏輯。

## 主要功能
- **連線 / 中斷流程**：預設連線 IP 為 `192.168.203.200`，會顯示 SDK 版本並記錄每則 callback。
- **馬達控制**：`Motor ON` / `Motor OFF` 按鈕提供確認、狀態檢查與警報回報，讓伺服馬達可安全開關。
- **LIN 運動測試**：輸入 X,Y,Z,A,B,C 座標、速度、模式與 smooth 值即可執行 `lin_pos`，內含馬達狀態、操作模式、可達性與路徑檢查。
- **當前位置輔助**：一鍵將機械手臂的即時 TCP 座標與關節角度填入 LIN 輸入框，方便進行可預測的微調。
- **日誌視窗**：所有操作與 SDK 回應都會帶時間戳記地 Append 進滾動式日誌以協助除錯。

## 使用需求
- 目標為 .NET 10
- Windows 環境，需安裝 Hiwin HRSDK 且能存取 `HRSDK.dll`
- 手臂控制器可由設定的 IP 位址存取

## 建置步驟
1. 使用 Visual Studio 2022（或更新）並啟用 Windows Forms 工作負載。
2. 開啟 `RA605-connect-test2` 專案並進行建置。
3. 確保 `HRSDK.dll` 位於可執行檔旁或已加入 PATH。

## 執行說明
1. 啟動應用程式。
2. 點擊 `Connect` 連線至機器手臂。
3. 先按 `Motor ON` 開啟馬達，才可送出運動命令。
4. 若需要，可按 `Get Current Position` 將當前位置載入到 LIN 輸入欄位。
5. 調整 LIN 目標位置、速度、模式與 smooth 值後按 `Execute LIN`。
6. 觀察日誌視窗以取得診斷與完成通知。
7. 使用 `Motor OFF` 與 `Exit` 安全結束操作。

## 日誌與診斷
- 每則 SDK callback 與內部狀態變化都會在日誌中顯示時間戳記。
- LIN 命令失敗時會記錄可達性 / 路徑檢查、錯誤碼與警報代碼，以協助找出設定問題。

## 版本與修訂
| 項目 | 資訊 |
| --- | --- |
| 版本 | 1.0.0 |
| 最新修訂 | 2026-01-02 17:03 |

## 支援
若遇到問題，請參閱 HRSDK 手冊中 `lin_pos`、`set_motor_state` 與連線生命週期相關的函式，並檢查日誌中的 SDK 回應碼。也可以聯絡原始維護者 `https://github.com/chiangyih/RA605-connect-test`。
