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
            lblX = new Label();
            txtX = new TextBox();
            lblY = new Label();
            txtY = new TextBox();
            lblZ = new Label();
            txtZ = new TextBox();
            lblA = new Label();
            txtA = new TextBox();
            lblB = new Label();
            txtB = new TextBox();
            lblC = new Label();
            txtC = new TextBox();
            lblLinSpeed = new Label();
            txtLinSpeed = new TextBox();
            lblMode = new Label();
            cboLinMode = new ComboBox();
            lblSmooth = new Label();
            txtSmooth = new TextBox();
            btnLinTest = new Button();
            btnGetCurrentPos = new Button();
            SuspendLayout();
            // 
            // txtIP
            // 
            txtIP.Location = new Point(12, 12);
            txtIP.Name = "txtIP";
            txtIP.Size = new Size(200, 23);
            txtIP.TabIndex = 0;
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(218, 11);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(100, 23);
            btnConnect.TabIndex = 1;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // btnDisconnect
            // 
            btnDisconnect.Location = new Point(324, 11);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(100, 23);
            btnDisconnect.TabIndex = 2;
            btnDisconnect.Text = "Disconnect";
            btnDisconnect.UseVisualStyleBackColor = true;
            btnDisconnect.Click += btnDisconnect_Click;
            // 
            // btnExit
            // 
            btnExit.Location = new Point(430, 11);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(100, 23);
            btnExit.TabIndex = 4;
            btnExit.Text = "Exit";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // btnMotorOff
            // 
            btnMotorOff.Location = new Point(536, 11);
            btnMotorOff.Name = "btnMotorOff";
            btnMotorOff.Size = new Size(100, 23);
            btnMotorOff.TabIndex = 16;
            btnMotorOff.Text = "Motor OFF";
            btnMotorOff.UseVisualStyleBackColor = true;
            btnMotorOff.Click += btnMotorOff_Click;
            // 
            // btnMotorOn
            // 
            btnMotorOn.Location = new Point(642, 11);
            btnMotorOn.Name = "btnMotorOn";
            btnMotorOn.Size = new Size(100, 23);
            btnMotorOn.TabIndex = 18;
            btnMotorOn.Text = "Motor ON";
            btnMotorOn.UseVisualStyleBackColor = true;
            btnMotorOn.Click += btnMotorOn_Click;
            // 
            // grpLinTest
            // 
            grpLinTest.Controls.Add(lblX);
            grpLinTest.Controls.Add(txtX);
            grpLinTest.Controls.Add(lblY);
            grpLinTest.Controls.Add(txtY);
            grpLinTest.Controls.Add(lblZ);
            grpLinTest.Controls.Add(txtZ);
            grpLinTest.Controls.Add(lblA);
            grpLinTest.Controls.Add(txtA);
            grpLinTest.Controls.Add(lblB);
            grpLinTest.Controls.Add(txtB);
            grpLinTest.Controls.Add(lblC);
            grpLinTest.Controls.Add(txtC);
            grpLinTest.Controls.Add(lblLinSpeed);
            grpLinTest.Controls.Add(txtLinSpeed);
            grpLinTest.Controls.Add(lblMode);
            grpLinTest.Controls.Add(cboLinMode);
            grpLinTest.Controls.Add(lblSmooth);
            grpLinTest.Controls.Add(txtSmooth);
            grpLinTest.Location = new Point(12, 41);
            grpLinTest.Name = "grpLinTest";
            grpLinTest.Size = new Size(300, 397);
            grpLinTest.TabIndex = 5;
            grpLinTest.TabStop = false;
            grpLinTest.Text = "LIN Test";
            // 
            // lblX
            // 
            lblX.AutoSize = true;
            lblX.Location = new Point(24, 66);
            lblX.Name = "lblX";
            lblX.Size = new Size(20, 15);
            lblX.TabIndex = 0;
            lblX.Text = "X:";
            // 
            // txtX
            // 
            txtX.Location = new Point(50, 63);
            txtX.Name = "txtX";
            txtX.Size = new Size(100, 23);
            txtX.TabIndex = 6;
            txtX.Text = "0";
            // 
            // lblY
            // 
            lblY.AutoSize = true;
            lblY.Location = new Point(160, 66);
            lblY.Name = "lblY";
            lblY.Size = new Size(20, 15);
            lblY.TabIndex = 0;
            lblY.Text = "Y:";
            // 
            // txtY
            // 
            txtY.Location = new Point(186, 63);
            txtY.Name = "txtY";
            txtY.Size = new Size(100, 23);
            txtY.TabIndex = 7;
            txtY.Text = "0";
            // 
            // lblZ
            // 
            lblZ.AutoSize = true;
            lblZ.Location = new Point(24, 95);
            lblZ.Name = "lblZ";
            lblZ.Size = new Size(20, 15);
            lblZ.TabIndex = 0;
            lblZ.Text = "Z:";
            // 
            // txtZ
            // 
            txtZ.Location = new Point(50, 92);
            txtZ.Name = "txtZ";
            txtZ.Size = new Size(100, 23);
            txtZ.TabIndex = 8;
            txtZ.Text = "0";
            // 
            // lblA
            // 
            lblA.AutoSize = true;
            lblA.Location = new Point(160, 95);
            lblA.Name = "lblA";
            lblA.Size = new Size(20, 15);
            lblA.TabIndex = 0;
            lblA.Text = "A:";
            // 
            // txtA
            // 
            txtA.Location = new Point(186, 92);
            txtA.Name = "txtA";
            txtA.Size = new Size(100, 23);
            txtA.TabIndex = 9;
            txtA.Text = "0";
            // 
            // lblB
            // 
            lblB.AutoSize = true;
            lblB.Location = new Point(24, 124);
            lblB.Name = "lblB";
            lblB.Size = new Size(20, 15);
            lblB.TabIndex = 0;
            lblB.Text = "B:";
            // 
            // txtB
            // 
            txtB.Location = new Point(50, 121);
            txtB.Name = "txtB";
            txtB.Size = new Size(100, 23);
            txtB.TabIndex = 10;
            txtB.Text = "0";
            // 
            // lblC
            // 
            lblC.AutoSize = true;
            lblC.Location = new Point(160, 124);
            lblC.Name = "lblC";
            lblC.Size = new Size(20, 15);
            lblC.TabIndex = 0;
            lblC.Text = "C:";
            // 
            // txtC
            // 
            txtC.Location = new Point(186, 121);
            txtC.Name = "txtC";
            txtC.Size = new Size(100, 23);
            txtC.TabIndex = 11;
            txtC.Text = "0";
            // 
            // lblLinSpeed
            // 
            lblLinSpeed.AutoSize = true;
            lblLinSpeed.Location = new Point(24, 163);
            lblLinSpeed.Name = "lblLinSpeed";
            lblLinSpeed.Size = new Size(91, 15);
            lblLinSpeed.TabIndex = 0;
            lblLinSpeed.Text = "Speed (mm/s):";
            // 
            // txtLinSpeed
            // 
            txtLinSpeed.Location = new Point(121, 160);
            txtLinSpeed.Name = "txtLinSpeed";
            txtLinSpeed.Size = new Size(100, 23);
            txtLinSpeed.TabIndex = 12;
            txtLinSpeed.Text = "100";
            // 
            // lblMode
            // 
            lblMode.AutoSize = true;
            lblMode.Location = new Point(24, 192);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(41, 15);
            lblMode.TabIndex = 0;
            lblMode.Text = "Mode:";
            // 
            // cboLinMode
            // 
            cboLinMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLinMode.FormattingEnabled = true;
            cboLinMode.Items.AddRange(new object[] { "0", "1", "2" });
            cboLinMode.Location = new Point(121, 189);
            cboLinMode.Name = "cboLinMode";
            cboLinMode.Size = new Size(100, 23);
            cboLinMode.TabIndex = 13;
            // 
            // lblSmooth
            // 
            lblSmooth.AutoSize = true;
            lblSmooth.Location = new Point(24, 221);
            lblSmooth.Name = "lblSmooth";
            lblSmooth.Size = new Size(53, 15);
            lblSmooth.TabIndex = 0;
            lblSmooth.Text = "Smooth:";
            // 
            // txtSmooth
            // 
            txtSmooth.Location = new Point(121, 218);
            txtSmooth.Name = "txtSmooth";
            txtSmooth.Size = new Size(100, 23);
            txtSmooth.TabIndex = 14;
            txtSmooth.Text = "0";
            // 
            // btnLinTest
            // 
            btnLinTest.Location = new Point(24, 260);
            btnLinTest.Name = "btnLinTest";
            btnLinTest.Size = new Size(262, 30);
            btnLinTest.TabIndex = 15;
            btnLinTest.Text = "Execute LIN";
            btnLinTest.UseVisualStyleBackColor = true;
            btnLinTest.Click += btnLinTest_Click;
            // 
            // btnGetCurrentPos
            // 
            btnGetCurrentPos.Location = new Point(24, 296);
            btnGetCurrentPos.Name = "btnGetCurrentPos";
            btnGetCurrentPos.Size = new Size(262, 30);
            btnGetCurrentPos.TabIndex = 17;
            btnGetCurrentPos.Text = "Get Current Position";
            btnGetCurrentPos.UseVisualStyleBackColor = true;
            btnGetCurrentPos.Click += btnGetCurrentPos_Click;
            // 
            // txtLog
            // 
            txtLog.Location = new Point(318, 41);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(470, 397);
            txtLog.TabIndex = 3;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnMotorOn);
            Controls.Add(btnMotorOff);
            Controls.Add(btnLinTest);
            Controls.Add(txtSmooth);
            Controls.Add(lblSmooth);
            Controls.Add(cboLinMode);
            Controls.Add(lblMode);
            Controls.Add(txtLinSpeed);
            Controls.Add(lblLinSpeed);
            Controls.Add(txtC);
            Controls.Add(lblC);
            Controls.Add(txtB);
            Controls.Add(lblB);
            Controls.Add(txtA);
            Controls.Add(lblA);
            Controls.Add(txtZ);
            Controls.Add(lblZ);
            Controls.Add(txtY);
            Controls.Add(lblY);
            Controls.Add(txtX);
            Controls.Add(lblX);
            Controls.Add(grpLinTest);
            Controls.Add(btnExit);
            Controls.Add(txtLog);
            Controls.Add(btnDisconnect);
            Controls.Add(btnConnect);
            Controls.Add(txtIP);
            Name = "Form1";
            Text = "RA605 HRSDK Connection Test";
            ResumeLayout(false);
            PerformLayout();
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
    }
}
