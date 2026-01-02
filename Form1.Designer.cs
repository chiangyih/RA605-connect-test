namespace RA605_connect_test2
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtIP = new TextBox();
            btnConnect = new Button();
            btnDisconnect = new Button();
            txtLog = new TextBox();
            btnExit = new Button();
            btnMotorOff = new Button();
            btnMotorOn = new Button();
            grpLinTest = new GroupBox();
            btnLinTest = new Button();
            btnGetCurrentPos = new Button();
            txtSmooth = new TextBox();
            lblSmooth = new Label();
            cboLinMode = new ComboBox();
            lblMode = new Label();
            txtLinSpeed = new TextBox();
            lblLinSpeed = new Label();
            txtC = new TextBox();
            lblC = new Label();
            txtB = new TextBox();
            lblB = new Label();
            txtA = new TextBox();
            lblA = new Label();
            txtZ = new TextBox();
            lblZ = new Label();
            txtY = new TextBox();
            lblY = new Label();
            txtX = new TextBox();
            lblX = new Label();
            lblSeparator = new Label();
            lblIPAddress = new Label();
            grpConnection = new GroupBox();
            grpMotorControl = new GroupBox();
            grpLog = new GroupBox();
            grpLinTest.SuspendLayout();
            grpConnection.SuspendLayout();
            grpMotorControl.SuspendLayout();
            grpLog.SuspendLayout();
            SuspendLayout();
            // 
            // txtIP
            // 
            txtIP.Location = new Point(15, 45);
            txtIP.Name = "txtIP";
            txtIP.Size = new Size(270, 23);
            txtIP.TabIndex = 1;
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(15, 75);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(125, 28);
            btnConnect.TabIndex = 2;
            btnConnect.Text = "連接";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // btnDisconnect
            // 
            btnDisconnect.Enabled = false;
            btnDisconnect.Location = new Point(150, 75);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(135, 28);
            btnDisconnect.TabIndex = 3;
            btnDisconnect.Text = "斷開";
            btnDisconnect.UseVisualStyleBackColor = true;
            btnDisconnect.Click += btnDisconnect_Click;
            // 
            // txtLog
            // 
            txtLog.BackColor = Color.White;
            txtLog.Dock = DockStyle.Fill;
            txtLog.Font = new Font("Consolas", 9F);
            txtLog.Location = new Point(3, 19);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(564, 530);
            txtLog.TabIndex = 17;
            // 
            // btnExit
            // 
            btnExit.Location = new Point(12, 570);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(300, 35);
            btnExit.TabIndex = 18;
            btnExit.Text = "退出程式";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // btnMotorOff
            // 
            btnMotorOff.Location = new Point(155, 25);
            btnMotorOff.Name = "btnMotorOff";
            btnMotorOff.Size = new Size(130, 32);
            btnMotorOff.TabIndex = 5;
            btnMotorOff.Text = "關閉馬達 (OFF)";
            btnMotorOff.UseVisualStyleBackColor = true;
            btnMotorOff.Click += btnMotorOff_Click;
            // 
            // btnMotorOn
            // 
            btnMotorOn.Location = new Point(15, 25);
            btnMotorOn.Name = "btnMotorOn";
            btnMotorOn.Size = new Size(130, 32);
            btnMotorOn.TabIndex = 4;
            btnMotorOn.Text = "開啟馬達 (ON)";
            btnMotorOn.UseVisualStyleBackColor = true;
            btnMotorOn.Click += btnMotorOn_Click;
            // 
            // grpLinTest
            // 
            grpLinTest.Controls.Add(btnLinTest);
            grpLinTest.Controls.Add(btnGetCurrentPos);
            grpLinTest.Controls.Add(txtSmooth);
            grpLinTest.Controls.Add(lblSmooth);
            grpLinTest.Controls.Add(cboLinMode);
            grpLinTest.Controls.Add(lblMode);
            grpLinTest.Controls.Add(txtLinSpeed);
            grpLinTest.Controls.Add(lblLinSpeed);
            grpLinTest.Controls.Add(txtC);
            grpLinTest.Controls.Add(lblC);
            grpLinTest.Controls.Add(txtB);
            grpLinTest.Controls.Add(lblB);
            grpLinTest.Controls.Add(txtA);
            grpLinTest.Controls.Add(lblA);
            grpLinTest.Controls.Add(txtZ);
            grpLinTest.Controls.Add(lblZ);
            grpLinTest.Controls.Add(txtY);
            grpLinTest.Controls.Add(lblY);
            grpLinTest.Controls.Add(txtX);
            grpLinTest.Controls.Add(lblX);
            grpLinTest.Controls.Add(lblSeparator);
            grpLinTest.Location = new Point(12, 204);
            grpLinTest.Name = "grpLinTest";
            grpLinTest.Size = new Size(300, 360);
            grpLinTest.TabIndex = 2;
            grpLinTest.TabStop = false;
            grpLinTest.Text = "LIN 直線運動測試";
            // 
            // btnLinTest
            // 
            btnLinTest.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            btnLinTest.Location = new Point(15, 280);
            btnLinTest.Name = "btnLinTest";
            btnLinTest.Size = new Size(270, 40);
            btnLinTest.TabIndex = 16;
            btnLinTest.Text = "執行 LIN 運動";
            btnLinTest.UseVisualStyleBackColor = true;
            btnLinTest.Click += btnLinTest_Click;
            // 
            // btnGetCurrentPos
            // 
            btnGetCurrentPos.Location = new Point(15, 235);
            btnGetCurrentPos.Name = "btnGetCurrentPos";
            btnGetCurrentPos.Size = new Size(270, 35);
            btnGetCurrentPos.TabIndex = 15;
            btnGetCurrentPos.Text = "取得當前位置";
            btnGetCurrentPos.UseVisualStyleBackColor = true;
            btnGetCurrentPos.Click += btnGetCurrentPos_Click;
            // 
            // txtSmooth
            // 
            txtSmooth.Location = new Point(110, 192);
            txtSmooth.Name = "txtSmooth";
            txtSmooth.Size = new Size(80, 23);
            txtSmooth.TabIndex = 14;
            txtSmooth.Text = "0";
            // 
            // lblSmooth
            // 
            lblSmooth.AutoSize = true;
            lblSmooth.Location = new Point(15, 195);
            lblSmooth.Name = "lblSmooth";
            lblSmooth.Size = new Size(46, 15);
            lblSmooth.TabIndex = 0;
            lblSmooth.Text = "平滑值:";
            // 
            // cboLinMode
            // 
            cboLinMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLinMode.FormattingEnabled = true;
            cboLinMode.Items.AddRange(new object[] { "0", "1", "2" });
            cboLinMode.Location = new Point(110, 162);
            cboLinMode.Name = "cboLinMode";
            cboLinMode.Size = new Size(80, 23);
            cboLinMode.TabIndex = 13;
            // 
            // lblMode
            // 
            lblMode.AutoSize = true;
            lblMode.Location = new Point(15, 165);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(34, 15);
            lblMode.TabIndex = 0;
            lblMode.Text = "模式:";
            // 
            // txtLinSpeed
            // 
            txtLinSpeed.Location = new Point(110, 132);
            txtLinSpeed.Name = "txtLinSpeed";
            txtLinSpeed.Size = new Size(80, 23);
            txtLinSpeed.TabIndex = 12;
            txtLinSpeed.Text = "100";
            // 
            // lblLinSpeed
            // 
            lblLinSpeed.AutoSize = true;
            lblLinSpeed.Location = new Point(15, 135);
            lblLinSpeed.Name = "lblLinSpeed";
            lblLinSpeed.Size = new Size(77, 15);
            lblLinSpeed.TabIndex = 0;
            lblLinSpeed.Text = "速度 (mm/s):";
            // 
            // txtC
            // 
            txtC.Location = new Point(210, 87);
            txtC.Name = "txtC";
            txtC.Size = new Size(80, 23);
            txtC.TabIndex = 11;
            txtC.Text = "0";
            // 
            // lblC
            // 
            lblC.AutoSize = true;
            lblC.Location = new Point(160, 90);
            lblC.Name = "lblC";
            lblC.Size = new Size(34, 15);
            lblC.TabIndex = 0;
            lblC.Text = "C (°):";
            // 
            // txtB
            // 
            txtB.Location = new Point(70, 87);
            txtB.Name = "txtB";
            txtB.Size = new Size(80, 23);
            txtB.TabIndex = 10;
            txtB.Text = "0";
            // 
            // lblB
            // 
            lblB.AutoSize = true;
            lblB.Location = new Point(15, 90);
            lblB.Name = "lblB";
            lblB.Size = new Size(33, 15);
            lblB.TabIndex = 0;
            lblB.Text = "B (°):";
            // 
            // txtA
            // 
            txtA.Location = new Point(210, 57);
            txtA.Name = "txtA";
            txtA.Size = new Size(80, 23);
            txtA.TabIndex = 9;
            txtA.Text = "0";
            // 
            // lblA
            // 
            lblA.AutoSize = true;
            lblA.Location = new Point(160, 60);
            lblA.Name = "lblA";
            lblA.Size = new Size(34, 15);
            lblA.TabIndex = 0;
            lblA.Text = "A (°):";
            // 
            // txtZ
            // 
            txtZ.Location = new Point(70, 57);
            txtZ.Name = "txtZ";
            txtZ.Size = new Size(80, 23);
            txtZ.TabIndex = 8;
            txtZ.Text = "0";
            // 
            // lblZ
            // 
            lblZ.AutoSize = true;
            lblZ.Location = new Point(15, 60);
            lblZ.Name = "lblZ";
            lblZ.Size = new Size(50, 15);
            lblZ.TabIndex = 0;
            lblZ.Text = "Z (mm):";
            // 
            // txtY
            // 
            txtY.Location = new Point(210, 27);
            txtY.Name = "txtY";
            txtY.Size = new Size(80, 23);
            txtY.TabIndex = 7;
            txtY.Text = "0";
            // 
            // lblY
            // 
            lblY.AutoSize = true;
            lblY.Location = new Point(160, 30);
            lblY.Name = "lblY";
            lblY.Size = new Size(50, 15);
            lblY.TabIndex = 0;
            lblY.Text = "Y (mm):";
            // 
            // txtX
            // 
            txtX.Location = new Point(70, 27);
            txtX.Name = "txtX";
            txtX.Size = new Size(80, 23);
            txtX.TabIndex = 6;
            txtX.Text = "0";
            // 
            // lblX
            // 
            lblX.AutoSize = true;
            lblX.Location = new Point(15, 30);
            lblX.Name = "lblX";
            lblX.Size = new Size(51, 15);
            lblX.TabIndex = 0;
            lblX.Text = "X (mm):";
            // 
            // lblSeparator
            // 
            lblSeparator.BorderStyle = BorderStyle.Fixed3D;
            lblSeparator.Location = new Point(15, 120);
            lblSeparator.Name = "lblSeparator";
            lblSeparator.Size = new Size(270, 2);
            lblSeparator.TabIndex = 0;
            // 
            // lblIPAddress
            // 
            lblIPAddress.AutoSize = true;
            lblIPAddress.Location = new Point(15, 25);
            lblIPAddress.Name = "lblIPAddress";
            lblIPAddress.Size = new Size(71, 15);
            lblIPAddress.TabIndex = 0;
            lblIPAddress.Text = "機械手臂 IP:";
            // 
            // grpConnection
            // 
            grpConnection.Controls.Add(btnDisconnect);
            grpConnection.Controls.Add(btnConnect);
            grpConnection.Controls.Add(txtIP);
            grpConnection.Controls.Add(lblIPAddress);
            grpConnection.Location = new Point(12, 12);
            grpConnection.Name = "grpConnection";
            grpConnection.Size = new Size(300, 110);
            grpConnection.TabIndex = 0;
            grpConnection.TabStop = false;
            grpConnection.Text = "連接設定";
            // 
            // grpMotorControl
            // 
            grpMotorControl.Controls.Add(btnMotorOff);
            grpMotorControl.Controls.Add(btnMotorOn);
            grpMotorControl.Location = new Point(12, 128);
            grpMotorControl.Name = "grpMotorControl";
            grpMotorControl.Size = new Size(300, 70);
            grpMotorControl.TabIndex = 1;
            grpMotorControl.TabStop = false;
            grpMotorControl.Text = "馬達控制";
            // 
            // grpLog
            // 
            grpLog.Controls.Add(txtLog);
            grpLog.Location = new Point(318, 12);
            grpLog.Name = "grpLog";
            grpLog.Size = new Size(570, 552);
            grpLog.TabIndex = 3;
            grpLog.TabStop = false;
            grpLog.Text = "操作日誌";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 615);
            Controls.Add(grpLog);
            Controls.Add(grpLinTest);
            Controls.Add(grpMotorControl);
            Controls.Add(grpConnection);
            Controls.Add(btnExit);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "RA605-GB-710 機器手臂連線與LIN運動測試";
            grpLinTest.ResumeLayout(false);
            grpLinTest.PerformLayout();
            grpConnection.ResumeLayout(false);
            grpConnection.PerformLayout();
            grpMotorControl.ResumeLayout(false);
            grpLog.ResumeLayout(false);
            grpLog.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TextBox txtIP;
        private Button btnConnect;
        private Button btnDisconnect;
        private TextBox txtLog;
        private Button btnExit;
        private Button btnMotorOff;
        private Button btnMotorOn;
        private GroupBox grpLinTest;
        private Label lblX;
        private TextBox txtX;
        private Label lblY;
        private TextBox txtY;
        private Label lblZ;
        private TextBox txtZ;
        private Label lblA;
        private TextBox txtA;
        private Label lblB;
        private TextBox txtB;
        private Label lblC;
        private TextBox txtC;
        private Label lblLinSpeed;
        private TextBox txtLinSpeed;
        private Label lblMode;
        private ComboBox cboLinMode;
        private Label lblSmooth;
        private TextBox txtSmooth;
        private Button btnLinTest;
        private Button btnGetCurrentPos;
        
        // 新增的組件
        private Label lblIPAddress;
        private GroupBox grpConnection;
        private GroupBox grpMotorControl;
        private GroupBox grpLog;
        private Label lblSeparator;
    }
}
