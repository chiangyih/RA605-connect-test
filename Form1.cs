using System;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SDKHrobot;

namespace RA605_connect_test2
{
    public partial class Form1 : Form
    {
        private int _robotId = -999;
        private volatile bool _connected = false;

        public Form1()
        {
            InitializeComponent();

            // 預設 IP
            txtIP.Text = "192.168.203.200";

            btnDisconnect.Enabled = false;
            
            // 預設 LIN Mode
            cboLinMode.SelectedIndex = 0;
            
            // 預設手臂初始位置 (Home Position)
            txtX.Text = "0";
            txtY.Text = "368";
            txtZ.Text = "293.5";
            txtA.Text = "180";
            txtB.Text = "0";
            txtC.Text = "90";
        }

        // 依手冊 callback_function:
        // void __stdcall CallBackFun(uint16_t cmd, uint16_t rlt, uint16_t* msg, int len)
        private void CallBackFun(ushort cmd, ushort rlt, ref ushort msg, int len)
        {
            // Callback 可能來自非 UI 執行緒，必須用 BeginInvoke 回到 UI thread
            string line = $"[Callback] cmd=0x{cmd:X4}, rlt=0x{rlt:X4}, len={len}";
            AppendLogThreadSafe(line);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (_connected) return;

            string ip = txtIP.Text.Trim();
            if (string.IsNullOrWhiteSpace(ip))
            {
                MessageBox.Show("請輸入手臂 IP");
                return;
            }

            // 顯示 SDK 版本（手冊範例 StringBuilder）
            var sdkVer = new StringBuilder(256);
            HRobot.get_hrsdk_version(sdkVer);
            AppendLog($"HRSDK Version: {sdkVer}");

            AppendLog($"Connecting to {ip} ...");

            // level：0=操作者，1=專家（手冊定義）
            const int level = 1;

            // open_connection 成功回傳 0~3；失敗回傳 -1~-4（手冊定義）
            _robotId = HRobot.open_connection(ip, level, CallBackFun);

            if (_robotId >= 0)
            {
                _connected = true;
                AppendLog($"Connect OK. robotId={_robotId}");

                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
                txtIP.Enabled = false;
            }
            else
            {
                AppendLog($"Connect FAIL. code={_robotId}");
                AppendLog("Error code hints:");
                AppendLog("  -1: command/connection failed");
                AppendLog("  -2: callback mechanism create failed");
                AppendLog("  -3: cannot reach robot");
                AppendLog("  -4: HRSDK and HRSS version mismatch");
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (!_connected) return;

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
                _connected = false;
                _robotId = -999;

                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
                txtIP.Enabled = true;

                AppendLog("Disconnected.");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            // 確認是否要結束
            var result = MessageBox.Show(
                "確定要結束程式嗎？", 
                "確認", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // 如果已連線，先斷線
                if (_connected)
                {
                    AppendLog("Disconnecting before exit...");
                    try
                    {
                        HRobot.disconnect(_robotId);
                        _connected = false;
                        _robotId = -999;
                        AppendLog("Disconnected.");
                    }
                    catch (Exception ex)
                    {
                        AppendLog($"Disconnect exception: {ex.Message}");
                    }
                }

                // 關閉應用程式
                Application.Exit();
            }
        }

        private void btnLinTest_Click(object sender, EventArgs e)
        {
            if (!_connected || _robotId < 0)
            {
                MessageBox.Show("尚未連線到手臂。請先 Connect。");
                return;
            }

            // 1) 解析輸入座標（X,Y,Z,A,B,C）
            if (!TryReadDouble(txtX, out double x) ||
                !TryReadDouble(txtY, out double y) ||
                !TryReadDouble(txtZ, out double z) ||
                !TryReadDouble(txtA, out double a) ||
                !TryReadDouble(txtB, out double b) ||
                !TryReadDouble(txtC, out double c))
            {
                MessageBox.Show("座標輸入格式錯誤，請確認 X,Y,Z,A,B,C 都是數字。");
                return;
            }

            // 2) 解析速度（mm/s）
            if (!TryReadDouble(txtLinSpeed, out double linSpeed) || linSpeed <= 0)
            {
                MessageBox.Show("LIN 速度輸入錯誤（mm/s），請輸入 > 0。");
                return;
            }

            // 3) 解析 mode 與 smooth_value
            int mode = 0;
            double smoothValue = 0;

            if (cboLinMode.SelectedItem != null)
                int.TryParse(cboLinMode.SelectedItem.ToString(), out mode);

            _ = TryReadDouble(txtSmooth, out smoothValue); // 失敗就維持 0

            // 4) 組合目標點 p[6] = {X,Y,Z,A,B,C}
            double[] p = new double[] { x, y, z, a, b, c };

            AppendLog("=== LIN Motion Check ===");

            // **重要前置檢查**
            // A. 檢查馬達狀態
            int motorState = HRobot.get_motor_state(_robotId);
            AppendLog($"Motor state: {motorState} (1=ON, 0=OFF)");
            if (motorState != 1)
            {
                MessageBox.Show("馬達未開啟！請先在示教器上開啟馬達 (Motor ON)。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // B. 檢查操作模式 (必須是 AUTO 模式才能執行外部命令)
            int opMode = HRobot.get_operation_mode(_robotId);
            AppendLog($"Operation mode: {opMode} (0=Manual, 1=Auto)");
            if (opMode != 1)
            {
                var result = MessageBox.Show(
                    "手臂不在 AUTO 模式！\n需要切換到 AUTO 模式才能執行 LIN 命令。\n\n是否嘗試切換到 AUTO 模式？",
                    "模式錯誤",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    int rc = HRobot.set_operation_mode(_robotId, 1); // 1 = Auto
                    AppendLog($"set_operation_mode(AUTO) rc={rc}");
                    if (rc != 0)
                    {
                        MessageBox.Show("切換到 AUTO 模式失敗！請手動在示教器上切換。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    Thread.Sleep(500); // 等待模式切換
                    AppendLog("Switched to AUTO mode successfully.");
                }
                else
                {
                    return;
                }
            }

            // C. 檢查運動狀態
            int motionState = HRobot.get_motion_state(_robotId);
            AppendLog($"Motion state: {motionState} (1=Idle, 2=Running, 3=Hold, 4=Delay, 5=Wait)");

            // D. 檢查目標點是否可達
            bool isReachable = false;
            int reachCheck = HRobot.motion_reachable(_robotId, p, ref isReachable);
            AppendLog($"Target reachability check: rc={reachCheck}, reachable={isReachable}");
            
            if (reachCheck == 0 && !isReachable)
            {
                MessageBox.Show(
                    "目標點不可達！\n可能原因：\n1. 超出手臂工作範圍\n2. 會產生奇異點\n3. 違反關節限制\n\n請檢查座標值是否正確。",
                    "路徑錯誤",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // E. 取得當前位置
            double[] currentPos = new double[6];
            HRobot.get_current_position(_robotId, currentPos);
            AppendLog($"Current position: [{currentPos[0]:F2}, {currentPos[1]:F2}, {currentPos[2]:F2}, {currentPos[3]:F2}, {currentPos[4]:F2}, {currentPos[5]:F2}]");
            AppendLog($"Target position: [{x:F2}, {y:F2}, {z:F2}, {a:F2}, {b:F2}, {c:F2}]");

            // F. 檢查路徑（從當前點到目標點）
            bool linPathOk = false;
            int linCheck = HRobot.motion_check_lin(_robotId, currentPos, p, ref linPathOk);
            AppendLog($"LIN path check: rc={linCheck}, path_ok={linPathOk}");
            
            if (linCheck == 0 && !linPathOk)
            {
                MessageBox.Show(
                    "直線路徑檢查失敗！\n路徑中可能存在：\n1. 奇異點\n2. 關節限制違反\n3. 軟體限制違反\n\n建議分段移動或調整目標點。",
                    "路徑錯誤",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // 5) 送出 LIN（依文件：先 set_lin_speed，再 lin_pos）
            AppendLog("=== Executing LIN Command ===");
            int rc1 = HRobot.set_lin_speed(_robotId, linSpeed);
            AppendLog($"set_lin_speed({linSpeed} mm/s) rc={rc1}");
            
            if (rc1 != 0)
            {
                MessageBox.Show($"設定 LIN 速度失敗 (rc={rc1})", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // lin_pos：絕對座標位置直線運動
            int rc2 = HRobot.lin_pos(_robotId, mode, smoothValue, p);
            AppendLog($"lin_pos(mode={mode}, smooth={smoothValue}) rc={rc2}");

            if (rc2 != 0)
            {
                MessageBox.Show($"LIN 命令執行失敗 (rc={rc2})\n請檢查 Log 視窗查看詳細資訊。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                // 檢查是否有警報
                int alarmCount = 0;
                ulong[] alarmCodes = new ulong[10];
                HRobot.get_alarm_code(_robotId, ref alarmCount, alarmCodes);
                if (alarmCount > 0)
                {
                    AppendLog($"Active alarms: {alarmCount}");
                    for (int i = 0; i < alarmCount && i < 10; i++)
                    {
                        AppendLog($"  Alarm[{i}]: 0x{alarmCodes[i]:X}");
                    }
                }
                return;
            }

            AppendLog("LIN command sent successfully.");

            // 6) 等待佇列清空，等效示教器「到點後才下一行」
            try
            {
                int count = 0;
                while ((count = HRobot.get_command_count(_robotId)) > 0)
                {
                    if (count > 0)
                        AppendLog($"Command queue count: {count}");
                    Thread.Sleep(100);
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

        private void btnMotorOff_Click(object sender, EventArgs e)
        {
            if (!_connected || _robotId < 0)
            {
                MessageBox.Show("尚未連線到手臂。請先 Connect。");
                return;
            }

            // 確認是否要關閉馬達
            var result = MessageBox.Show(
                "確定要關閉手臂馬達嗎？\n這將會停止所有運動並關閉伺服馬達。",
                "確認關閉馬達",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                AppendLog("Turning motor OFF...");
                
                // 先檢查當前馬達狀態
                int currentState = HRobot.get_motor_state(_robotId);
                AppendLog($"Current motor state: {currentState} (1=ON, 0=OFF)");

                // 設定馬達狀態為 OFF (0=OFF, 1=ON)
                int rc = HRobot.set_motor_state(_robotId, 0);
                
                if (rc == 0)
                {
                    AppendLog("Motor OFF command sent successfully (rc=0)");
                    
                    // 等待一下後再次檢查狀態
                    Thread.Sleep(500);
                    int newState = HRobot.get_motor_state(_robotId);
                    AppendLog($"New motor state: {newState} (1=ON, 0=OFF)");
                    
                    if (newState == 0)
                    {
                        MessageBox.Show("馬達已成功關閉。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("馬達關閉命令已送出，但狀態未確認。請檢查手臂。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    AppendLog($"Motor OFF command failed (rc={rc})");
                    MessageBox.Show($"關閉馬達失敗。錯誤碼: {rc}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                AppendLog("Motor OFF operation cancelled by user.");
            }
        }

        private void btnMotorOn_Click(object sender, EventArgs e)
        {
            if (!_connected || _robotId < 0)
            {
                MessageBox.Show("尚未連線到手臂。請先 Connect。");
                return;
            }

            AppendLog("Turning motor ON...");
            
            // 先檢查當前馬達狀態
            int currentState = HRobot.get_motor_state(_robotId);
            AppendLog($"Current motor state: {currentState} (1=ON, 0=OFF)");

            if (currentState == 1)
            {
                AppendLog("Motor is already ON.");
                MessageBox.Show("馬達已經是開啟狀態。", "資訊", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 設定馬達狀態為 ON (0=OFF, 1=ON)
            int rc = HRobot.set_motor_state(_robotId, 1);
            
            if (rc == 0)
            {
                AppendLog("Motor ON command sent successfully (rc=0)");
                
                // 等待一下後再次檢查狀態
                Thread.Sleep(500);
                int newState = HRobot.get_motor_state(_robotId);
                AppendLog($"New motor state: {newState} (1=ON, 0=OFF)");
                
                if (newState == 1)
                {
                    AppendLog("Motor ON confirmed.");
                    MessageBox.Show("馬達已成功開啟。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    AppendLog("Motor ON command sent but state not confirmed.");
                    MessageBox.Show(
                        "馬達開啟命令已送出，但狀態未確認。\n\n可能原因：\n1. 手臂有警報需要先清除\n2. 手臂在錯誤狀態\n3. 安全門未關閉\n\n請檢查手臂示教器。", 
                        "警告", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Warning);
                    
                    // 檢查是否有警報
                    int alarmCount = 0;
                    ulong[] alarmCodes = new ulong[10];
                    HRobot.get_alarm_code(_robotId, ref alarmCount, alarmCodes);
                    if (alarmCount > 0)
                    {
                        AppendLog($"Active alarms detected: {alarmCount}");
                        for (int i = 0; i < alarmCount && i < 10; i++)
                        {
                            AppendLog($"  Alarm[{i}]: 0x{alarmCodes[i]:X}");
                        }
                        MessageBox.Show(
                            $"偵測到 {alarmCount} 個警報！\n請先在示教器上清除警報後再開啟馬達。\n詳細資訊請查看 Log 視窗。",
                            "警報",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                AppendLog($"Motor ON command failed (rc={rc})");
                MessageBox.Show(
                    $"開啟馬達失敗。錯誤碼: {rc}\n\n可能原因：\n1. 手臂有警報需要先清除\n2. 手臂在錯誤狀態\n3. 操作模式不正確\n\n請檢查手臂示教器。", 
                    "錯誤", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
                
                // 檢查是否有警報
                int alarmCount = 0;
                ulong[] alarmCodes = new ulong[10];
                HRobot.get_alarm_code(_robotId, ref alarmCount, alarmCodes);
                if (alarmCount > 0)
                {
                    AppendLog($"Active alarms detected: {alarmCount}");
                    for (int i = 0; i < alarmCount && i < 10; i++)
                    {
                        AppendLog($"  Alarm[{i}]: 0x{alarmCodes[i]:X}");
                    }
                }
            }
        }

        private void btnGetCurrentPos_Click(object sender, EventArgs e)
        {
            if (!_connected || _robotId < 0)
            {
                MessageBox.Show("尚未連線到手臂。請先 Connect。");
                return;
            }

            AppendLog("=== Getting Current Position ===");

            try
            {
                // 取得當前座標位置 (X, Y, Z, A, B, C)
                double[] currentPos = new double[6];
                int rc = HRobot.get_current_position(_robotId, currentPos);
                
                if (rc == 0)
                {
                    // 填入到輸入框中
                    txtX.Text = currentPos[0].ToString("F2", CultureInfo.InvariantCulture);
                    txtY.Text = currentPos[1].ToString("F2", CultureInfo.InvariantCulture);
                    txtZ.Text = currentPos[2].ToString("F2", CultureInfo.InvariantCulture);
                    txtA.Text = currentPos[3].ToString("F2", CultureInfo.InvariantCulture);
                    txtB.Text = currentPos[4].ToString("F2", CultureInfo.InvariantCulture);
                    txtC.Text = currentPos[5].ToString("F2", CultureInfo.InvariantCulture);

                    AppendLog($"Current position retrieved successfully:");
                    AppendLog($"  X={currentPos[0]:F2}, Y={currentPos[1]:F2}, Z={currentPos[2]:F2}");
                    AppendLog($"  A={currentPos[3]:F2}, B={currentPos[4]:F2}, C={currentPos[5]:F2}");

                    // 同時取得關節角度資訊
                    double[] joints = new double[6];
                    int rcJoint = HRobot.get_current_joint(_robotId, joints);
                    if (rcJoint == 0)
                    {
                        AppendLog($"Current joint angles:");
                        AppendLog($"  J1={joints[0]:F2}°, J2={joints[1]:F2}°, J3={joints[2]:F2}°");
                        AppendLog($"  J4={joints[3]:F2}°, J5={joints[4]:F2}°, J6={joints[5]:F2}°");
                    }

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
                else
                {
                    AppendLog($"Failed to get current position (rc={rc})");
                    MessageBox.Show($"取得當前位置失敗 (rc={rc})", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                AppendLog($"Exception getting current position: {ex.Message}");
                MessageBox.Show($"取得當前位置時發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool TryReadDouble(TextBox tb, out double value)
        {
            // 使用 InvariantCulture 避免逗號/小數點地區差異
            return double.TryParse(tb.Text.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out value);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // 視窗關閉時自動斷線，避免控制器端殘留連線狀態
            if (_connected)
            {
                try { HRobot.disconnect(_robotId); } catch { /* ignore */ }
            }
            base.OnFormClosing(e);
        }

        private void AppendLog(string msg)
        {
            txtLog.AppendText($"{DateTime.Now:HH:mm:ss.fff} {msg}{Environment.NewLine}");
        }

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
    }
}
