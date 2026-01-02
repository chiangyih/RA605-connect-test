using System;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SDKHrobot;

namespace RA605_connect_test2
{
    /// <summary>
    /// RA605 機械手臂控制應用程式主表單
    /// 提供連接、馬達控制、位置查詢和 LIN 直線運動測試功能
    /// </summary>
    public partial class Form1 : Form
    {
        #region Constants
        // ===== 機械手臂狀態常數 =====
        /// <summary>無效的機械手臂 ID</summary>
        private const int INVALID_ROBOT_ID = -999;
        /// <summary>操作成功返回碼</summary>
        private const int SUCCESS_CODE = 0;
        /// <summary>連接等級：專家模式（可執行外部命令）</summary>
        private const int EXPERT_LEVEL = 1;
        
        // ===== 馬達狀態常數 =====
        /// <summary>馬達開啟狀態</summary>
        private const int MOTOR_ON = 1;
        /// <summary>馬達關閉狀態</summary>
        private const int MOTOR_OFF = 0;
        
        // ===== 操作模式常數 =====
        /// <summary>自動模式（可執行外部命令）</summary>
        private const int AUTO_MODE = 1;
        
        // ===== 延遲時間常數 (毫秒) =====
        /// <summary>狀態檢查延遲時間（等待狀態變更確認）</summary>
        private const int STATE_CHECK_DELAY_MS = 500;
        /// <summary>命令佇列檢查延遲時間（等待運動完成）</summary>
        private const int COMMAND_QUEUE_CHECK_DELAY_MS = 100;
        
        // ===== 其他常數 =====
        /// <summary>最多可取得的警報碼數量</summary>
        private const int MAX_ALARM_CODES = 10;
        /// <summary>預設機械手臂 IP 位址</summary>
        private const string DEFAULT_IP = "192.168.203.200";
        #endregion

        #region Fields
        // ===== 私有欄位 =====
        /// <summary>機械手臂 ID（成功連接後由 SDK 分配）</summary>
        private int _robotId = INVALID_ROBOT_ID;
        /// <summary>連接狀態標誌（使用 volatile 確保多執行緒安全）</summary>
        private volatile bool _connected = false;
        #endregion

        /// <summary>
        /// 表單初始化
        /// 呼叫自動生成的初始化代碼和設定預設值
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            InitializeDefaults();
        }

        /// <summary>
        /// 初始化預設值
        /// 設定預設 IP、禁用按鈕、設定預設位置
        /// </summary>
        private void InitializeDefaults()
        {
            txtIP.Text = DEFAULT_IP;
            btnDisconnect.Enabled = false;
            cboLinMode.SelectedIndex = 0;
            // 設定機械手臂初始位置 (Home Position)
            SetPositionFields(0, 368, 293.5, 180, 0, 90);
        }

        /// <summary>
        /// 設定位置輸入框的值
        /// </summary>
        /// <param name="x">X 座標（mm）</param>
        /// <param name="y">Y 座標（mm）</param>
        /// <param name="z">Z 座標（mm）</param>
        /// <param name="a">A 旋轉角（度）</param>
        /// <param name="b">B 旋轉角（度）</param>
        /// <param name="c">C 旋轉角（度）</param>
        private void SetPositionFields(double x, double y, double z, double a, double b, double c)
        {
            txtX.Text = x.ToString(CultureInfo.InvariantCulture);
            txtY.Text = y.ToString(CultureInfo.InvariantCulture);
            txtZ.Text = z.ToString(CultureInfo.InvariantCulture);
            txtA.Text = a.ToString(CultureInfo.InvariantCulture);
            txtB.Text = b.ToString(CultureInfo.InvariantCulture);
            txtC.Text = c.ToString(CultureInfo.InvariantCulture);
        }

        #region Connection Management（連接管理）
        /// <summary>
        /// 檢查是否已連接到機械手臂
        /// </summary>
        private bool IsConnected => _connected && _robotId >= 0;

        /// <summary>
        /// 確保已連接到機械手臂
        /// 若未連接則拋出異常
        /// </summary>
        /// <param name="operationName">操作名稱（用於錯誤訊息）</param>
        private void EnsureConnected(string operationName)
        {
            if (!IsConnected)
                throw new InvalidOperationException($"尚未連線到手臂。請先 Connect。({operationName})");
        }

