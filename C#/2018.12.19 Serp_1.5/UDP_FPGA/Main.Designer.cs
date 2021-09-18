namespace UDP_FPGA
{
    partial class FormUDP
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUDP));
            this.timerClock = new System.Windows.Forms.Timer(this.components);
            this.labelTimer = new System.Windows.Forms.Label();
            this.checkBoxLEDS = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.timerCheckInfo = new System.Windows.Forms.Timer(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.labelConnection = new System.Windows.Forms.Label();
            this.panelLFTChannels = new System.Windows.Forms.Panel();
            this.checkBox5800M = new System.Windows.Forms.CheckBox();
            this.checkBox2400M = new System.Windows.Forms.CheckBox();
            this.checkBox1200M = new System.Windows.Forms.CheckBox();
            this.checkBox900M = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.labelVoltage = new System.Windows.Forms.Label();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.labelTemperature = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.checkBoxExtControl = new System.Windows.Forms.CheckBox();
            this.panelStatusFlags = new System.Windows.Forms.Panel();
            this.checkBoxErr = new System.Windows.Forms.CheckBox();
            this.checkBoxNewCoordinatesReceived = new System.Windows.Forms.CheckBox();
            this.checkBoxRF = new System.Windows.Forms.CheckBox();
            this.checkBoxTRG = new System.Windows.Forms.CheckBox();
            this.checkBoxRDY = new System.Windows.Forms.CheckBox();
            this.checkBoxCORR = new System.Windows.Forms.CheckBox();
            this.labelTargetAzimuth = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.timerSendDataToLocator = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.labelLatitude = new System.Windows.Forms.Label();
            this.labelLongitude = new System.Windows.Forms.Label();
            this.labelAltitude = new System.Windows.Forms.Label();
            this.panelGPS = new System.Windows.Forms.Panel();
            this.label27 = new System.Windows.Forms.Label();
            this.checkBoxShowSettings = new System.Windows.Forms.CheckBox();
            this.comboBoxSerpNum = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.labelRotatorAzimuth = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.labelRotatorElevation = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.buttonRotatorStop = new System.Windows.Forms.Button();
            this.textBoxElevation = new System.Windows.Forms.TextBox();
            this.textBoxAzimuth = new System.Windows.Forms.TextBox();
            this.buttonRotatorSet = new System.Windows.Forms.Button();
            this.buttonRotatorSendCommand = new System.Windows.Forms.Button();
            this.textBoxRotatorCommand = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.textBoxElevationOffset = new System.Windows.Forms.TextBox();
            this.textBoxAzimuthOffset = new System.Windows.Forms.TextBox();
            this.buttonRefreshRotatorOffsets = new System.Windows.Forms.Button();
            this.checkBoxManualGPS = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.textBoxAltitude = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.textBoxLongitude = new System.Windows.Forms.TextBox();
            this.textBoxLatitude = new System.Windows.Forms.TextBox();
            this.buttonApplyGPSSettings = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.panelRotatorSettings = new System.Windows.Forms.Panel();
            this.label28 = new System.Windows.Forms.Label();
            this.richTextBoxRotatorStatus = new System.Windows.Forms.RichTextBox();
            this.buttonPark = new System.Windows.Forms.Button();
            this.checkBoxTrueAngles = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxRF_ATT = new System.Windows.Forms.TextBox();
            this.textBoxRF_PSK_F = new System.Windows.Forms.TextBox();
            this.textBoxRF_PSK_B = new System.Windows.Forms.TextBox();
            this.textBoxRF_LFT_SWR = new System.Windows.Forms.TextBox();
            this.textBoxRF_LFT_F2 = new System.Windows.Forms.TextBox();
            this.textBoxRF_LFT_F1 = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.checkBoxRF_PSKen = new System.Windows.Forms.CheckBox();
            this.checkBoxRF_LFTen = new System.Windows.Forms.CheckBox();
            this.checkBoxRFen = new System.Windows.Forms.CheckBox();
            this.panelAD9361 = new System.Windows.Forms.Panel();
            this.buttonResend = new System.Windows.Forms.Button();
            this.buttonGPSRequest = new System.Windows.Forms.Button();
            this.buttonControllerAsk = new System.Windows.Forms.Button();
            this.panelRotator = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.panelGPSsettings = new System.Windows.Forms.Panel();
            this.panelTarget = new System.Windows.Forms.Panel();
            this.labelTargetDistance = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.labelTargetElevation = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.panelLFTChannels.SuspendLayout();
            this.panelInfo.SuspendLayout();
            this.panelStatusFlags.SuspendLayout();
            this.panelGPS.SuspendLayout();
            this.panelRotatorSettings.SuspendLayout();
            this.panelAD9361.SuspendLayout();
            this.panelRotator.SuspendLayout();
            this.panelGPSsettings.SuspendLayout();
            this.panelTarget.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerClock
            // 
            this.timerClock.Interval = 1000;
            this.timerClock.Tick += new System.EventHandler(this.timerClock_Tick);
            // 
            // labelTimer
            // 
            this.labelTimer.AutoSize = true;
            this.labelTimer.Location = new System.Drawing.Point(67, 6);
            this.labelTimer.Name = "labelTimer";
            this.labelTimer.Size = new System.Drawing.Size(49, 13);
            this.labelTimer.TabIndex = 1;
            this.labelTimer.Text = "00:00:00";
            // 
            // checkBoxLEDS
            // 
            this.checkBoxLEDS.AutoSize = true;
            this.checkBoxLEDS.Location = new System.Drawing.Point(10, 94);
            this.checkBoxLEDS.Name = "checkBoxLEDS";
            this.checkBoxLEDS.Size = new System.Drawing.Size(54, 17);
            this.checkBoxLEDS.TabIndex = 4;
            this.checkBoxLEDS.TabStop = false;
            this.checkBoxLEDS.Text = "LEDS";
            this.checkBoxLEDS.UseVisualStyleBackColor = true;
            this.checkBoxLEDS.Click += new System.EventHandler(this.tSettingsCheckBox_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Active time:";
            // 
            // timerCheckInfo
            // 
            this.timerCheckInfo.Interval = 2000;
            this.timerCheckInfo.Tick += new System.EventHandler(this.timerCheckInfo_Tick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Connection:";
            // 
            // labelConnection
            // 
            this.labelConnection.AutoSize = true;
            this.labelConnection.ForeColor = System.Drawing.Color.DarkRed;
            this.labelConnection.Location = new System.Drawing.Point(67, 70);
            this.labelConnection.Name = "labelConnection";
            this.labelConnection.Size = new System.Drawing.Size(27, 13);
            this.labelConnection.TabIndex = 7;
            this.labelConnection.Text = "OFF";
            // 
            // panelLFTChannels
            // 
            this.panelLFTChannels.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLFTChannels.Controls.Add(this.checkBox5800M);
            this.panelLFTChannels.Controls.Add(this.checkBox2400M);
            this.panelLFTChannels.Controls.Add(this.checkBox1200M);
            this.panelLFTChannels.Controls.Add(this.checkBox900M);
            this.panelLFTChannels.Controls.Add(this.checkBoxLEDS);
            this.panelLFTChannels.Enabled = false;
            this.panelLFTChannels.Location = new System.Drawing.Point(399, 5);
            this.panelLFTChannels.Name = "panelLFTChannels";
            this.panelLFTChannels.Size = new System.Drawing.Size(74, 120);
            this.panelLFTChannels.TabIndex = 135;
            // 
            // checkBox5800M
            // 
            this.checkBox5800M.AutoSize = true;
            this.checkBox5800M.Location = new System.Drawing.Point(10, 73);
            this.checkBox5800M.Name = "checkBox5800M";
            this.checkBox5800M.Size = new System.Drawing.Size(59, 17);
            this.checkBox5800M.TabIndex = 3;
            this.checkBox5800M.TabStop = false;
            this.checkBox5800M.Text = "5800M";
            this.checkBox5800M.UseVisualStyleBackColor = true;
            this.checkBox5800M.Click += new System.EventHandler(this.tSettingsCheckBox_Click);
            // 
            // checkBox2400M
            // 
            this.checkBox2400M.AutoSize = true;
            this.checkBox2400M.Location = new System.Drawing.Point(10, 52);
            this.checkBox2400M.Name = "checkBox2400M";
            this.checkBox2400M.Size = new System.Drawing.Size(59, 17);
            this.checkBox2400M.TabIndex = 2;
            this.checkBox2400M.TabStop = false;
            this.checkBox2400M.Text = "2400M";
            this.checkBox2400M.UseVisualStyleBackColor = true;
            this.checkBox2400M.Click += new System.EventHandler(this.tSettingsCheckBox_Click);
            // 
            // checkBox1200M
            // 
            this.checkBox1200M.AutoSize = true;
            this.checkBox1200M.Location = new System.Drawing.Point(10, 31);
            this.checkBox1200M.Name = "checkBox1200M";
            this.checkBox1200M.Size = new System.Drawing.Size(59, 17);
            this.checkBox1200M.TabIndex = 1;
            this.checkBox1200M.TabStop = false;
            this.checkBox1200M.Text = "1200M";
            this.checkBox1200M.UseVisualStyleBackColor = true;
            this.checkBox1200M.Click += new System.EventHandler(this.tSettingsCheckBox_Click);
            // 
            // checkBox900M
            // 
            this.checkBox900M.AutoSize = true;
            this.checkBox900M.Location = new System.Drawing.Point(10, 10);
            this.checkBox900M.Name = "checkBox900M";
            this.checkBox900M.Size = new System.Drawing.Size(53, 17);
            this.checkBox900M.TabIndex = 0;
            this.checkBox900M.TabStop = false;
            this.checkBox900M.Text = "900M";
            this.checkBox900M.UseVisualStyleBackColor = true;
            this.checkBox900M.Click += new System.EventHandler(this.tSettingsCheckBox_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(23, 27);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(46, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "Voltage:";
            // 
            // labelVoltage
            // 
            this.labelVoltage.AutoSize = true;
            this.labelVoltage.Location = new System.Drawing.Point(67, 27);
            this.labelVoltage.Name = "labelVoltage";
            this.labelVoltage.Size = new System.Drawing.Size(24, 13);
            this.labelVoltage.TabIndex = 3;
            this.labelVoltage.Text = "n/a";
            // 
            // panelInfo
            // 
            this.panelInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelInfo.Controls.Add(this.labelTemperature);
            this.panelInfo.Controls.Add(this.label16);
            this.panelInfo.Controls.Add(this.labelVoltage);
            this.panelInfo.Controls.Add(this.labelTimer);
            this.panelInfo.Controls.Add(this.label11);
            this.panelInfo.Controls.Add(this.label3);
            this.panelInfo.Controls.Add(this.label4);
            this.panelInfo.Controls.Add(this.labelConnection);
            this.panelInfo.Location = new System.Drawing.Point(5, 32);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(128, 93);
            this.panelInfo.TabIndex = 10;
            // 
            // labelTemperature
            // 
            this.labelTemperature.AutoSize = true;
            this.labelTemperature.Location = new System.Drawing.Point(67, 48);
            this.labelTemperature.Name = "labelTemperature";
            this.labelTemperature.Size = new System.Drawing.Size(24, 13);
            this.labelTemperature.TabIndex = 5;
            this.labelTemperature.Text = "n/a";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(-1, 48);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(70, 13);
            this.label16.TabIndex = 4;
            this.label16.Text = "Temperature:";
            // 
            // checkBoxExtControl
            // 
            this.checkBoxExtControl.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxExtControl.AutoSize = true;
            this.checkBoxExtControl.Location = new System.Drawing.Point(5, 163);
            this.checkBoxExtControl.MaximumSize = new System.Drawing.Size(128, 32);
            this.checkBoxExtControl.MinimumSize = new System.Drawing.Size(128, 32);
            this.checkBoxExtControl.Name = "checkBoxExtControl";
            this.checkBoxExtControl.Size = new System.Drawing.Size(128, 32);
            this.checkBoxExtControl.TabIndex = 1;
            this.checkBoxExtControl.TabStop = false;
            this.checkBoxExtControl.Text = "External control";
            this.checkBoxExtControl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxExtControl.UseVisualStyleBackColor = true;
            this.checkBoxExtControl.CheckedChanged += new System.EventHandler(this.checkBoxExtControl_CheckedChanged);
            // 
            // panelStatusFlags
            // 
            this.panelStatusFlags.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelStatusFlags.Controls.Add(this.checkBoxErr);
            this.panelStatusFlags.Controls.Add(this.checkBoxNewCoordinatesReceived);
            this.panelStatusFlags.Controls.Add(this.checkBoxRF);
            this.panelStatusFlags.Controls.Add(this.checkBoxTRG);
            this.panelStatusFlags.Controls.Add(this.checkBoxRDY);
            this.panelStatusFlags.Controls.Add(this.checkBoxCORR);
            this.panelStatusFlags.Location = new System.Drawing.Point(479, 5);
            this.panelStatusFlags.Name = "panelStatusFlags";
            this.panelStatusFlags.Size = new System.Drawing.Size(74, 120);
            this.panelStatusFlags.TabIndex = 9;
            // 
            // checkBoxErr
            // 
            this.checkBoxErr.AutoSize = true;
            this.checkBoxErr.Location = new System.Drawing.Point(9, 95);
            this.checkBoxErr.Name = "checkBoxErr";
            this.checkBoxErr.Size = new System.Drawing.Size(49, 17);
            this.checkBoxErr.TabIndex = 142;
            this.checkBoxErr.TabStop = false;
            this.checkBoxErr.Text = "ERR";
            this.checkBoxErr.UseVisualStyleBackColor = true;
            // 
            // checkBoxNewCoordinatesReceived
            // 
            this.checkBoxNewCoordinatesReceived.AutoSize = true;
            this.checkBoxNewCoordinatesReceived.Location = new System.Drawing.Point(9, 78);
            this.checkBoxNewCoordinatesReceived.Name = "checkBoxNewCoordinatesReceived";
            this.checkBoxNewCoordinatesReceived.Size = new System.Drawing.Size(52, 17);
            this.checkBoxNewCoordinatesReceived.TabIndex = 141;
            this.checkBoxNewCoordinatesReceived.TabStop = false;
            this.checkBoxNewCoordinatesReceived.Text = "NEW";
            this.checkBoxNewCoordinatesReceived.UseVisualStyleBackColor = true;
            this.checkBoxNewCoordinatesReceived.Click += new System.EventHandler(this.nonClickableCheckBoxes_Click);
            // 
            // checkBoxRF
            // 
            this.checkBoxRF.AutoSize = true;
            this.checkBoxRF.Location = new System.Drawing.Point(9, 44);
            this.checkBoxRF.Name = "checkBoxRF";
            this.checkBoxRF.Size = new System.Drawing.Size(40, 17);
            this.checkBoxRF.TabIndex = 127;
            this.checkBoxRF.TabStop = false;
            this.checkBoxRF.Text = "RF";
            this.checkBoxRF.UseVisualStyleBackColor = true;
            this.checkBoxRF.Click += new System.EventHandler(this.nonClickableCheckBoxes_Click);
            // 
            // checkBoxTRG
            // 
            this.checkBoxTRG.AutoSize = true;
            this.checkBoxTRG.Location = new System.Drawing.Point(9, 61);
            this.checkBoxTRG.Name = "checkBoxTRG";
            this.checkBoxTRG.Size = new System.Drawing.Size(49, 17);
            this.checkBoxTRG.TabIndex = 129;
            this.checkBoxTRG.TabStop = false;
            this.checkBoxTRG.Text = "TRG";
            this.checkBoxTRG.UseVisualStyleBackColor = true;
            this.checkBoxTRG.Click += new System.EventHandler(this.nonClickableCheckBoxes_Click);
            // 
            // checkBoxRDY
            // 
            this.checkBoxRDY.AutoSize = true;
            this.checkBoxRDY.Location = new System.Drawing.Point(9, 10);
            this.checkBoxRDY.Name = "checkBoxRDY";
            this.checkBoxRDY.Size = new System.Drawing.Size(49, 17);
            this.checkBoxRDY.TabIndex = 128;
            this.checkBoxRDY.TabStop = false;
            this.checkBoxRDY.Text = "RDY";
            this.checkBoxRDY.UseVisualStyleBackColor = true;
            this.checkBoxRDY.Click += new System.EventHandler(this.nonClickableCheckBoxes_Click);
            // 
            // checkBoxCORR
            // 
            this.checkBoxCORR.AutoSize = true;
            this.checkBoxCORR.Location = new System.Drawing.Point(9, 27);
            this.checkBoxCORR.Name = "checkBoxCORR";
            this.checkBoxCORR.Size = new System.Drawing.Size(57, 17);
            this.checkBoxCORR.TabIndex = 130;
            this.checkBoxCORR.TabStop = false;
            this.checkBoxCORR.Text = "CORR";
            this.checkBoxCORR.UseVisualStyleBackColor = true;
            this.checkBoxCORR.Click += new System.EventHandler(this.nonClickableCheckBoxes_Click);
            // 
            // labelTargetAzimuth
            // 
            this.labelTargetAzimuth.AutoSize = true;
            this.labelTargetAzimuth.Location = new System.Drawing.Point(66, 23);
            this.labelTargetAzimuth.Name = "labelTargetAzimuth";
            this.labelTargetAzimuth.Size = new System.Drawing.Size(24, 13);
            this.labelTargetAzimuth.TabIndex = 101;
            this.labelTargetAzimuth.Text = "n/a";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 124;
            this.label5.Text = "Target";
            // 
            // timerSendDataToLocator
            // 
            this.timerSendDataToLocator.Interval = 500;
            this.timerSendDataToLocator.Tick += new System.EventHandler(this.timerSendDataToLocator_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label1.Location = new System.Drawing.Point(15, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 147;
            this.label1.Text = "Latitude";
            this.label1.DoubleClick += new System.EventHandler(this.panelGPS_DoubleClick);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label6.Location = new System.Drawing.Point(15, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 13);
            this.label6.TabIndex = 146;
            this.label6.Text = "Longitude";
            this.label6.DoubleClick += new System.EventHandler(this.panelGPS_DoubleClick);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label7.Location = new System.Drawing.Point(15, 77);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 13);
            this.label7.TabIndex = 145;
            this.label7.Text = "Altitude";
            this.label7.DoubleClick += new System.EventHandler(this.panelGPS_DoubleClick);
            // 
            // labelLatitude
            // 
            this.labelLatitude.AutoSize = true;
            this.labelLatitude.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.labelLatitude.Location = new System.Drawing.Point(70, 24);
            this.labelLatitude.Name = "labelLatitude";
            this.labelLatitude.Size = new System.Drawing.Size(24, 13);
            this.labelLatitude.TabIndex = 144;
            this.labelLatitude.Text = "n/a";
            this.labelLatitude.DoubleClick += new System.EventHandler(this.panelGPS_DoubleClick);
            // 
            // labelLongitude
            // 
            this.labelLongitude.AutoSize = true;
            this.labelLongitude.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.labelLongitude.Location = new System.Drawing.Point(70, 50);
            this.labelLongitude.Name = "labelLongitude";
            this.labelLongitude.Size = new System.Drawing.Size(24, 13);
            this.labelLongitude.TabIndex = 143;
            this.labelLongitude.Text = "n/a";
            this.labelLongitude.DoubleClick += new System.EventHandler(this.panelGPS_DoubleClick);
            // 
            // labelAltitude
            // 
            this.labelAltitude.AutoSize = true;
            this.labelAltitude.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.labelAltitude.Location = new System.Drawing.Point(70, 77);
            this.labelAltitude.Name = "labelAltitude";
            this.labelAltitude.Size = new System.Drawing.Size(24, 13);
            this.labelAltitude.TabIndex = 142;
            this.labelAltitude.Text = "n/a";
            this.labelAltitude.DoubleClick += new System.EventHandler(this.panelGPS_DoubleClick);
            // 
            // panelGPS
            // 
            this.panelGPS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelGPS.Controls.Add(this.label27);
            this.panelGPS.Controls.Add(this.label6);
            this.panelGPS.Controls.Add(this.label1);
            this.panelGPS.Controls.Add(this.labelAltitude);
            this.panelGPS.Controls.Add(this.labelLongitude);
            this.panelGPS.Controls.Add(this.label7);
            this.panelGPS.Controls.Add(this.labelLatitude);
            this.panelGPS.Location = new System.Drawing.Point(399, 130);
            this.panelGPS.Name = "panelGPS";
            this.panelGPS.Size = new System.Drawing.Size(154, 101);
            this.panelGPS.TabIndex = 148;
            this.panelGPS.DoubleClick += new System.EventHandler(this.panelGPS_DoubleClick);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(4, 5);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(99, 13);
            this.label27.TabIndex = 148;
            this.label27.Text = "Device coordinates";
            // 
            // checkBoxShowSettings
            // 
            this.checkBoxShowSettings.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxShowSettings.AutoSize = true;
            this.checkBoxShowSettings.Location = new System.Drawing.Point(5, 199);
            this.checkBoxShowSettings.MaximumSize = new System.Drawing.Size(128, 32);
            this.checkBoxShowSettings.MinimumSize = new System.Drawing.Size(128, 32);
            this.checkBoxShowSettings.Name = "checkBoxShowSettings";
            this.checkBoxShowSettings.Size = new System.Drawing.Size(128, 32);
            this.checkBoxShowSettings.TabIndex = 2;
            this.checkBoxShowSettings.TabStop = false;
            this.checkBoxShowSettings.Text = "Settings";
            this.checkBoxShowSettings.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxShowSettings.UseVisualStyleBackColor = true;
            this.checkBoxShowSettings.CheckedChanged += new System.EventHandler(this.checkBoxShowSettings_CheckedChanged);
            // 
            // comboBoxSerpNum
            // 
            this.comboBoxSerpNum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSerpNum.Location = new System.Drawing.Point(67, 5);
            this.comboBoxSerpNum.Name = "comboBoxSerpNum";
            this.comboBoxSerpNum.Size = new System.Drawing.Size(66, 21);
            this.comboBoxSerpNum.TabIndex = 8;
            this.comboBoxSerpNum.TabStop = false;
            this.comboBoxSerpNum.SelectedIndexChanged += new System.EventHandler(this.comboBoxSerpNum_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(21, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 13);
            this.label8.TabIndex = 200;
            this.label8.Text = "Azimuth";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 50);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(51, 13);
            this.label9.TabIndex = 201;
            this.label9.Text = "Elevation";
            // 
            // labelRotatorAzimuth
            // 
            this.labelRotatorAzimuth.AutoSize = true;
            this.labelRotatorAzimuth.ForeColor = System.Drawing.Color.Black;
            this.labelRotatorAzimuth.Location = new System.Drawing.Point(73, 24);
            this.labelRotatorAzimuth.Name = "labelRotatorAzimuth";
            this.labelRotatorAzimuth.Size = new System.Drawing.Size(24, 13);
            this.labelRotatorAzimuth.TabIndex = 202;
            this.labelRotatorAzimuth.Text = "n/a";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(10, 29);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(51, 13);
            this.label12.TabIndex = 209;
            this.label12.Text = "Elevation";
            // 
            // labelRotatorElevation
            // 
            this.labelRotatorElevation.AutoSize = true;
            this.labelRotatorElevation.ForeColor = System.Drawing.Color.Black;
            this.labelRotatorElevation.Location = new System.Drawing.Point(73, 50);
            this.labelRotatorElevation.Name = "labelRotatorElevation";
            this.labelRotatorElevation.Size = new System.Drawing.Size(24, 13);
            this.labelRotatorElevation.TabIndex = 203;
            this.labelRotatorElevation.Text = "n/a";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(17, 6);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(44, 13);
            this.label13.TabIndex = 208;
            this.label13.Text = "Azimuth";
            // 
            // buttonRotatorStop
            // 
            this.buttonRotatorStop.Location = new System.Drawing.Point(100, 68);
            this.buttonRotatorStop.Name = "buttonRotatorStop";
            this.buttonRotatorStop.Size = new System.Drawing.Size(41, 38);
            this.buttonRotatorStop.TabIndex = 3;
            this.buttonRotatorStop.TabStop = false;
            this.buttonRotatorStop.Text = "Stop";
            this.buttonRotatorStop.UseVisualStyleBackColor = true;
            this.buttonRotatorStop.Click += new System.EventHandler(this.buttonRotatorStop_Click);
            // 
            // textBoxElevation
            // 
            this.textBoxElevation.Location = new System.Drawing.Point(59, 26);
            this.textBoxElevation.Name = "textBoxElevation";
            this.textBoxElevation.Size = new System.Drawing.Size(37, 20);
            this.textBoxElevation.TabIndex = 1;
            this.textBoxElevation.Text = "0";
            this.textBoxElevation.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumericSign_KeyPress);
            // 
            // textBoxAzimuth
            // 
            this.textBoxAzimuth.Location = new System.Drawing.Point(59, 3);
            this.textBoxAzimuth.Name = "textBoxAzimuth";
            this.textBoxAzimuth.Size = new System.Drawing.Size(37, 20);
            this.textBoxAzimuth.TabIndex = 0;
            this.textBoxAzimuth.Text = "0";
            this.textBoxAzimuth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumericSign_KeyPress);
            // 
            // buttonRotatorSet
            // 
            this.buttonRotatorSet.Location = new System.Drawing.Point(100, 7);
            this.buttonRotatorSet.Name = "buttonRotatorSet";
            this.buttonRotatorSet.Size = new System.Drawing.Size(41, 38);
            this.buttonRotatorSet.TabIndex = 2;
            this.buttonRotatorSet.TabStop = false;
            this.buttonRotatorSet.Text = "Set";
            this.buttonRotatorSet.UseVisualStyleBackColor = true;
            this.buttonRotatorSet.Click += new System.EventHandler(this.buttonRotatorSet_Click);
            // 
            // buttonRotatorSendCommand
            // 
            this.buttonRotatorSendCommand.Location = new System.Drawing.Point(232, 7);
            this.buttonRotatorSendCommand.Name = "buttonRotatorSendCommand";
            this.buttonRotatorSendCommand.Size = new System.Drawing.Size(41, 38);
            this.buttonRotatorSendCommand.TabIndex = 5;
            this.buttonRotatorSendCommand.TabStop = false;
            this.buttonRotatorSendCommand.Text = "Send";
            this.buttonRotatorSendCommand.UseVisualStyleBackColor = true;
            this.buttonRotatorSendCommand.Click += new System.EventHandler(this.buttonRotatorSendCommand_Click);
            // 
            // textBoxRotatorCommand
            // 
            this.textBoxRotatorCommand.Location = new System.Drawing.Point(147, 17);
            this.textBoxRotatorCommand.Name = "textBoxRotatorCommand";
            this.textBoxRotatorCommand.Size = new System.Drawing.Size(82, 20);
            this.textBoxRotatorCommand.TabIndex = 4;
            this.textBoxRotatorCommand.TabStop = false;
            this.toolTip1.SetToolTip(this.textBoxRotatorCommand, resources.GetString("textBoxRotatorCommand.ToolTip"));
            // 
            // textBoxElevationOffset
            // 
            this.textBoxElevationOffset.Location = new System.Drawing.Point(339, 45);
            this.textBoxElevationOffset.Name = "textBoxElevationOffset";
            this.textBoxElevationOffset.Size = new System.Drawing.Size(41, 20);
            this.textBoxElevationOffset.TabIndex = 7;
            this.textBoxElevationOffset.TabStop = false;
            this.textBoxElevationOffset.Text = "0";
            this.toolTip1.SetToolTip(this.textBoxElevationOffset, "RotatorAngle = AngleWithOffset + Offset");
            this.textBoxElevationOffset.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumericSign_KeyPress);
            // 
            // textBoxAzimuthOffset
            // 
            this.textBoxAzimuthOffset.Location = new System.Drawing.Point(339, 22);
            this.textBoxAzimuthOffset.Name = "textBoxAzimuthOffset";
            this.textBoxAzimuthOffset.Size = new System.Drawing.Size(41, 20);
            this.textBoxAzimuthOffset.TabIndex = 6;
            this.textBoxAzimuthOffset.TabStop = false;
            this.textBoxAzimuthOffset.Text = "0";
            this.toolTip1.SetToolTip(this.textBoxAzimuthOffset, "RotatorAngle = AngleWithOffset + Offset");
            this.textBoxAzimuthOffset.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumericSign_KeyPress);
            // 
            // buttonRefreshRotatorOffsets
            // 
            this.buttonRefreshRotatorOffsets.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.buttonRefreshRotatorOffsets.Location = new System.Drawing.Point(339, 68);
            this.buttonRefreshRotatorOffsets.Name = "buttonRefreshRotatorOffsets";
            this.buttonRefreshRotatorOffsets.Size = new System.Drawing.Size(41, 38);
            this.buttonRefreshRotatorOffsets.TabIndex = 8;
            this.buttonRefreshRotatorOffsets.TabStop = false;
            this.buttonRefreshRotatorOffsets.Text = "Apply";
            this.toolTip1.SetToolTip(this.buttonRefreshRotatorOffsets, "offset - показания ОПУ в горизонтальном положении АС в направлении севера");
            this.buttonRefreshRotatorOffsets.UseVisualStyleBackColor = true;
            this.buttonRefreshRotatorOffsets.Click += new System.EventHandler(this.buttonRefreshRotatorOffsets_Click);
            // 
            // checkBoxManualGPS
            // 
            this.checkBoxManualGPS.AutoSize = true;
            this.checkBoxManualGPS.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxManualGPS.Location = new System.Drawing.Point(28, 73);
            this.checkBoxManualGPS.Name = "checkBoxManualGPS";
            this.checkBoxManualGPS.Size = new System.Drawing.Size(61, 17);
            this.checkBoxManualGPS.TabIndex = 3;
            this.checkBoxManualGPS.TabStop = false;
            this.checkBoxManualGPS.Text = "Manual";
            this.checkBoxManualGPS.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(31, 52);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(42, 13);
            this.label14.TabIndex = 218;
            this.label14.Text = "Altitude";
            // 
            // textBoxAltitude
            // 
            this.textBoxAltitude.Location = new System.Drawing.Point(74, 49);
            this.textBoxAltitude.Name = "textBoxAltitude";
            this.textBoxAltitude.Size = new System.Drawing.Size(67, 20);
            this.textBoxAltitude.TabIndex = 2;
            this.textBoxAltitude.TabStop = false;
            this.textBoxAltitude.Text = "300";
            this.textBoxAltitude.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumeric_KeyPress);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(19, 29);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(54, 13);
            this.label15.TabIndex = 216;
            this.label15.Text = "Longitude";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(28, 6);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(45, 13);
            this.label17.TabIndex = 215;
            this.label17.Text = "Latitude";
            // 
            // textBoxLongitude
            // 
            this.textBoxLongitude.Location = new System.Drawing.Point(74, 26);
            this.textBoxLongitude.Name = "textBoxLongitude";
            this.textBoxLongitude.Size = new System.Drawing.Size(67, 20);
            this.textBoxLongitude.TabIndex = 1;
            this.textBoxLongitude.TabStop = false;
            this.textBoxLongitude.Text = "0";
            this.textBoxLongitude.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumericDot_KeyPress);
            // 
            // textBoxLatitude
            // 
            this.textBoxLatitude.Location = new System.Drawing.Point(74, 3);
            this.textBoxLatitude.Name = "textBoxLatitude";
            this.textBoxLatitude.Size = new System.Drawing.Size(67, 20);
            this.textBoxLatitude.TabIndex = 0;
            this.textBoxLatitude.TabStop = false;
            this.textBoxLatitude.Text = "0";
            this.textBoxLatitude.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumericDot_KeyPress);
            // 
            // buttonApplyGPSSettings
            // 
            this.buttonApplyGPSSettings.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.buttonApplyGPSSettings.Location = new System.Drawing.Point(100, 69);
            this.buttonApplyGPSSettings.Name = "buttonApplyGPSSettings";
            this.buttonApplyGPSSettings.Size = new System.Drawing.Size(41, 37);
            this.buttonApplyGPSSettings.TabIndex = 4;
            this.buttonApplyGPSSettings.TabStop = false;
            this.buttonApplyGPSSettings.Text = "Apply";
            this.buttonApplyGPSSettings.UseVisualStyleBackColor = true;
            this.buttonApplyGPSSettings.Click += new System.EventHandler(this.buttonApplyGPSSettings_Click);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(290, 48);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(51, 13);
            this.label19.TabIndex = 227;
            this.label19.Text = "Elevation";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(297, 25);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(44, 13);
            this.label20.TabIndex = 226;
            this.label20.Text = "Azimuth";
            // 
            // panelRotatorSettings
            // 
            this.panelRotatorSettings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelRotatorSettings.Controls.Add(this.label28);
            this.panelRotatorSettings.Controls.Add(this.richTextBoxRotatorStatus);
            this.panelRotatorSettings.Controls.Add(this.buttonPark);
            this.panelRotatorSettings.Controls.Add(this.textBoxElevation);
            this.panelRotatorSettings.Controls.Add(this.textBoxAzimuth);
            this.panelRotatorSettings.Controls.Add(this.checkBoxTrueAngles);
            this.panelRotatorSettings.Controls.Add(this.textBoxAzimuthOffset);
            this.panelRotatorSettings.Controls.Add(this.textBoxElevationOffset);
            this.panelRotatorSettings.Controls.Add(this.label19);
            this.panelRotatorSettings.Controls.Add(this.buttonRefreshRotatorOffsets);
            this.panelRotatorSettings.Controls.Add(this.textBoxRotatorCommand);
            this.panelRotatorSettings.Controls.Add(this.label20);
            this.panelRotatorSettings.Controls.Add(this.buttonRotatorSendCommand);
            this.panelRotatorSettings.Controls.Add(this.label12);
            this.panelRotatorSettings.Controls.Add(this.label13);
            this.panelRotatorSettings.Controls.Add(this.buttonRotatorStop);
            this.panelRotatorSettings.Controls.Add(this.buttonRotatorSet);
            this.panelRotatorSettings.Location = new System.Drawing.Point(6, 237);
            this.panelRotatorSettings.Name = "panelRotatorSettings";
            this.panelRotatorSettings.Size = new System.Drawing.Size(387, 112);
            this.panelRotatorSettings.TabIndex = 228;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(283, 3);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(43, 13);
            this.label28.TabIndex = 231;
            this.label28.Text = "Offsets:";
            // 
            // richTextBoxRotatorStatus
            // 
            this.richTextBoxRotatorStatus.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBoxRotatorStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxRotatorStatus.Location = new System.Drawing.Point(147, 48);
            this.richTextBoxRotatorStatus.Name = "richTextBoxRotatorStatus";
            this.richTextBoxRotatorStatus.ReadOnly = true;
            this.richTextBoxRotatorStatus.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBoxRotatorStatus.Size = new System.Drawing.Size(126, 55);
            this.richTextBoxRotatorStatus.TabIndex = 230;
            this.richTextBoxRotatorStatus.TabStop = false;
            this.richTextBoxRotatorStatus.Text = "";
            // 
            // buttonPark
            // 
            this.buttonPark.Location = new System.Drawing.Point(4, 68);
            this.buttonPark.Name = "buttonPark";
            this.buttonPark.Size = new System.Drawing.Size(41, 38);
            this.buttonPark.TabIndex = 229;
            this.buttonPark.TabStop = false;
            this.buttonPark.Text = "Park";
            this.buttonPark.UseVisualStyleBackColor = true;
            this.buttonPark.Click += new System.EventHandler(this.buttonPark_Click);
            // 
            // checkBoxTrueAngles
            // 
            this.checkBoxTrueAngles.AutoSize = true;
            this.checkBoxTrueAngles.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxTrueAngles.Location = new System.Drawing.Point(-3, 49);
            this.checkBoxTrueAngles.Name = "checkBoxTrueAngles";
            this.checkBoxTrueAngles.Size = new System.Drawing.Size(88, 17);
            this.checkBoxTrueAngles.TabIndex = 9;
            this.checkBoxTrueAngles.TabStop = false;
            this.checkBoxTrueAngles.Text = "True angles  ";
            this.checkBoxTrueAngles.UseVisualStyleBackColor = true;
            this.checkBoxTrueAngles.CheckedChanged += new System.EventHandler(this.checkBoxTrueAngles_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Device ID";
            // 
            // textBoxRF_ATT
            // 
            this.textBoxRF_ATT.Location = new System.Drawing.Point(45, 18);
            this.textBoxRF_ATT.Name = "textBoxRF_ATT";
            this.textBoxRF_ATT.Size = new System.Drawing.Size(58, 20);
            this.textBoxRF_ATT.TabIndex = 0;
            this.textBoxRF_ATT.Text = "0";
            this.textBoxRF_ATT.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumeric_KeyPress);
            this.textBoxRF_ATT.Leave += new System.EventHandler(this.textBoxRF_ATT_Leave);
            // 
            // textBoxRF_PSK_F
            // 
            this.textBoxRF_PSK_F.Location = new System.Drawing.Point(45, 55);
            this.textBoxRF_PSK_F.Name = "textBoxRF_PSK_F";
            this.textBoxRF_PSK_F.Size = new System.Drawing.Size(58, 20);
            this.textBoxRF_PSK_F.TabIndex = 1;
            this.textBoxRF_PSK_F.Text = "0";
            this.textBoxRF_PSK_F.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumericDot_KeyPress);
            this.textBoxRF_PSK_F.Leave += new System.EventHandler(this.textBoxRF_F_Leave);
            // 
            // textBoxRF_PSK_B
            // 
            this.textBoxRF_PSK_B.Location = new System.Drawing.Point(111, 55);
            this.textBoxRF_PSK_B.Name = "textBoxRF_PSK_B";
            this.textBoxRF_PSK_B.Size = new System.Drawing.Size(58, 20);
            this.textBoxRF_PSK_B.TabIndex = 2;
            this.textBoxRF_PSK_B.Text = "0";
            this.textBoxRF_PSK_B.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumeric_KeyPress);
            this.textBoxRF_PSK_B.Leave += new System.EventHandler(this.textBoxRF_PSK_B_Leave);
            // 
            // textBoxRF_LFT_SWR
            // 
            this.textBoxRF_LFT_SWR.Location = new System.Drawing.Point(177, 92);
            this.textBoxRF_LFT_SWR.Name = "textBoxRF_LFT_SWR";
            this.textBoxRF_LFT_SWR.Size = new System.Drawing.Size(58, 20);
            this.textBoxRF_LFT_SWR.TabIndex = 5;
            this.textBoxRF_LFT_SWR.Text = "0";
            this.textBoxRF_LFT_SWR.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumeric_KeyPress);
            this.textBoxRF_LFT_SWR.Leave += new System.EventHandler(this.textBoxRF_LFT_SWR_Leave);
            // 
            // textBoxRF_LFT_F2
            // 
            this.textBoxRF_LFT_F2.Location = new System.Drawing.Point(111, 92);
            this.textBoxRF_LFT_F2.Name = "textBoxRF_LFT_F2";
            this.textBoxRF_LFT_F2.Size = new System.Drawing.Size(58, 20);
            this.textBoxRF_LFT_F2.TabIndex = 4;
            this.textBoxRF_LFT_F2.Text = "0";
            this.textBoxRF_LFT_F2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumericDot_KeyPress);
            this.textBoxRF_LFT_F2.Leave += new System.EventHandler(this.textBoxRF_F_Leave);
            // 
            // textBoxRF_LFT_F1
            // 
            this.textBoxRF_LFT_F1.Location = new System.Drawing.Point(45, 92);
            this.textBoxRF_LFT_F1.Name = "textBoxRF_LFT_F1";
            this.textBoxRF_LFT_F1.Size = new System.Drawing.Size(58, 20);
            this.textBoxRF_LFT_F1.TabIndex = 3;
            this.textBoxRF_LFT_F1.Text = "0";
            this.textBoxRF_LFT_F1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumericDot_KeyPress);
            this.textBoxRF_LFT_F1.Leave += new System.EventHandler(this.textBoxRF_F_Leave);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(45, 4);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(50, 13);
            this.label22.TabIndex = 239;
            this.label22.Text = "ATT [dB]";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(109, 78);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(50, 13);
            this.label23.TabIndex = 7;
            this.label23.Text = "F2 [MHz]";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(43, 78);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(50, 13);
            this.label24.TabIndex = 3;
            this.label24.Text = "F1 [MHz]";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(109, 41);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(62, 13);
            this.label25.TabIndex = 5;
            this.label25.Text = "B [kbit/sec]";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(43, 41);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(44, 13);
            this.label26.TabIndex = 1;
            this.label26.Text = "F [MHz]";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(175, 78);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(61, 13);
            this.label29.TabIndex = 246;
            this.label29.Text = "SWR [kHz]";
            // 
            // checkBoxRF_PSKen
            // 
            this.checkBoxRF_PSKen.AutoSize = true;
            this.checkBoxRF_PSKen.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.checkBoxRF_PSKen.Location = new System.Drawing.Point(9, 40);
            this.checkBoxRF_PSKen.Name = "checkBoxRF_PSKen";
            this.checkBoxRF_PSKen.Size = new System.Drawing.Size(32, 31);
            this.checkBoxRF_PSKen.TabIndex = 7;
            this.checkBoxRF_PSKen.TabStop = false;
            this.checkBoxRF_PSKen.Text = "PSK";
            this.checkBoxRF_PSKen.UseVisualStyleBackColor = true;
            this.checkBoxRF_PSKen.CheckedChanged += new System.EventHandler(this.checkBoxRF_PSK_LFT_CheckedChanged);
            this.checkBoxRF_PSKen.Click += new System.EventHandler(this.tSettingsCheckBox_Click);
            // 
            // checkBoxRF_LFTen
            // 
            this.checkBoxRF_LFTen.AutoSize = true;
            this.checkBoxRF_LFTen.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.checkBoxRF_LFTen.Location = new System.Drawing.Point(10, 77);
            this.checkBoxRF_LFTen.Name = "checkBoxRF_LFTen";
            this.checkBoxRF_LFTen.Size = new System.Drawing.Size(30, 31);
            this.checkBoxRF_LFTen.TabIndex = 8;
            this.checkBoxRF_LFTen.TabStop = false;
            this.checkBoxRF_LFTen.Text = "LFT";
            this.checkBoxRF_LFTen.UseVisualStyleBackColor = true;
            this.checkBoxRF_LFTen.CheckedChanged += new System.EventHandler(this.checkBoxRF_PSK_LFT_CheckedChanged);
            this.checkBoxRF_LFTen.Click += new System.EventHandler(this.tSettingsCheckBox_Click);
            // 
            // checkBoxRFen
            // 
            this.checkBoxRFen.AutoSize = true;
            this.checkBoxRFen.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.checkBoxRFen.Location = new System.Drawing.Point(13, 3);
            this.checkBoxRFen.Name = "checkBoxRFen";
            this.checkBoxRFen.Size = new System.Drawing.Size(26, 31);
            this.checkBoxRFen.TabIndex = 6;
            this.checkBoxRFen.TabStop = false;
            this.checkBoxRFen.Text = "EN";
            this.checkBoxRFen.UseVisualStyleBackColor = true;
            this.checkBoxRFen.CheckedChanged += new System.EventHandler(this.checkBoxRF_PSK_LFT_CheckedChanged);
            this.checkBoxRFen.Click += new System.EventHandler(this.tSettingsCheckBox_Click);
            // 
            // panelAD9361
            // 
            this.panelAD9361.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelAD9361.Controls.Add(this.buttonResend);
            this.panelAD9361.Controls.Add(this.checkBoxRFen);
            this.panelAD9361.Controls.Add(this.textBoxRF_ATT);
            this.panelAD9361.Controls.Add(this.checkBoxRF_LFTen);
            this.panelAD9361.Controls.Add(this.textBoxRF_PSK_F);
            this.panelAD9361.Controls.Add(this.checkBoxRF_PSKen);
            this.panelAD9361.Controls.Add(this.textBoxRF_PSK_B);
            this.panelAD9361.Controls.Add(this.label29);
            this.panelAD9361.Controls.Add(this.textBoxRF_LFT_F1);
            this.panelAD9361.Controls.Add(this.label26);
            this.panelAD9361.Controls.Add(this.textBoxRF_LFT_F2);
            this.panelAD9361.Controls.Add(this.label25);
            this.panelAD9361.Controls.Add(this.textBoxRF_LFT_SWR);
            this.panelAD9361.Controls.Add(this.label24);
            this.panelAD9361.Controls.Add(this.label22);
            this.panelAD9361.Controls.Add(this.label23);
            this.panelAD9361.Enabled = false;
            this.panelAD9361.Location = new System.Drawing.Point(139, 5);
            this.panelAD9361.Name = "panelAD9361";
            this.panelAD9361.Size = new System.Drawing.Size(254, 120);
            this.panelAD9361.TabIndex = 251;
            // 
            // buttonResend
            // 
            this.buttonResend.Location = new System.Drawing.Point(178, 15);
            this.buttonResend.Name = "buttonResend";
            this.buttonResend.Size = new System.Drawing.Size(57, 23);
            this.buttonResend.TabIndex = 9;
            this.buttonResend.TabStop = false;
            this.buttonResend.Text = "Resend";
            this.buttonResend.UseVisualStyleBackColor = true;
            this.buttonResend.Click += new System.EventHandler(this.buttonResend_Click);
            // 
            // buttonGPSRequest
            // 
            this.buttonGPSRequest.Location = new System.Drawing.Point(71, 129);
            this.buttonGPSRequest.Name = "buttonGPSRequest";
            this.buttonGPSRequest.Size = new System.Drawing.Size(62, 31);
            this.buttonGPSRequest.TabIndex = 248;
            this.buttonGPSRequest.TabStop = false;
            this.buttonGPSRequest.Text = "GPS";
            this.buttonGPSRequest.UseVisualStyleBackColor = true;
            this.buttonGPSRequest.Click += new System.EventHandler(this.buttonGPSRequest_Click);
            // 
            // buttonControllerAsk
            // 
            this.buttonControllerAsk.Location = new System.Drawing.Point(5, 129);
            this.buttonControllerAsk.Name = "buttonControllerAsk";
            this.buttonControllerAsk.Size = new System.Drawing.Size(62, 31);
            this.buttonControllerAsk.TabIndex = 249;
            this.buttonControllerAsk.TabStop = false;
            this.buttonControllerAsk.Text = "T°";
            this.buttonControllerAsk.UseVisualStyleBackColor = true;
            this.buttonControllerAsk.Click += new System.EventHandler(this.buttonControllerAsk_Click);
            // 
            // panelRotator
            // 
            this.panelRotator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelRotator.Controls.Add(this.label10);
            this.panelRotator.Controls.Add(this.labelRotatorElevation);
            this.panelRotator.Controls.Add(this.labelRotatorAzimuth);
            this.panelRotator.Controls.Add(this.label9);
            this.panelRotator.Controls.Add(this.label8);
            this.panelRotator.Location = new System.Drawing.Point(139, 130);
            this.panelRotator.Name = "panelRotator";
            this.panelRotator.Size = new System.Drawing.Size(124, 101);
            this.panelRotator.TabIndex = 252;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(5, 5);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(42, 13);
            this.label10.TabIndex = 204;
            this.label10.Text = "Rotator";
            // 
            // panelGPSsettings
            // 
            this.panelGPSsettings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelGPSsettings.Controls.Add(this.label15);
            this.panelGPSsettings.Controls.Add(this.buttonApplyGPSSettings);
            this.panelGPSsettings.Controls.Add(this.label17);
            this.panelGPSsettings.Controls.Add(this.checkBoxManualGPS);
            this.panelGPSsettings.Controls.Add(this.label14);
            this.panelGPSsettings.Controls.Add(this.textBoxLatitude);
            this.panelGPSsettings.Controls.Add(this.textBoxAltitude);
            this.panelGPSsettings.Controls.Add(this.textBoxLongitude);
            this.panelGPSsettings.Location = new System.Drawing.Point(399, 237);
            this.panelGPSsettings.Name = "panelGPSsettings";
            this.panelGPSsettings.Size = new System.Drawing.Size(154, 112);
            this.panelGPSsettings.TabIndex = 253;
            // 
            // panelTarget
            // 
            this.panelTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTarget.Controls.Add(this.labelTargetDistance);
            this.panelTarget.Controls.Add(this.label30);
            this.panelTarget.Controls.Add(this.labelTargetElevation);
            this.panelTarget.Controls.Add(this.label18);
            this.panelTarget.Controls.Add(this.label21);
            this.panelTarget.Controls.Add(this.labelTargetAzimuth);
            this.panelTarget.Controls.Add(this.label5);
            this.panelTarget.Enabled = false;
            this.panelTarget.Location = new System.Drawing.Point(269, 130);
            this.panelTarget.Name = "panelTarget";
            this.panelTarget.Size = new System.Drawing.Size(124, 101);
            this.panelTarget.TabIndex = 254;
            // 
            // labelTargetDistance
            // 
            this.labelTargetDistance.AutoSize = true;
            this.labelTargetDistance.Location = new System.Drawing.Point(66, 76);
            this.labelTargetDistance.Name = "labelTargetDistance";
            this.labelTargetDistance.Size = new System.Drawing.Size(24, 13);
            this.labelTargetDistance.TabIndex = 206;
            this.labelTargetDistance.Text = "n/a";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(14, 76);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(49, 13);
            this.label30.TabIndex = 205;
            this.label30.Text = "Distance";
            // 
            // labelTargetElevation
            // 
            this.labelTargetElevation.AutoSize = true;
            this.labelTargetElevation.Location = new System.Drawing.Point(66, 49);
            this.labelTargetElevation.Name = "labelTargetElevation";
            this.labelTargetElevation.Size = new System.Drawing.Size(24, 13);
            this.labelTargetElevation.TabIndex = 204;
            this.labelTargetElevation.Text = "n/a";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(14, 49);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(51, 13);
            this.label18.TabIndex = 203;
            this.label18.Text = "Elevation";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(14, 23);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(44, 13);
            this.label21.TabIndex = 202;
            this.label21.Text = "Azimuth";
            // 
            // FormUDP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 353);
            this.Controls.Add(this.buttonGPSRequest);
            this.Controls.Add(this.panelTarget);
            this.Controls.Add(this.buttonControllerAsk);
            this.Controls.Add(this.panelGPSsettings);
            this.Controls.Add(this.panelRotator);
            this.Controls.Add(this.panelAD9361);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panelRotatorSettings);
            this.Controls.Add(this.comboBoxSerpNum);
            this.Controls.Add(this.checkBoxShowSettings);
            this.Controls.Add(this.panelGPS);
            this.Controls.Add(this.panelStatusFlags);
            this.Controls.Add(this.checkBoxExtControl);
            this.Controls.Add(this.panelInfo);
            this.Controls.Add(this.panelLFTChannels);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormUDP";
            this.Text = "Serp";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormUDP_FormClosing);
            this.panelLFTChannels.ResumeLayout(false);
            this.panelLFTChannels.PerformLayout();
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            this.panelStatusFlags.ResumeLayout(false);
            this.panelStatusFlags.PerformLayout();
            this.panelGPS.ResumeLayout(false);
            this.panelGPS.PerformLayout();
            this.panelRotatorSettings.ResumeLayout(false);
            this.panelRotatorSettings.PerformLayout();
            this.panelAD9361.ResumeLayout(false);
            this.panelAD9361.PerformLayout();
            this.panelRotator.ResumeLayout(false);
            this.panelRotator.PerformLayout();
            this.panelGPSsettings.ResumeLayout(false);
            this.panelGPSsettings.PerformLayout();
            this.panelTarget.ResumeLayout(false);
            this.panelTarget.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timerClock;
        private System.Windows.Forms.Label labelTimer;
        private System.Windows.Forms.CheckBox checkBoxLEDS;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Timer timerCheckInfo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelConnection;
        private System.Windows.Forms.Panel panelLFTChannels;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label labelVoltage;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.CheckBox checkBox5800M;
        private System.Windows.Forms.CheckBox checkBox2400M;
        private System.Windows.Forms.CheckBox checkBox1200M;
        private System.Windows.Forms.CheckBox checkBox900M;
        private System.Windows.Forms.CheckBox checkBoxExtControl;
        private System.Windows.Forms.Panel panelStatusFlags;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelTargetAzimuth;
        private System.Windows.Forms.Timer timerSendDataToLocator;
        private System.Windows.Forms.Label labelTemperature;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.CheckBox checkBoxTRG;
        private System.Windows.Forms.CheckBox checkBoxCORR;
        private System.Windows.Forms.CheckBox checkBoxRF;
        private System.Windows.Forms.CheckBox checkBoxRDY;
        private System.Windows.Forms.CheckBox checkBoxNewCoordinatesReceived;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelLatitude;
        private System.Windows.Forms.Label labelLongitude;
        private System.Windows.Forms.Label labelAltitude;
        private System.Windows.Forms.Panel panelGPS;
        private System.Windows.Forms.CheckBox checkBoxShowSettings;
        private System.Windows.Forms.ComboBox comboBoxSerpNum;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labelRotatorAzimuth;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label labelRotatorElevation;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button buttonRotatorStop;
        private System.Windows.Forms.TextBox textBoxElevation;
        private System.Windows.Forms.TextBox textBoxAzimuth;
        private System.Windows.Forms.Button buttonRotatorSet;
        private System.Windows.Forms.Button buttonRotatorSendCommand;
        private System.Windows.Forms.TextBox textBoxRotatorCommand;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkBoxManualGPS;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBoxAltitude;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox textBoxLongitude;
        private System.Windows.Forms.TextBox textBoxLatitude;
        private System.Windows.Forms.Button buttonApplyGPSSettings;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox textBoxElevationOffset;
        private System.Windows.Forms.TextBox textBoxAzimuthOffset;
        private System.Windows.Forms.Button buttonRefreshRotatorOffsets;
        private System.Windows.Forms.Panel panelRotatorSettings;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxRF_ATT;
        private System.Windows.Forms.TextBox textBoxRF_PSK_F;
        private System.Windows.Forms.TextBox textBoxRF_PSK_B;
        private System.Windows.Forms.TextBox textBoxRF_LFT_SWR;
        private System.Windows.Forms.TextBox textBoxRF_LFT_F2;
        private System.Windows.Forms.TextBox textBoxRF_LFT_F1;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.CheckBox checkBoxRF_PSKen;
        private System.Windows.Forms.CheckBox checkBoxRF_LFTen;
        private System.Windows.Forms.CheckBox checkBoxRFen;
        private System.Windows.Forms.Panel panelAD9361;
        private System.Windows.Forms.Panel panelRotator;
        private System.Windows.Forms.Panel panelGPSsettings;
        private System.Windows.Forms.Panel panelTarget;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label labelTargetDistance;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label labelTargetElevation;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.CheckBox checkBoxTrueAngles;
        private System.Windows.Forms.Button buttonResend;
        private System.Windows.Forms.Button buttonControllerAsk;
        private System.Windows.Forms.Button buttonGPSRequest;
        private System.Windows.Forms.Button buttonPark;
        private System.Windows.Forms.RichTextBox richTextBoxRotatorStatus;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.CheckBox checkBoxErr;
    }
}

