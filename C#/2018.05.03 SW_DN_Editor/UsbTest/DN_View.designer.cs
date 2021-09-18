namespace UsbTest
{
    partial class DN_View
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.zedGraphControlAmpl = new ZedGraph.ZedGraphControl();
            this.LoadFileButton = new System.Windows.Forms.Button();
            this.FrequencyTextBox = new System.Windows.Forms.TextBox();
            this.FreqNumTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.trackBarStartPoint = new System.Windows.Forms.TrackBar();
            this.trackBarSelectedFile = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.labelSelectedFile = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonSelectBaseAtFreqFile = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.radioButtonEditCh1 = new System.Windows.Forms.RadioButton();
            this.radioButtonEditCh2 = new System.Windows.Forms.RadioButton();
            this.radioButtonEditCh3 = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.buttonAverageAngle = new System.Windows.Forms.Button();
            this.buttonAverageFreq = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.trackBarStopPoint = new System.Windows.Forms.TrackBar();
            this.label12 = new System.Windows.Forms.Label();
            this.buttonReplace = new System.Windows.Forms.Button();
            this.buttonSet = new System.Windows.Forms.Button();
            this.textBoxAbs = new System.Windows.Forms.TextBox();
            this.buttonClear = new System.Windows.Forms.Button();
            this.textBoxFilename = new System.Windows.Forms.TextBox();
            this.checkBoxSinglePoint = new System.Windows.Forms.CheckBox();
            this.buttonAverageSelected = new System.Windows.Forms.Button();
            this.buttonSelectBaseAtAll = new System.Windows.Forms.Button();
            this.checkBoxManualAdjust = new System.Windows.Forms.CheckBox();
            this.textBoxPhi = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.radioButtonPlotPhase = new System.Windows.Forms.RadioButton();
            this.radioButtonPlotAmplitude = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarStartPoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSelectedFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarStopPoint)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // zedGraphControlAmpl
            // 
            this.zedGraphControlAmpl.Location = new System.Drawing.Point(13, 201);
            this.zedGraphControlAmpl.Name = "zedGraphControlAmpl";
            this.zedGraphControlAmpl.ScrollGrace = 0D;
            this.zedGraphControlAmpl.ScrollMaxX = 0D;
            this.zedGraphControlAmpl.ScrollMaxY = 0D;
            this.zedGraphControlAmpl.ScrollMaxY2 = 0D;
            this.zedGraphControlAmpl.ScrollMinX = 0D;
            this.zedGraphControlAmpl.ScrollMinY = 0D;
            this.zedGraphControlAmpl.ScrollMinY2 = 0D;
            this.zedGraphControlAmpl.Size = new System.Drawing.Size(860, 309);
            this.zedGraphControlAmpl.TabIndex = 31;
            // 
            // LoadFileButton
            // 
            this.LoadFileButton.Location = new System.Drawing.Point(12, 28);
            this.LoadFileButton.Name = "LoadFileButton";
            this.LoadFileButton.Size = new System.Drawing.Size(81, 73);
            this.LoadFileButton.TabIndex = 32;
            this.LoadFileButton.Text = "Load File";
            this.toolTip1.SetToolTip(this.LoadFileButton, "Load file with measured data (*.sav)");
            this.LoadFileButton.UseVisualStyleBackColor = true;
            this.LoadFileButton.Click += new System.EventHandler(this.LoadFileButton_Click);
            // 
            // FrequencyTextBox
            // 
            this.FrequencyTextBox.Enabled = false;
            this.FrequencyTextBox.Location = new System.Drawing.Point(479, 78);
            this.FrequencyTextBox.Name = "FrequencyTextBox";
            this.FrequencyTextBox.Size = new System.Drawing.Size(85, 20);
            this.FrequencyTextBox.TabIndex = 44;
            this.toolTip1.SetToolTip(this.FrequencyTextBox, "Current frequency");
            this.FrequencyTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrequencyTextBox_KeyDown);
            // 
            // FreqNumTextBox
            // 
            this.FreqNumTextBox.Enabled = false;
            this.FreqNumTextBox.Location = new System.Drawing.Point(480, 106);
            this.FreqNumTextBox.Name = "FreqNumTextBox";
            this.FreqNumTextBox.Size = new System.Drawing.Size(50, 20);
            this.FreqNumTextBox.TabIndex = 45;
            this.FreqNumTextBox.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(416, 109);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 46;
            this.label1.Text = "FreqNum";
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(377, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 47;
            this.label2.Text = "Frequency [MHz]";
            // 
            // trackBarStartPoint
            // 
            this.trackBarStartPoint.Cursor = System.Windows.Forms.Cursors.Default;
            this.trackBarStartPoint.LargeChange = 1;
            this.trackBarStartPoint.Location = new System.Drawing.Point(55, 153);
            this.trackBarStartPoint.Maximum = 0;
            this.trackBarStartPoint.Name = "trackBarStartPoint";
            this.trackBarStartPoint.Size = new System.Drawing.Size(815, 45);
            this.trackBarStartPoint.TabIndex = 56;
            this.trackBarStartPoint.Scroll += new System.EventHandler(this.trackBarStartPoint_Scroll);
            // 
            // trackBarSelectedFile
            // 
            this.trackBarSelectedFile.LargeChange = 1;
            this.trackBarSelectedFile.Location = new System.Drawing.Point(210, 22);
            this.trackBarSelectedFile.Maximum = 0;
            this.trackBarSelectedFile.Name = "trackBarSelectedFile";
            this.trackBarSelectedFile.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarSelectedFile.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.trackBarSelectedFile.Size = new System.Drawing.Size(45, 85);
            this.trackBarSelectedFile.TabIndex = 57;
            this.trackBarSelectedFile.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarSelectedFile.Scroll += new System.EventHandler(this.RayRadioButton_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(207, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 58;
            this.label4.Text = "Select file";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelSelectedFile
            // 
            this.labelSelectedFile.AutoSize = true;
            this.labelSelectedFile.Location = new System.Drawing.Point(68, 109);
            this.labelSelectedFile.Name = "labelSelectedFile";
            this.labelSelectedFile.Size = new System.Drawing.Size(56, 13);
            this.labelSelectedFile.TabIndex = 59;
            this.labelSelectedFile.Text = "# filename";
            this.labelSelectedFile.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 109);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 60;
            this.label6.Text = "Base file:";
            // 
            // buttonSelectBaseAtFreqFile
            // 
            this.buttonSelectBaseAtFreqFile.Location = new System.Drawing.Point(273, 28);
            this.buttonSelectBaseAtFreqFile.Name = "buttonSelectBaseAtFreqFile";
            this.buttonSelectBaseAtFreqFile.Size = new System.Drawing.Size(80, 73);
            this.buttonSelectBaseAtFreqFile.TabIndex = 61;
            this.buttonSelectBaseAtFreqFile.Text = "Select as base at current frequency";
            this.buttonSelectBaseAtFreqFile.UseVisualStyleBackColor = true;
            this.buttonSelectBaseAtFreqFile.Click += new System.EventHandler(this.buttonbuttonSelectBaseFile_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(477, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 13);
            this.label7.TabIndex = 62;
            this.label7.Text = "Ray1";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(534, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(32, 13);
            this.label8.TabIndex = 63;
            this.label8.Text = "Ray2";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(591, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(32, 13);
            this.label9.TabIndex = 64;
            this.label9.Text = "Ray3";
            // 
            // radioButtonEditCh1
            // 
            this.radioButtonEditCh1.AutoSize = true;
            this.radioButtonEditCh1.Checked = true;
            this.radioButtonEditCh1.Enabled = false;
            this.radioButtonEditCh1.Location = new System.Drawing.Point(10, 6);
            this.radioButtonEditCh1.Name = "radioButtonEditCh1";
            this.radioButtonEditCh1.Size = new System.Drawing.Size(14, 13);
            this.radioButtonEditCh1.TabIndex = 66;
            this.radioButtonEditCh1.TabStop = true;
            this.radioButtonEditCh1.UseVisualStyleBackColor = true;
            this.radioButtonEditCh1.CheckedChanged += new System.EventHandler(this.RayRadioButton_CheckedChanged);
            // 
            // radioButtonEditCh2
            // 
            this.radioButtonEditCh2.AutoSize = true;
            this.radioButtonEditCh2.Enabled = false;
            this.radioButtonEditCh2.Location = new System.Drawing.Point(67, 6);
            this.radioButtonEditCh2.Name = "radioButtonEditCh2";
            this.radioButtonEditCh2.Size = new System.Drawing.Size(14, 13);
            this.radioButtonEditCh2.TabIndex = 67;
            this.radioButtonEditCh2.UseVisualStyleBackColor = true;
            this.radioButtonEditCh2.CheckedChanged += new System.EventHandler(this.RayRadioButton_CheckedChanged);
            // 
            // radioButtonEditCh3
            // 
            this.radioButtonEditCh3.AutoSize = true;
            this.radioButtonEditCh3.Enabled = false;
            this.radioButtonEditCh3.Location = new System.Drawing.Point(126, 6);
            this.radioButtonEditCh3.Name = "radioButtonEditCh3";
            this.radioButtonEditCh3.Size = new System.Drawing.Size(14, 13);
            this.radioButtonEditCh3.TabIndex = 68;
            this.radioButtonEditCh3.UseVisualStyleBackColor = true;
            this.radioButtonEditCh3.CheckedChanged += new System.EventHandler(this.RayRadioButton_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(440, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 13);
            this.label3.TabIndex = 69;
            this.label3.Text = "Edit";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 131);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(98, 13);
            this.label11.TabIndex = 70;
            this.label11.Text = "Select points range";
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // buttonAverageAngle
            // 
            this.buttonAverageAngle.Location = new System.Drawing.Point(12, 505);
            this.buttonAverageAngle.MaximumSize = new System.Drawing.Size(71, 55);
            this.buttonAverageAngle.MinimumSize = new System.Drawing.Size(71, 55);
            this.buttonAverageAngle.Name = "buttonAverageAngle";
            this.buttonAverageAngle.Size = new System.Drawing.Size(71, 55);
            this.buttonAverageAngle.TabIndex = 71;
            this.buttonAverageAngle.Text = "Average by adjacent angles";
            this.buttonAverageAngle.UseVisualStyleBackColor = true;
            this.buttonAverageAngle.Click += new System.EventHandler(this.buttonAverageAngle_Click);
            // 
            // buttonAverageFreq
            // 
            this.buttonAverageFreq.Location = new System.Drawing.Point(83, 505);
            this.buttonAverageFreq.MaximumSize = new System.Drawing.Size(71, 55);
            this.buttonAverageFreq.MinimumSize = new System.Drawing.Size(71, 55);
            this.buttonAverageFreq.Name = "buttonAverageFreq";
            this.buttonAverageFreq.Size = new System.Drawing.Size(71, 55);
            this.buttonAverageFreq.TabIndex = 72;
            this.buttonAverageFreq.Text = "Average by adjacent frequencies";
            this.buttonAverageFreq.UseVisualStyleBackColor = true;
            this.buttonAverageFreq.Click += new System.EventHandler(this.buttonAverageFreq_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(659, 505);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(71, 55);
            this.buttonSave.TabIndex = 73;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 74;
            this.label5.Text = "Start";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // trackBarStopPoint
            // 
            this.trackBarStopPoint.LargeChange = 1;
            this.trackBarStopPoint.Location = new System.Drawing.Point(55, 178);
            this.trackBarStopPoint.Maximum = 0;
            this.trackBarStopPoint.Name = "trackBarStopPoint";
            this.trackBarStopPoint.Size = new System.Drawing.Size(815, 45);
            this.trackBarStopPoint.TabIndex = 75;
            this.trackBarStopPoint.Scroll += new System.EventHandler(this.trackBarStopPoint_Scroll);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 180);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(29, 13);
            this.label12.TabIndex = 76;
            this.label12.Text = "Stop";
            this.label12.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // buttonReplace
            // 
            this.buttonReplace.Location = new System.Drawing.Point(225, 505);
            this.buttonReplace.MaximumSize = new System.Drawing.Size(71, 55);
            this.buttonReplace.MinimumSize = new System.Drawing.Size(71, 55);
            this.buttonReplace.Name = "buttonReplace";
            this.buttonReplace.Size = new System.Drawing.Size(71, 55);
            this.buttonReplace.TabIndex = 77;
            this.buttonReplace.Text = "Replace from selected";
            this.buttonReplace.UseVisualStyleBackColor = true;
            this.buttonReplace.Click += new System.EventHandler(this.buttonReplace_Click);
            // 
            // buttonSet
            // 
            this.buttonSet.Location = new System.Drawing.Point(367, 505);
            this.buttonSet.MaximumSize = new System.Drawing.Size(71, 55);
            this.buttonSet.MinimumSize = new System.Drawing.Size(71, 55);
            this.buttonSet.Name = "buttonSet";
            this.buttonSet.Size = new System.Drawing.Size(71, 55);
            this.buttonSet.TabIndex = 78;
            this.buttonSet.Text = "Set value";
            this.buttonSet.UseVisualStyleBackColor = true;
            this.buttonSet.Click += new System.EventHandler(this.buttonSet_Click);
            // 
            // textBoxAbs
            // 
            this.textBoxAbs.Location = new System.Drawing.Point(484, 508);
            this.textBoxAbs.Name = "textBoxAbs";
            this.textBoxAbs.Size = new System.Drawing.Size(71, 20);
            this.textBoxAbs.TabIndex = 79;
            this.textBoxAbs.Text = "0";
            this.textBoxAbs.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumericDot_KeyPress);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(588, 505);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(71, 55);
            this.buttonClear.TabIndex = 80;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // textBoxFilename
            // 
            this.textBoxFilename.Location = new System.Drawing.Point(733, 523);
            this.textBoxFilename.Name = "textBoxFilename";
            this.textBoxFilename.Size = new System.Drawing.Size(125, 20);
            this.textBoxFilename.TabIndex = 81;
            this.textBoxFilename.Text = "filename";
            // 
            // checkBoxSinglePoint
            // 
            this.checkBoxSinglePoint.AutoSize = true;
            this.checkBoxSinglePoint.Location = new System.Drawing.Point(116, 131);
            this.checkBoxSinglePoint.Name = "checkBoxSinglePoint";
            this.checkBoxSinglePoint.Size = new System.Drawing.Size(81, 17);
            this.checkBoxSinglePoint.TabIndex = 82;
            this.checkBoxSinglePoint.Text = "Single point";
            this.checkBoxSinglePoint.UseVisualStyleBackColor = true;
            this.checkBoxSinglePoint.CheckedChanged += new System.EventHandler(this.checkBoxSinglePoint_CheckedChanged);
            // 
            // buttonAverageSelected
            // 
            this.buttonAverageSelected.Location = new System.Drawing.Point(154, 505);
            this.buttonAverageSelected.MaximumSize = new System.Drawing.Size(71, 55);
            this.buttonAverageSelected.MinimumSize = new System.Drawing.Size(71, 55);
            this.buttonAverageSelected.Name = "buttonAverageSelected";
            this.buttonAverageSelected.Size = new System.Drawing.Size(71, 55);
            this.buttonAverageSelected.TabIndex = 83;
            this.buttonAverageSelected.Text = "Average by selected";
            this.buttonAverageSelected.UseVisualStyleBackColor = true;
            this.buttonAverageSelected.Click += new System.EventHandler(this.buttonAverageSelected_Click);
            // 
            // buttonSelectBaseAtAll
            // 
            this.buttonSelectBaseAtAll.Location = new System.Drawing.Point(99, 28);
            this.buttonSelectBaseAtAll.Name = "buttonSelectBaseAtAll";
            this.buttonSelectBaseAtAll.Size = new System.Drawing.Size(80, 73);
            this.buttonSelectBaseAtAll.TabIndex = 84;
            this.buttonSelectBaseAtAll.Text = "Select as base at all frequencies";
            this.buttonSelectBaseAtAll.UseVisualStyleBackColor = true;
            this.buttonSelectBaseAtAll.Click += new System.EventHandler(this.buttonSelectBaseAtAll_Click);
            // 
            // checkBoxManualAdjust
            // 
            this.checkBoxManualAdjust.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxManualAdjust.AutoSize = true;
            this.checkBoxManualAdjust.Location = new System.Drawing.Point(296, 505);
            this.checkBoxManualAdjust.MaximumSize = new System.Drawing.Size(71, 55);
            this.checkBoxManualAdjust.MinimumSize = new System.Drawing.Size(71, 55);
            this.checkBoxManualAdjust.Name = "checkBoxManualAdjust";
            this.checkBoxManualAdjust.Size = new System.Drawing.Size(71, 55);
            this.checkBoxManualAdjust.TabIndex = 85;
            this.checkBoxManualAdjust.Text = "Manual adjust";
            this.checkBoxManualAdjust.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxManualAdjust.UseVisualStyleBackColor = true;
            this.checkBoxManualAdjust.CheckedChanged += new System.EventHandler(this.checkBoxManualAdjust_CheckedChanged);
            // 
            // textBoxPhi
            // 
            this.textBoxPhi.Location = new System.Drawing.Point(484, 536);
            this.textBoxPhi.Name = "textBoxPhi";
            this.textBoxPhi.Size = new System.Drawing.Size(71, 20);
            this.textBoxPhi.TabIndex = 86;
            this.textBoxPhi.Text = "0";
            this.textBoxPhi.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumericDot_KeyPress);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(444, 511);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(25, 13);
            this.label10.TabIndex = 87;
            this.label10.Text = "Abs";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(445, 539);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(37, 13);
            this.label13.TabIndex = 88;
            this.label13.Text = "Phase";
            this.label13.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // radioButtonPlotPhase
            // 
            this.radioButtonPlotPhase.AutoSize = true;
            this.radioButtonPlotPhase.Location = new System.Drawing.Point(66, 6);
            this.radioButtonPlotPhase.Name = "radioButtonPlotPhase";
            this.radioButtonPlotPhase.Size = new System.Drawing.Size(14, 13);
            this.radioButtonPlotPhase.TabIndex = 91;
            this.radioButtonPlotPhase.UseVisualStyleBackColor = true;
            this.radioButtonPlotPhase.CheckedChanged += new System.EventHandler(this.checkBoxPlotPhase_CheckedChanged);
            // 
            // radioButtonPlotAmplitude
            // 
            this.radioButtonPlotAmplitude.AutoSize = true;
            this.radioButtonPlotAmplitude.Checked = true;
            this.radioButtonPlotAmplitude.Location = new System.Drawing.Point(7, 6);
            this.radioButtonPlotAmplitude.Name = "radioButtonPlotAmplitude";
            this.radioButtonPlotAmplitude.Size = new System.Drawing.Size(14, 13);
            this.radioButtonPlotAmplitude.TabIndex = 90;
            this.radioButtonPlotAmplitude.TabStop = true;
            this.radioButtonPlotAmplitude.UseVisualStyleBackColor = true;
            this.radioButtonPlotAmplitude.CheckedChanged += new System.EventHandler(this.checkBoxPlotPhase_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioButtonPlotAmplitude);
            this.panel1.Controls.Add(this.radioButtonPlotPhase);
            this.panel1.Location = new System.Drawing.Point(689, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(87, 26);
            this.panel1.TabIndex = 92;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.radioButtonEditCh1);
            this.panel2.Controls.Add(this.radioButtonEditCh2);
            this.panel2.Controls.Add(this.radioButtonEditCh3);
            this.panel2.Location = new System.Drawing.Point(474, 28);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(149, 26);
            this.panel2.TabIndex = 93;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(744, 2);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(36, 26);
            this.label14.TabIndex = 95;
            this.label14.Text = "Plot\r\nphase";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(686, 2);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(52, 26);
            this.label15.TabIndex = 94;
            this.label15.Text = "Plot\r\namplitude";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DN_View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 564);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonAverageFreq);
            this.Controls.Add(this.buttonAverageAngle);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBoxPhi);
            this.Controls.Add(this.checkBoxManualAdjust);
            this.Controls.Add(this.buttonSelectBaseAtAll);
            this.Controls.Add(this.buttonAverageSelected);
            this.Controls.Add(this.checkBoxSinglePoint);
            this.Controls.Add(this.textBoxFilename);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.textBoxAbs);
            this.Controls.Add(this.buttonSet);
            this.Controls.Add(this.buttonReplace);
            this.Controls.Add(this.zedGraphControlAmpl);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.trackBarStopPoint);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.buttonSelectBaseAtFreqFile);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.labelSelectedFile);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.trackBarSelectedFile);
            this.Controls.Add(this.trackBarStartPoint);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FreqNumTextBox);
            this.Controls.Add(this.FrequencyTextBox);
            this.Controls.Add(this.LoadFileButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DN_View";
            this.Text = "Radiation pattern";
            ((System.ComponentModel.ISupportInitialize)(this.trackBarStartPoint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSelectedFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarStopPoint)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ZedGraph.ZedGraphControl zedGraphControlAmpl;
        private System.Windows.Forms.Button LoadFileButton;
        private System.Windows.Forms.TextBox FrequencyTextBox;
        private System.Windows.Forms.TextBox FreqNumTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TrackBar trackBarStartPoint;
        private System.Windows.Forms.TrackBar trackBarSelectedFile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelSelectedFile;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonSelectBaseAtFreqFile;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.RadioButton radioButtonEditCh1;
        private System.Windows.Forms.RadioButton radioButtonEditCh2;
        private System.Windows.Forms.RadioButton radioButtonEditCh3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button buttonAverageAngle;
        private System.Windows.Forms.Button buttonAverageFreq;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar trackBarStopPoint;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button buttonReplace;
        private System.Windows.Forms.Button buttonSet;
        private System.Windows.Forms.TextBox textBoxAbs;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.TextBox textBoxFilename;
        private System.Windows.Forms.CheckBox checkBoxSinglePoint;
        private System.Windows.Forms.Button buttonAverageSelected;
        private System.Windows.Forms.Button buttonSelectBaseAtAll;
        private System.Windows.Forms.CheckBox checkBoxManualAdjust;
        private System.Windows.Forms.TextBox textBoxPhi;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.RadioButton radioButtonPlotPhase;
        private System.Windows.Forms.RadioButton radioButtonPlotAmplitude;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
    }
}