        /// <summary>
        /// 機械手臂回呼函式
        /// 用於接收來自 SDK 的非同步訊息和事件
        /// 由於可能來自非 UI 執行緒，使用 BeginInvoke 確保執行緒安全
        /// </summary>
        /// <param name="cmd">命令代碼</param>
        /// <param name="rlt">結果代碼</param>
        /// <param name="msg">訊息指標</param>
        /// <param name="len">訊息長度</param>
        private void CallBackFun(ushort cmd, ushort rlt, ref ushort msg, int len)
        {
            string line = $"[Callback] cmd=0x{cmd:X4}, rlt=0x{rlt:X4}, len={len}";
            AppendLogThreadSafe(line);
        }

        /// <summary>
        /// 連接按鈕事件處理
        /// 連接到指定 IP 的機械手臂控制器
        /// </summary>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            // 已連接則退出
            if (_connected) return;

            // 驗證 IP 輸入
            string ip = txtIP.Text.Trim();
            if (string.IsNullOrWhiteSpace(ip))
            {
                MessageBox.Show("請輸入手臂 IP");
                return;
            }

            // 顯示 SDK 版本並開始連接
            DisplaySdkVersion();
            AppendLog($"Connecting to {ip} ...");

            // 以專家等級連接（允許外部命令執行）
            _robotId = HRobot.open_connection(ip, EXPERT_LEVEL, CallBackFun);

            // 檢查連接結果
            if (_robotId >= 0)
            {
                _connected = true;
                AppendLog($"Connect OK. robotId={_robotId}");
                SetConnectionUIState(connected: true);
            }
            else
            {
                AppendLog($"Connect FAIL. code={_robotId}");
                LogConnectionErrorHints(_robotId);
            }
        }

        /// <summary>
        /// 顯示 SDK 版本
        /// 查詢並記錄機械手臂 SDK 的版本資訊
        /// </summary>
        private void DisplaySdkVersion()
        {
            var sdkVer = new StringBuilder(256);
            HRobot.get_hrsdk_version(sdkVer);
            AppendLog($"HRSDK Version: {sdkVer}");
        }

        /// <summary>
        /// 記錄連接錯誤提示
        /// 解釋各個連接失敗的錯誤代碼含義
        /// </summary>
        /// <param name="errorCode">錯誤代碼</param>
        private void LogConnectionErrorHints(int errorCode)
        {
            AppendLog("Error code hints:");
            AppendLog("  -1: command/connection failed");
            AppendLog("  -2: callback mechanism create failed");
            AppendLog("  -3: cannot reach robot");
            AppendLog("  -4: HRSDK and HRSS version mismatch");
        }

        /// <summary>
        /// 設定連接相關 UI 元件的狀態
        /// 根據連接狀態啟用或禁用按鈕和文字框
        /// </summary>
        /// <param name="connected">連接狀態</param>
        private void SetConnectionUIState(bool connected)
        {
            btnConnect.Enabled = !connected;
            btnDisconnect.Enabled = connected;
            txtIP.Enabled = !connected;
        }

        /// <summary>
        /// 斷開按鈕事件處理
        /// 斷開與機械手臂的連接
        /// </summary>
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (!_connected) return;
            Disconnect();
        }

        /// <summary>
        /// 斷開連接
        /// 安全地關閉與機械手臂的連接並重設狀態
        /// 使用 try-finally 確保即使發生異常也能重設狀態
        /// </summary>
        private void Disconnect()
        {
            AppendLog("Disconnecting ...");
            try
            {
                HRobot.disconnect(_robotId);
            }
            catch (Exception ex)
            {
                AppendLog($"Disconnect exception: {ex.Message}");
            }
            finally
            {
                // 無論成功或失敗都重設狀態
                _connected = false;
                _robotId = INVALID_ROBOT_ID;
                SetConnectionUIState(connected: false);
                AppendLog("Disconnected.");
            }
        }

        /// <summary>
        /// 結束按鈕事件處理
        /// 提示用戶確認後關閉應用程式
        /// 若已連接則先斷開連接
        /// </summary>
        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "確定要結束程式嗎？",
                "確認",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // 如果已連接則先斷開
                if (_connected)
                    Disconnect();

                Application.Exit();
            }
        }
        #endregion

        #region Motor Control（馬達控制）
        /// <summary>
        /// 開啟馬達按鈕事件處理
        /// 開啟機械手臂伺服馬達
        /// </summary>
        private void btnMotorOn_Click(object sender, EventArgs e)
        {
            try
            {
                EnsureConnected("Motor ON");
                SetMotorState(MOTOR_ON);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 關閉馬達按鈕事件處理
        /// 經由用戶確認後關閉機械手臂伺服馬達
        /// </summary>
        private void btnMotorOff_Click(object sender, EventArgs e)
        {
            try
            {
                EnsureConnected("Motor OFF");

                // 詢問用戶確認
                if (MessageBox.Show(
                    "確定要關閉手臂馬達嗎？\n這將會停止所有運動並關閉伺服馬達。",
                    "確認關閉馬達",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    SetMotorState(MOTOR_OFF);
                }
                else
                {
                    AppendLog("Motor OFF operation cancelled by user.");
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 設定馬達狀態
        /// 根據參數開啟或關閉馬達，並驗證狀態變更
        /// 分別處理成功和失敗的情況
        /// </summary>
        /// <param name="state">目標狀態（MOTOR_ON 或 MOTOR_OFF）</param>
        private void SetMotorState(int state)
        {
            string stateLabel = state == MOTOR_ON ? "ON" : "OFF";
            AppendLog($"Turning motor {stateLabel}...");

            // 檢查當前狀態
            int currentState = HRobot.get_motor_state(_robotId);
            AppendLog($"Current motor state: {currentState} (1=ON, 0=OFF)");

            // 如果已經是目標狀態則提示並退出
            if (state == MOTOR_ON && currentState == MOTOR_ON)
            {
                AppendLog("Motor is already ON.");
                MessageBox.Show("馬達已經是開啟狀態。", "資訊", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 發送命令
            int rc = HRobot.set_motor_state(_robotId, state);
            AppendLog($"Motor {stateLabel} command: rc={rc}");

            // 根據結果處理
            if (rc == SUCCESS_CODE)
            {
                // 等待並驗證狀態變更
                Thread.Sleep(STATE_CHECK_DELAY_MS);
                int newState = HRobot.get_motor_state(_robotId);
                AppendLog($"New motor state: {newState} (1=ON, 0=OFF)");

                if (newState == state)
                {
                    // 狀態變更成功
                    string successMsg = state == MOTOR_ON ? "馬達已成功開啟。" : "馬達已成功關閉。";
                    AppendLog($"Motor {stateLabel} confirmed.");
                    MessageBox.Show(successMsg, "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // 命令已送但狀態未變更
                    HandleMotorStateNotConfirmed(state);
                }
            }
            else
            {
                // 命令執行失敗
                HandleMotorCommandFailed(state, rc);
            }
        }

        /// <summary>
        /// 處理馬達狀態未確認情況
        /// 馬達命令已送出但狀態未變更（可能有警報或其他問題）
        /// </summary>
        /// <param name="state">目標狀態</param>
        private void HandleMotorStateNotConfirmed(int state)
        {
            string stateLabel = state == MOTOR_ON ? "開啟" : "關閉";
            AppendLog($"Motor {stateLabel} command sent but state not confirmed.");

            MessageBox.Show(
                $"馬達{stateLabel}命令已送出，但狀態未確認。\n\n可能原因：\n1. 手臂有警報需要先清除\n2. 手臂在錯誤狀態\n3. 安全門未關閉\n\n請檢查手臂示教器。",
                "警告",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            // 檢查並顯示任何警報
            TryShowAlarms();
        }

        /// <summary>
        /// 處理馬達命令失敗情況
        /// 馬達命令執行失敗（返回非零代碼）
        /// </summary>
        /// <param name="state">目標狀態</param>
        /// <param name="rc">返回碼</param>
        private void HandleMotorCommandFailed(int state, int rc)
        {
            string stateLabel = state == MOTOR_ON ? "開啟" : "關閉";
            AppendLog($"Motor {stateLabel} command failed (rc={rc})");

            MessageBox.Show(
                $"馬達{stateLabel}失敗。錯誤碼: {rc}\n\n可能原因：\n1. 手臂有警報需要先清除\n2. 手臂在錯誤狀態\n3. 操作模式不正確\n\n請檢查手臂示教器。",
                "錯誤",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            // 檢查並顯示任何警報
            TryShowAlarms();
        }

        /// <summary>
        /// 嘗試顯示警報代碼
        /// 查詢機械手臂目前的警報列表並顯示給用戶
        /// 如果沒有警報則不顯示任何訊息
        /// </summary>
        private void TryShowAlarms()
        {
            int alarmCount = 0;
            ulong[] alarmCodes = new ulong[MAX_ALARM_CODES];

            // 取得警報代碼
            HRobot.get_alarm_code(_robotId, ref alarmCount, alarmCodes);

            if (alarmCount > 0)
            {
                // 記錄警報訊息
                AppendLog($"Active alarms detected: {alarmCount}");
                for (int i = 0; i < alarmCount && i < MAX_ALARM_CODES; i++)
                {
                    AppendLog($"  Alarm[{i}]: 0x{alarmCodes[i]:X}");
                }

                // 顯示警報提示訊息
                MessageBox.Show(
                    $"偵測到 {alarmCount} 個警報！\n請先在示教器上清除警報。\n詳細資訊請查看 Log 視窗。",
                    "警報",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Position Management（位置管理）
        /// <summary>
        /// 取得當前位置按鈕事件處理
        /// 從機械手臂查詢當前座標和關節角度
        /// 將結果填入到輸入框供用戶使用
        /// </summary>
        private void btnGetCurrentPos_Click(object sender, EventArgs e)
        {
            try
            {
                EnsureConnected("Get Current Position");
                LoadCurrentPosition();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                AppendLog($"Exception getting current position: {ex.Message}");
                MessageBox.Show($"取得當前位置時發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 載入當前位置
        /// 從機械手臂取得當前座標和關節角度
        /// 並在 UI 和 Log 中顯示結果
        /// </summary>
        private void LoadCurrentPosition()
        {
            AppendLog("=== Getting Current Position ===");

            // 取得座標位置
            double[] currentPos = new double[6];
            int rc = HRobot.get_current_position(_robotId, currentPos);

            if (rc != SUCCESS_CODE)
            {
                AppendLog($"Failed to get current position (rc={rc})");
                MessageBox.Show($"取得當前位置失敗 (rc={rc})", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 更新 UI 和記錄日誌
            SetPositionTextBoxes(currentPos);
            LogCurrentPosition(currentPos);
            LogCurrentJoints();

            // 顯示位置訊息
            MessageBox.Show(
                $"當前位置已載入到輸入框！\n\n" +
                $"X = {currentPos[0]:F2} mm\n" +
                $"Y = {currentPos[1]:F2} mm\n" +
                $"Z = {currentPos[2]:F2} mm\n" +
                $"A = {currentPos[3]:F2}°\n" +
                $"B = {currentPos[4]:F2}°\n" +
                $"C = {currentPos[5]:F2}°\n\n" +
                "現在您可以微調這些值來測試 LIN 運動。",
                "當前位置",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// 設定位置文字框
        /// 將座標陣列中的值填入對應的文字框
        /// </summary>
        /// <param name="pos">座標陣列（X, Y, Z, A, B, C）</param>
        private void SetPositionTextBoxes(double[] pos)
        {
            txtX.Text = pos[0].ToString("F2", CultureInfo.InvariantCulture);
            txtY.Text = pos[1].ToString("F2", CultureInfo.InvariantCulture);
            txtZ.Text = pos[2].ToString("F2", CultureInfo.InvariantCulture);
            txtA.Text = pos[3].ToString("F2", CultureInfo.InvariantCulture);
            txtB.Text = pos[4].ToString("F2", CultureInfo.InvariantCulture);
            txtC.Text = pos[5].ToString("F2", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 記錄當前位置
        /// 將座標資訊寫入日誌視窗
        /// </summary>
        /// <param name="pos">座標陣列</param>
        private void LogCurrentPosition(double[] pos)
        {
            AppendLog($"Current position retrieved successfully:");
            AppendLog($"  X={pos[0]:F2}, Y={pos[1]:F2}, Z={pos[2]:F2}");
            AppendLog($"  A={pos[3]:F2}, B={pos[4]:F2}, C={pos[5]:F2}");
        }

        /// <summary>
        /// 記錄當前關節角度
        /// 從機械手臂取得並記錄各關節的角度資訊
        /// </summary>
        private void LogCurrentJoints()
        {
            double[] joints = new double[6];
            int rc = HRobot.get_current_joint(_robotId, joints);

            if (rc == SUCCESS_CODE)
            {
                AppendLog($"Current joint angles:");
                AppendLog($"  J1={joints[0]:F2}°, J2={joints[1]:F2}°, J3={joints[2]:F2}°");
                AppendLog($"  J4={joints[3]:F2}°, J5={joints[4]:F2}°, J6={joints[5]:F2}°");
            }
        }
        #endregion

        #region LIN Motion（直線運動）
        /// <summary>
        /// LIN 運動測試按鈕事件處理
        /// 驗證連接狀態後執行完整的直線運動流程
        /// 包括前置檢查、命令執行和完成等待
        /// </summary>
        private void btnLinTest_Click(object sender, EventArgs e)
        {
            try
            {
                EnsureConnected("LIN Motion");
                ExecuteLinMotion();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                AppendLog($"LIN motion error: {ex.Message}");
                MessageBox.Show($"LIN 運動執行失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 執行直線運動
        /// 完整的 LIN 運動流程：
        /// 1. 解析輸入參數
        /// 2. 執行前置檢查（馬達、模式、可達性、路徑）
        /// 3. 執行 LIN 命令
        /// 4. 等待運動完成
        /// </summary>
        private void ExecuteLinMotion()
        {
            // 1. 解析輸入
            if (!TryReadPositionAndSpeed(out double x, out double y, out double z,
                                        out double a, out double b, out double c,
                                        out double linSpeed))
                return;

            int mode = ParseLinMode();
            _ = TryReadDouble(txtSmooth, out double smoothValue);

            double[] targetPos = new double[] { x, y, z, a, b, c };

            // 2. 執行前置檢查
            AppendLog("=== LIN Motion Check ===");

            if (!CheckMotorState())
                return;

            if (!CheckAndSetOperationMode())
                return;

            int motionState = HRobot.get_motion_state(_robotId);
            AppendLog($"Motion state: {motionState} (1=Idle, 2=Running, 3=Hold, 4=Delay, 5=Wait)");

            if (!CheckTargetReachability(targetPos))
                return;

            if (!CheckLinPath(targetPos))
                return;

            // 3. 執行運動
            if (!ExecuteLinCommand(linSpeed, mode, smoothValue, targetPos))
                return;

            // 4. 等待完成
            WaitForMotionComplete();
        }

        /// <summary>
        /// 嘗試讀取位置和速度參數
        /// 驗證所有輸入值的有效性
        /// </summary>
        /// <param name="x">X 座標輸出</param>
        /// <param name="y">Y 座標輸出</param>
        /// <param name="z">Z 座標輸出</param>
        /// <param name="a">A 旋轉角輸出</param>
        /// <param name="b">B 旋轉角輸出</param>
        /// <param name="c">C 旋轉角輸出</param>
        /// <param name="linSpeed">直線速度輸出（mm/s）</param>
        /// <returns>所有值都有效時返回 true</returns>
        private bool TryReadPositionAndSpeed(out double x, out double y, out double z,
                                            out double a, out double b, out double c,
                                            out double linSpeed)
        {
            x = y = z = a = b = c = linSpeed = 0;

            // 驗證座標輸入
            if (!TryReadDouble(txtX, out x) ||
                !TryReadDouble(txtY, out y) ||
                !TryReadDouble(txtZ, out z) ||
                !TryReadDouble(txtA, out a) ||
                !TryReadDouble(txtB, out b) ||
                !TryReadDouble(txtC, out c))
            {
                MessageBox.Show("座標輸入格式錯誤，請確認 X,Y,Z,A,B,C 都是數字。");
                return false;
            }

            // 驗證速度輸入（必須 > 0）
            if (!TryReadDouble(txtLinSpeed, out linSpeed) || linSpeed <= 0)
            {
                MessageBox.Show("LIN 速度輸入錯誤（mm/s），請輸入 > 0。");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 解析 LIN 運動模式
        /// 從下拉選單中讀取模式選項
        /// </summary>
        /// <returns>模式值（預設為 0）</returns>
        private int ParseLinMode()
        {
            if (cboLinMode.SelectedItem == null)
                return 0;

            int.TryParse(cboLinMode.SelectedItem.ToString(), out int mode);
            return mode;
        }

        /// <summary>
        /// 檢查馬達狀態
        /// 確保馬達已開啟，否則無法執行運動命令
        /// </summary>
        /// <returns>馬達已開啟時返回 true</returns>
        private bool CheckMotorState()
        {
            int motorState = HRobot.get_motor_state(_robotId);
            AppendLog($"Motor state: {motorState} (1=ON, 0=OFF)");

            if (motorState != MOTOR_ON)
            {
                MessageBox.Show("馬達未開啟！請先在示教器上開啟馬達 (Motor ON)。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 檢查並設定操作模式
        /// 確保手臂在自動模式，若不是則嘗試切換
        /// 只有自動模式才能執行外部命令
        /// </summary>
        /// <returns>已在自動模式時返回 true</returns>
        private bool CheckAndSetOperationMode()
        {
            int opMode = HRobot.get_operation_mode(_robotId);
            AppendLog($"Operation mode: {opMode} (0=Manual, 1=Auto)");

            // 已經是自動模式
            if (opMode == AUTO_MODE)
                return true;

            // 詢問用戶是否切換到自動模式
            if (MessageBox.Show(
                "手臂不在 AUTO 模式！\n需要切換到 AUTO 模式才能執行 LIN 命令。\n\n是否嘗試切換到 AUTO 模式？",
                "模式錯誤",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) != DialogResult.Yes)
                return false;

            // 嘗試切換模式
            int rc = HRobot.set_operation_mode(_robotId, AUTO_MODE);
            AppendLog($"set_operation_mode(AUTO) rc={rc}");

            if (rc != SUCCESS_CODE)
            {
                MessageBox.Show("切換到 AUTO 模式失敗！請手動在示教器上切換。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // 等待模式切換完成
            Thread.Sleep(STATE_CHECK_DELAY_MS);
            AppendLog("Switched to AUTO mode successfully.");
            return true;
        }

        /// <summary>
        /// 檢查目標點是否可達
        /// 驗證目標座標是否在機械手臂的工作範圍內且不會產生奇異點
        /// </summary>
        /// <param name="targetPos">目標座標陣列</param>
        /// <returns>目標點可達時返回 true</returns>
        private bool CheckTargetReachability(double[] targetPos)
        {
            bool isReachable = false;
            int rc = HRobot.motion_reachable(_robotId, targetPos, ref isReachable);
            AppendLog($"Target reachability check: rc={rc}, reachable={isReachable}");

            if (rc == SUCCESS_CODE && !isReachable)
            {
                MessageBox.Show(
                    "目標點不可達！\n可能原因：\n1. 超出手臂工作範圍\n2. 會產生奇異點\n3. 違反關節限制\n\n請檢查座標值是否正確。",
                    "路徑錯誤",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 檢查直線路徑
        /// 驗證從當前位置到目標位置的直線路徑是否有障礙或奇異點
        /// </summary>
        /// <param name="targetPos">目標座標陣列</param>
        /// <returns>路徑暢通時返回 true</returns>
        private bool CheckLinPath(double[] targetPos)
        {
            // 取得當前位置
            double[] currentPos = new double[6];
            HRobot.get_current_position(_robotId, currentPos);
            AppendLog($"Current position: [{string.Join(", ", System.Array.ConvertAll(currentPos, x => x.ToString("F2")))}]");
            AppendLog($"Target position: [{string.Join(", ", System.Array.ConvertAll(targetPos, x => x.ToString("F2")))}]");

            // 檢查直線路徑
            bool linPathOk = false;
            int rc = HRobot.motion_check_lin(_robotId, currentPos, targetPos, ref linPathOk);
            AppendLog($"LIN path check: rc={rc}, path_ok={linPathOk}");

            if (rc == SUCCESS_CODE && !linPathOk)
            {
                MessageBox.Show(
                    "直線路徑檢查失敗！\n路徑中可能存在：\n1. 奇異點\n2. 關節限制違反\n3. 軟體限制違反\n\n建議分段移動或調整目標點。",
                    "路徑錯誤",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 執行直線運動命令
        /// 設定速度並發送 LIN 命令到機械手臂
        /// 處理命令執行的結果
        /// </summary>
        /// <param name="linSpeed">直線速度（mm/s）</param>
        /// <param name="mode">運動模式</param>
        /// <param name="smoothValue">平滑參數</param>
        /// <param name="targetPos">目標座標陣列</param>
        /// <returns>命令成功發送時返回 true</returns>
        private bool ExecuteLinCommand(double linSpeed, int mode, double smoothValue, double[] targetPos)
        {
            AppendLog("=== Executing LIN Command ===");

            // 設定直線速度
            int rc1 = HRobot.set_lin_speed(_robotId, linSpeed);
            AppendLog($"set_lin_speed({linSpeed} mm/s) rc={rc1}");

            if (rc1 != SUCCESS_CODE)
            {
                MessageBox.Show($"設定 LIN 速度失敗 (rc={rc1})", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // 執行直線運動（絕對座標位置）
            int rc2 = HRobot.lin_pos(_robotId, mode, smoothValue, targetPos);
            AppendLog($"lin_pos(mode={mode}, smooth={smoothValue}) rc={rc2}");

            if (rc2 != SUCCESS_CODE)
            {
                MessageBox.Show($"LIN 命令執行失敗 (rc={rc2})\n請檢查 Log 視窗查看詳細資訊。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TryShowAlarms();
                return false;
            }

            AppendLog("LIN command sent successfully.");
            return true;
        }

        /// <summary>
        /// 等待運動完成
        /// 監控命令佇列直到為空（表示所有運動已完成）
        /// 等效於示教器上「到點後才下一行」的效果
        /// </summary>
        private void WaitForMotionComplete()
        {
            try
            {
                int count;
                while ((count = HRobot.get_command_count(_robotId)) > 0)
                {
                    if (count > 0)
                        AppendLog($"Command queue count: {count}");
                    Thread.Sleep(COMMAND_QUEUE_CHECK_DELAY_MS);
                }

                AppendLog("LIN motion completed (command queue empty).");
                MessageBox.Show("LIN 運動完成！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AppendLog($"Queue monitoring error: {ex.Message}");
                AppendLog("LIN command sent (queue monitoring not available).");
            }
        }
        #endregion

        #region Helper Methods（輔助方法）
        /// <summary>
        /// 嘗試從文字框讀取雙精度浮點數
        /// 使用不變文化確保小數點格式一致（避免地區設定影響）
        /// </summary>
        /// <param name="tb">來源文字框</param>
        /// <param name="value">讀取的值</param>
        /// <returns>成功讀取時返回 true</returns>
        private bool TryReadDouble(TextBox tb, out double value)
        {
            return double.TryParse(tb.Text.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out value);
        }

        /// <summary>
        /// 在日誌視窗追加訊息
        /// 自動添加時間戳記和換行符號
        /// </summary>
        /// <param name="msg">要追加的訊息</param>
        private void AppendLog(string msg)
        {
            txtLog.AppendText($"{DateTime.Now:HH:mm:ss.fff} {msg}{Environment.NewLine}");
        }

        /// <summary>
        /// 在日誌視窗追加訊息（執行緒安全）
        /// 用於非 UI 執行緒的回呼函式
        /// 通過 BeginInvoke 確保在 UI 執行緒上執行
        /// </summary>
        /// <param name="msg">要追加的訊息</param>
        private void AppendLogThreadSafe(string msg)
        {
            if (IsHandleCreated && InvokeRequired)
            {
                BeginInvoke(new Action(() => AppendLog(msg)));
            }
            else
            {
                AppendLog(msg);
            }
        }

        /// <summary>
        /// 表單關閉事件處理
        /// 確保應用程式結束前已安全斷開與機械手臂的連接
        /// 避免控制器端殘留連線狀態
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_connected)
            {
                try { HRobot.disconnect(_robotId); }
                catch { /* ignore */ }
            }
            base.OnFormClosing(e);
        }
        #endregion
    }
}
