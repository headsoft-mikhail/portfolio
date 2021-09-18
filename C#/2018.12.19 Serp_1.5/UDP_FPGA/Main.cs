using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace UDP_FPGA
{
    public partial class FormUDP : Form
    {
        /////////////////////////////////////////////////////////////
        #region DEFINE
        IniFile INI = new IniFile("settings.ini");
        public static int deviceCount;
        int selectedDevice;
        Locator locator = new Locator();

        static bool freqError = false;
        static int minAtt = 0;
        static int maxAtt = 80;
        static int defaultAtt = 50;
        static int minFreq = 70;
        static int maxFreq = 6000;
        static int defaultFreq = 1500;
        static int minB = 10;
        static int maxB = 25000;
        static int defaultB = 51;
        static int minSWR = 0;
        static int maxSWR = 5000;
        static int defaultSWR = 1000;

        static bool gpsInDegrees = true;
        static bool trueRotatorAngles = false;
        static bool settingsLoaded = false;

        static TimeSpan timeCounter;
        /////////////////////////////////////////
        static SerpDataBase dataBase;
        static string dataBaseIp = "'10.2.25.201'"; //"localhost"
        ////////////////////////////////////////
        #endregion DEFINE

        /////////////////////////////////////////////////////////////
        #region INIT_FORM

        public FormUDP()
        {
            InitializeComponent();
            this.Size = new Size(Size.Width, 274);
            timeCounter = DateTime.Now.TimeOfDay;

            LoadSettingsINIstatic(INI);

            deviceCount = Serp.deviceCount;

            locator.Start();

            for (byte i = 0; i < deviceCount; i++)
            {
                new Serp();
                comboBoxSerpNum.Items.Add(i);
                Serp.serps[i].pingReceived += Serp_pingReceived;
                Serp.serps[i].pingMissed += Serp_pingMissed;
                Serp.serps[i].rotatorDataReceived += Serp_rotatorDataReceived;
                Serp.serps[i].gpsDataReceived += Serp_gpsDataReceived;
                Serp.serps[i].controllerDataReceived += Serp_controllerDataReceived;

                LoadPreSettingsINIdynamic(INI, i);
                LoadSettingsINIdynamic(INI, i);
            }
            this.Text = "Serp " + Serp.IPaddr;

            //settingsLoaded = true;
            for (int i = deviceCount - 1; i >= 0; i--)
            {
                comboBoxSerpNum.SelectedIndex = i;
                SendTSettings(i, false);
            }
            timerClock.Start();
            timerCheckInfo.Start();
            timerSendDataToLocator.Start();
        }

        #endregion INIT_FORM

        /////////////////////////////////////////////////////////////
        #region UI_LOGIC

        private void textBoxNumeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && (e.KeyChar != 8) && (e.KeyChar != 127))
                e.Handled = true;
        }

        private void textBoxNumericSign_KeyPress(object sender, KeyPressEventArgs e)
        {
            int signIndex = ((TextBox)sender).Text.IndexOf("-");
            if ((signIndex != -1) && (e.KeyChar == 45))
                e.Handled = true;

            if (!Char.IsDigit(e.KeyChar) && (e.KeyChar != 8) && (e.KeyChar != 127) && (e.KeyChar != 45))
                e.Handled = true;
        }

        private void textBoxNumericDot_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 46) e.KeyChar = ',';

            string vlCell = ((TextBox)sender).Text;

            if ((e.KeyChar == 44) && (vlCell.IndexOf(',') == -1))
                return;

            if (!Char.IsDigit(e.KeyChar) && (e.KeyChar != 8) && (e.KeyChar != 127))
                e.Handled = true;
        }

        private void checkBoxShowSettings_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShowSettings.Checked)
            {
                this.Size = new System.Drawing.Size(Size.Width, 391);
            }
            else
            {
                this.Size = new System.Drawing.Size(Size.Width, 274);
            }
        }

        private void timerClock_Tick(object sender, EventArgs e)
        {
            labelTimer.Text = (DateTime.Now.TimeOfDay - timeCounter).ToString(@"hh\:mm\:ss");
        }

        private void nonClickableCheckBoxes_Click(object sender, EventArgs e)
        {
            ((CheckBox)sender).Checked = !((CheckBox)sender).Checked;
        }

        private void FormUDP_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettingsINI();

            for (int i = 0; i < deviceCount; i++)
            {
                SendTSettings(i, true);
                Thread.Sleep(10);
                Serp.serps[i].Stop();
                Serp.serps[i].pingReceived -= Serp_pingReceived;
                Serp.serps[i].pingMissed -= Serp_pingMissed;

            }
            locator.Stop();
            /////////////////////////////////////////
            //if (dataBase != null) dataBase.Close();
            /////////////////////////////////////////
        }

        private void TextBox_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                double value = double.Parse(((TextBox)sender).Text);
                if (e.Delta > 0) value += 10;
                else if (value >= 10) value -= 10;
                ((TextBox)sender).Text = value.ToString();
            }
            catch { }
        }

        private void TextBox_MouseWheelAngles(object sender, MouseEventArgs e)
        {
            try
            {
                double value = double.Parse(((TextBox)sender).Text);
                if (e.Delta > 0) value++;
                else value--;
                ((TextBox)sender).Text = value.ToString();
            }
            catch { }
        }

        private void buttonPark_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < deviceCount; i++)
            {
                Serp.serps[i].SetCorner(0, 20, false, true);
            }
        }

        #endregion UI_LOGIC

        /////////////////////////////////////////////////////////////
        #region INI

        private void LoadSettingsINIstatic(IniFile INI)
        {
            //////////////////////// Serp
            // DevicesCount
            if (INI.KeyExistsINI("Serp", "DevicesCount"))
                Serp.deviceCount = int.Parse(INI.ReadINI("Serp", "DevicesCount"));
            else
                return;
            // NetSettings
            string subnet, myIPLastByte;
            int port, startIP;
            if ((INI.KeyExistsINI("Serp", "Subnet")) && (INI.KeyExistsINI("Serp", "MyIPLastByte")) && (INI.KeyExistsINI("Serp", "Port")) && (INI.KeyExistsINI("Serp", "StartIP")))
            {
                subnet = INI.ReadINI("Serp", "Subnet");
                myIPLastByte = INI.ReadINI("Serp", "MyIPLastByte");
                port = int.Parse(INI.ReadINI("Serp", "Port"));
                startIP = int.Parse(INI.ReadINI("Serp", "StartIP"));
                Serp.NetSettings(subnet, myIPLastByte, port, startIP);
            }

            //////////////////////// Locator
            if (INI.KeyExistsINI("Locator", "IP") && INI.KeyExistsINI("Locator", "Port"))
            {
                string _IP = INI.ReadINI("Locator", "IP");
                int _port = int.Parse(INI.ReadINI("Locator", "Port"));
                Locator.NetSettings(_IP, _port);
            }
        }

        private void LoadTextboxes()
        {
            settingsLoaded = false;
            textBoxRF_ATT.Text = Serp.serps[selectedDevice].lastTSettingsText.ATT;
            textBoxRF_PSK_F.Text = Serp.serps[selectedDevice].lastTSettingsText.F;
            textBoxRF_PSK_B.Text = Serp.serps[selectedDevice].lastTSettingsText.B;
            textBoxRF_LFT_F1.Text = Serp.serps[selectedDevice].lastTSettingsText.F1;
            textBoxRF_LFT_F2.Text = Serp.serps[selectedDevice].lastTSettingsText.F2;
            textBoxRF_LFT_SWR.Text = Serp.serps[selectedDevice].lastTSettingsText.SWR;
            checkBox900M.Checked = Serp.serps[selectedDevice].lastTSettingsText.ch900;
            checkBox1200M.Checked = Serp.serps[selectedDevice].lastTSettingsText.ch1200;
            checkBox2400M.Checked = Serp.serps[selectedDevice].lastTSettingsText.ch2400;
            checkBox5800M.Checked = Serp.serps[selectedDevice].lastTSettingsText.ch5800;
            checkBoxRF_PSKen.Checked = Serp.serps[selectedDevice].lastTSettingsText.PSKen;
            checkBoxRF_LFTen.Checked = Serp.serps[selectedDevice].lastTSettingsText.LFTen;
            checkBoxRFen.Checked = Serp.serps[selectedDevice].lastTSettingsText.RFen;
            settingsLoaded = true;
            CheckFreqSettings();
        }

        private void LoadSettingsINIdynamic(IniFile INI, int i)
        {
            Serp.TSettingsTextShell data = new Serp.TSettingsTextShell(3);

            if (!INI.KeyExistsINI("Serp", "DevicesCount")) return;
            /////////////////// Settings
            // ATT
            if (INI.KeyExistsINI("Settings" + i.ToString(), "ATT"))
                data.ATT = INI.ReadINI("Settings" + i.ToString(), "ATT");
            else
                data.ATT = defaultAtt.ToString();
            /////// PSK
            // EN
            if (INI.KeyExistsINI("Settings" + i.ToString(), "PSKen"))
                data.PSKen = bool.Parse(INI.ReadINI("Settings" + i.ToString(), "PSKen"));
            else
                data.PSKen = false;
            // F
            if (INI.KeyExistsINI("Settings" + i.ToString(), "F"))
                data.F = INI.ReadINI("Settings" + i.ToString(), "F");
            else
                data.F = defaultFreq.ToString();
            // B
            if (INI.KeyExistsINI("Settings" + i.ToString(), "B"))
                data.B = INI.ReadINI("Settings" + i.ToString(), "B");
            else
                data.B = defaultB.ToString();

            /////// LFT
            // EN
            if (INI.KeyExistsINI("Settings" + i.ToString(), "LFTen"))
                data.LFTen = bool.Parse(INI.ReadINI("Settings" + i.ToString(), "LFTen"));
            else
                data.LFTen = false;
            // F1
            if (INI.KeyExistsINI("Settings" + i.ToString(), "F1"))
                data.F1 = INI.ReadINI("Settings" + i.ToString(), "F1");
            else
                data.F1 = defaultFreq.ToString();
            // F2
            if (INI.KeyExistsINI("Settings" + i.ToString(), "F2"))
                data.F2 = INI.ReadINI("Settings" + i.ToString(), "F2");
            else
                data.F2 = defaultAtt.ToString();
            // SWR
            if (INI.KeyExistsINI("Settings" + i.ToString(), "SWR"))
                data.SWR = INI.ReadINI("Settings" + i.ToString(), "SWR");
            else
                data.SWR = defaultSWR.ToString();

            Serp.serps[i].lastTSettingsText = data;
        }

        private void LoadPreSettingsINIdynamic(IniFile INI, int i)
        {
            /////////////////// Rotator
            // AzimuthOffset
            string elevationOffset, azimuthOffset;
            if (INI.KeyExistsINI("Rotator" + i.ToString(), "AzimuthOffset"))
                azimuthOffset = INI.ReadINI("Rotator" + i.ToString(), "AzimuthOffset");
            else
                azimuthOffset = defaultFreq.ToString();
            // ElevationOffset
            if (INI.KeyExistsINI("Rotator" + i.ToString(), "ElevationOffset"))
                elevationOffset = INI.ReadINI("Rotator" + i.ToString(), "ElevationOffset");
            else
                elevationOffset = defaultAtt.ToString();

            Serp.serps[i].RefreshOffsets(float.Parse(azimuthOffset), float.Parse(elevationOffset));

            /////////////////// GPS
            // Latitude
            string latitude, longitude, altitude;
            bool manualGPS;
            if (INI.KeyExistsINI("GPS" + i.ToString(), "Latitude"))
                latitude = INI.ReadINI("GPS" + i.ToString(), "Latitude");
            else
                latitude = defaultFreq.ToString();
            // Longitude
            if (INI.KeyExistsINI("GPS" + i.ToString(), "Longitude"))
                longitude = INI.ReadINI("GPS" + i.ToString(), "Longitude");
            else
                longitude = defaultAtt.ToString();
            // Altitude
            if (INI.KeyExistsINI("GPS" + i.ToString(), "Altitude"))
                altitude = INI.ReadINI("GPS" + i.ToString(), "Altitude");
            else
                altitude = defaultAtt.ToString();
            // ManualGPS
            if (INI.KeyExistsINI("GPS" + i.ToString(), "ManualGPS"))
                manualGPS = bool.Parse(INI.ReadINI("GPS" + i.ToString(), "ManualGPS"));
            else
                manualGPS = false;

            Serp.serps[i].ApplyManualGPS(float.Parse(latitude), float.Parse(longitude), float.Parse(altitude), manualGPS);
        }

        private void SaveSettingsINI()
        {
            ///////// Serp
            // DevicesCount
            INI.WriteINI("Serp", "DevicesCount", Serp.deviceCount.ToString());
            string subnet = "", myIPLastByte = "";
            int port = 0, startIP = 0;
            Serp.NetSettings(out subnet, out myIPLastByte, out port, out startIP);
            // Subnet
            INI.WriteINI("Serp", "Subnet", subnet);
            // MyIPLastByte
            INI.WriteINI("Serp", "MyIPLastByte", myIPLastByte);
            // Port
            INI.WriteINI("Serp", "Port", port.ToString());
            // StartIP
            INI.WriteINI("Serp", "StartIP", startIP.ToString());
            ////////// Locator
            string _IP = "";
            int _port = 0;
            Locator.NetSettings(out _IP, out _port);
            // Port
            INI.WriteINI("Locator", "Port", _port.ToString());
            // IP
            INI.WriteINI("Locator", "IP", _IP);

            for (int i = 0; i < Serp.deviceCount; i++)
            {
                //////////////////// Settings
                // ATT
                INI.WriteINI("Settings" + i.ToString(), "ATT", Serp.serps[i].lastTSettingsText.ATT);
                /////// PSK
                // EN
                INI.WriteINI("Settings" + i.ToString(), "PSKen", Serp.serps[i].lastTSettingsText.PSKen.ToString());
                // F
                INI.WriteINI("Settings" + i.ToString(), "F", Serp.serps[i].lastTSettingsText.F);
                // B
                INI.WriteINI("Settings" + i.ToString(), "B", Serp.serps[i].lastTSettingsText.B);
                /////// LFT
                // EN
                INI.WriteINI("Settings" + i.ToString(), "LFTen", Serp.serps[i].lastTSettingsText.LFTen.ToString());
                // F1
                INI.WriteINI("Settings" + i.ToString(), "F1", Serp.serps[i].lastTSettingsText.F1);
                // F2
                INI.WriteINI("Settings" + i.ToString(), "F2", Serp.serps[i].lastTSettingsText.F2);
                // SWR
                INI.WriteINI("Settings" + i.ToString(), "SWR", Serp.serps[i].lastTSettingsText.SWR);

                /////////////////// Rotator
                // AzimuthOffset
                INI.WriteINI("Rotator" + i.ToString(), "AzimuthOffset", Serp.serps[i].AzimuthOffset.ToString());
                // ElevationOffset
                INI.WriteINI("Rotator" + i.ToString(), "ElevationOffset", Serp.serps[i].ElevationOffset.ToString());

                /////////////////// GPS
                // Latitude
                INI.WriteINI("GPS" + i.ToString(), "Latitude", Serp.serps[i].GPSmemory.Latitude.ToString());
                // Longitude
                INI.WriteINI("GPS" + i.ToString(), "Longitude", Serp.serps[i].GPSmemory.Longitude.ToString());
                // Altitude
                INI.WriteINI("GPS" + i.ToString(), "Altitude", Serp.serps[i].GPSmemory.Altitude.ToString());
                // ManualGPS
                INI.WriteINI("GPS" + i.ToString(), "ManualGPS", Serp.serps[i].ManualGPS.ToString());
            }
        }

        #endregion INI

        /////////////////////////////////////////////////////////////
        #region STATUS_CHECK

        private void timerCheckInfo_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < deviceCount; i++)
            {
                Serp.serps[i].StatusOKRequest();
            }
        }

        private void buttonGPSRequest_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < deviceCount; i++)
            {
                Serp.serps[i].GpsRequest();
            }
        }

        private void buttonControllerAsk_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < deviceCount; i++)
            {
                Serp.serps[i].ControllerDataRequest();
            }
        }

        private void Serp_controllerDataReceived(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler(Serp_controllerDataReceived), new object[] { sender, e });
            }
            else
            {
                byte deviceID = ((Serp)sender).ID;
                if (deviceID == selectedDevice)
                {
                    if (checkBoxRFen.Checked)
                    {
                        labelVoltage.Text = String.Format("{0:0.##}", 60.2573 * Serp.serps[deviceID].ControllerOAV / (1 << 24)) + " V";
                    }
                    else
                    {
                        labelVoltage.Text = String.Format("{0:0.##}", 35.088 * Serp.serps[deviceID].ControllerOAV / (1 << 24)) + " V";
                    }
                    labelTemperature.Text = Serp.serps[deviceID].ControllerTemperature.ToString() + " °C";
                }

            }
        }

        private void Serp_gpsDataReceived(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler(Serp_gpsDataReceived), new object[] { sender, e });
            }
            else
            {
                byte deviceID = ((Serp)sender).ID;
                if (deviceID == selectedDevice)
                {
                    if (gpsInDegrees)
                    {
                        labelLatitude.Text = Serp.serps[deviceID].GPSDecToDeg(Serp.serps[deviceID].GPS.Latitude);
                        labelLongitude.Text = Serp.serps[deviceID].GPSDecToDeg(Serp.serps[deviceID].GPS.Longitude);
                        labelAltitude.Text = Serp.serps[deviceID].GPS.Altitude.ToString() + " m";
                    }
                    else
                    {
                        labelLatitude.Text = String.Format("{0:0.######}", Serp.serps[deviceID].GPS.Latitude);
                        labelLongitude.Text = String.Format("{0:0.######}", Serp.serps[deviceID].GPS.Longitude);
                        labelAltitude.Text = Serp.serps[deviceID].GPS.Altitude.ToString() + " m";
                    }
                }
                ////////////////////////////////////////
                //try
                //{
                //    if (dataBase == null) dataBase = new SerpDataBase("server="+dataBaseIp+";user=remoteUser;database=vzor;password=qwerty;", deviceCount);
                //    dataBase.UpdateGPSData(deviceID, Serp.serps[deviceID].GPS.Latitude, Serp.serps[deviceID].GPS.Longitude);
                //}
                //catch
                //{ 
                //}
                ////////////////////////////////////////
            }
        }

        private void Serp_pingReceived(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler(Serp_pingReceived), new object[] { sender, e });
            }
            else
            {
                if (((Serp)sender).ID == selectedDevice)
                {
                    labelConnection.Text = "OK";
                    labelConnection.ForeColor = Color.DarkGreen;
                    if (!checkBoxExtControl.Checked)
                    {
                        panelAD9361.Enabled = true;
                        panelLFTChannels.Enabled = true;
                    }
                }

                if (((Serp)sender).RebootFlag == true)
                {
                    ((Serp)sender).Reboot();
                }
            }
        }

        private void Serp_pingMissed(object sender, EventArgs e)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new EventHandler(Serp_pingMissed), new object[] { sender, e });
                }
                else
                {
                    if (((Serp)sender).ID == selectedDevice)
                    {
                        labelConnection.Text = "OFF";
                        labelConnection.ForeColor = Color.DarkRed;
                        panelAD9361.Enabled = false;
                        panelLFTChannels.Enabled = false;
                    }
                }
            }
            catch
            {
                Console.WriteLine("Serp_pingMissed error");
            }
        }

        private void Serp_rotatorDataReceived(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler(Serp_rotatorDataReceived), new object[] { sender, e });
            }
            else
            {
                int deviceID = ((Serp)sender).ID;
                if (deviceID == selectedDevice)
                {
                    labelRotatorElevation.Text = String.Format("{0:0}", ((Serp)sender).ElevationGeo) + "°";
                    labelRotatorAzimuth.Text = String.Format("{0:0}", ((Serp)sender).AzimuthGeo) + "°";
                    RefreshRotatorStatus(((Serp)sender).RotatorStatus);
                }
                ///////////////////////////////////////////
                //try
                //{
                //    if (dataBase == null) dataBase = new SerpDataBase("server=" + dataBaseIp + ";user=root;database=vzor;password=;", deviceCount);
                //    dataBase.UpdateRotatorData(deviceID, Serp.serps[deviceID].AzimuthGeo);
                //}
                //catch
                //{
                //}
                //////////////////////////////////////////////
            }
        }

        #endregion STATUS_CHECK

        /////////////////////////////////////////////////////////////
        #region PROGRAM_LOGIC

        #region change_serpNum

        private void comboBoxSerpNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedDevice = comboBoxSerpNum.SelectedIndex;
            LoadTextboxes();
            if (!checkBoxExtControl.Checked)
            {
                panelAD9361.Enabled = Serp.serps[selectedDevice].ConnectionOK;
                panelLFTChannels.Enabled = Serp.serps[selectedDevice].ConnectionOK;
            }

            if (Serp.serps[selectedDevice].ConnectionOK)
            {
                labelConnection.Text = "OK";
                labelConnection.ForeColor = Color.DarkGreen;
            }
            else
            {
                labelConnection.Text = "OFF";
                labelConnection.ForeColor = Color.DarkRed;
            }

            RefreshLabels(selectedDevice);
        }

        private void RefreshLabels(int index)
        {
            //controller
            labelTemperature.Text = Serp.serps[index].ControllerTemperature.ToString() + " °C";
            if (checkBoxRFen.Checked)
            {
                labelVoltage.Text = String.Format("{0:0.##}", 60.2573 * Serp.serps[index].ControllerOAV / (1 << 24)) + " V";
            }
            else
            {
                labelVoltage.Text = String.Format("{0:0.##}", 35.088 * Serp.serps[index].ControllerOAV / (1 << 24)) + " V";
            }

            //rotator
            textBoxElevationOffset.Text = Serp.serps[index].ElevationOffset.ToString();
            textBoxAzimuthOffset.Text = Serp.serps[index].AzimuthOffset.ToString();

            RefreshRotatorTextBoxes(index);
            richTextBoxRotatorStatus.Text = Serp.serps[index].RotatorStatus;
            labelRotatorAzimuth.Text = String.Format("{0:0}", Serp.serps[index].AzimuthGeo) + "°";
            labelRotatorElevation.Text = String.Format("{0:0}", Serp.serps[index].ElevationGeo) + "°";

            //GPS
            checkBoxManualGPS.Checked = Serp.serps[index].ManualGPS;
            textBoxLatitude.Text = Serp.serps[index].GPSmemory.Latitude.ToString();
            textBoxLongitude.Text = Serp.serps[index].GPSmemory.Longitude.ToString();
            textBoxAltitude.Text = Serp.serps[index].GPSmemory.Altitude.ToString();
            RefreshGPSLabels(index);

            //targets
            if (Serp.serps[index].target != null)
            {
                panelTarget.Enabled = !Serp.serps[index].target.EndFollowTarget;
                labelTargetAzimuth.Text = String.Format("{0:0.##}", Serp.serps[index].target.Azimuth) + "°";
                labelTargetElevation.Text = String.Format("{0:0.##}", Serp.serps[index].target.Elevation) + "°";
                labelTargetDistance.Text = String.Format("{0:0}", Serp.serps[index].target.Distance) + " m";
            }
        }


        #endregion change_serpNum

        #region rotator

        private void buttonRotatorSendCommand_Click(object sender, EventArgs e)
        {
            if (textBoxRotatorCommand.Text != "")
            {
                Serp.serps[selectedDevice].SendRotatorCommand(textBoxRotatorCommand.Text);
            }
            else
            {
                Serp.serps[selectedDevice].SendRotatorCommand("Y\r");
            }
        }

        private void buttonRotatorStop_Click(object sender, EventArgs e)
        {
            Serp.serps[selectedDevice].SendRotatorCommand("S\r");
        }

        private void buttonRotatorSet_Click(object sender, EventArgs e)
        {
            Serp.serps[selectedDevice].SetCorner(float.Parse(textBoxAzimuth.Text), float.Parse(textBoxElevation.Text), trueRotatorAngles, true);
        }

        private void buttonRefreshRotatorOffsets_Click(object sender, EventArgs e)
        {
            Serp.serps[selectedDevice].RefreshOffsets(float.Parse(textBoxAzimuthOffset.Text), float.Parse(textBoxElevationOffset.Text));
            labelRotatorAzimuth.Text = String.Format("{0:0}", Serp.serps[selectedDevice].AzimuthGeo) + "°";
            labelRotatorElevation.Text = String.Format("{0:0}", Serp.serps[selectedDevice].ElevationGeo) + "°";
            RefreshRotatorTextBoxes(selectedDevice);
        }

        private void checkBoxTrueAngles_CheckedChanged(object sender, EventArgs e)
        {
            trueRotatorAngles = checkBoxTrueAngles.Checked;
            RefreshRotatorTextBoxes(selectedDevice);
        }

        private void RefreshRotatorStatus(string rotatorStatus)
        {
            //if (richTextBoxRotatorStatus.Lines.Length > 50)
            //{
            //    richTextBoxRotatorStatus.Text = "";
            //}
            rotatorStatus = rotatorStatus.Replace("\r", "");
            int n = richTextBoxRotatorStatus.Lines.Length;
            string[] statuses;
            if (n > 3)
            {
                statuses = new string[n];
                for (int i = 0; i < n - 1; i++)
                {
                    statuses[i] = richTextBoxRotatorStatus.Lines[i + 1].ToString();
                }
                statuses[n - 1] = rotatorStatus;
                richTextBoxRotatorStatus.Lines = statuses;
            }
            else
                richTextBoxRotatorStatus.AppendText("\n" + rotatorStatus);
        }

        private void RefreshRotatorTextBoxes(int i)
        {
            if (trueRotatorAngles)
            {
                textBoxAzimuth.Text = String.Format("{0:0}", Serp.serps[i].AzimuthRot);
                textBoxElevation.Text = String.Format("{0:0}", Serp.serps[i].ElevationRot);
            }
            else
            {
                textBoxAzimuth.Text = String.Format("{0:0}", Serp.serps[i].AzimuthGeo);
                textBoxElevation.Text = String.Format("{0:0}", Serp.serps[i].ElevationGeo);
            }
        }

        private static float Clamp(float angle)
        {
            if (angle >= 360)
            {
                angle -= 360;
            }
            if (angle < 0)
            {
                angle += 360;
            }
            return angle;
        }

        #endregion rotator

        #region gps

        private void buttonApplyGPSSettings_Click(object sender, EventArgs e)
        {
            Serp.serps[selectedDevice].ApplyManualGPS(float.Parse(textBoxLatitude.Text), float.Parse(textBoxLongitude.Text), float.Parse(textBoxAltitude.Text), checkBoxManualGPS.Checked);
            RefreshGPSLabels(selectedDevice);
        }

        private void panelGPS_DoubleClick(object sender, EventArgs e)
        {
            gpsInDegrees = !gpsInDegrees;
            RefreshGPSLabels(selectedDevice);
        }

        private void RefreshGPSLabels(int index)
        {
            if (gpsInDegrees)
            {
                labelLatitude.Text = Serp.serps[index].GPSDecToDeg(Serp.serps[index].GPS.Latitude);
                labelLongitude.Text = Serp.serps[index].GPSDecToDeg(Serp.serps[index].GPS.Longitude);
            }
            else
            {
                labelLatitude.Text = Serp.serps[index].GPS.Latitude.ToString();
                labelLongitude.Text = Serp.serps[index].GPS.Longitude.ToString();
            }
            labelAltitude.Text = Serp.serps[index].GPS.Altitude.ToString() + " m";
        }

        #endregion gps

        #region RFsettings_changed

        private void textBoxRF_ATT_Leave(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(textBoxRF_ATT.Text) < minAtt)
                {
                    textBoxRF_ATT.Text = minAtt.ToString();
                }
                if (int.Parse(textBoxRF_ATT.Text) > maxAtt)
                {
                    textBoxRF_ATT.Text = maxAtt.ToString();
                }
            }
            catch
            {
                textBoxRF_ATT.Text = defaultAtt.ToString();
            }
            SendTSettings(selectedDevice, false);
        }

        private void textBoxRF_PSK_B_Leave(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(textBoxRF_PSK_B.Text) < minB)
                {
                    textBoxRF_PSK_B.Text = minB.ToString();
                }
                if (int.Parse(textBoxRF_ATT.Text) > maxB)
                {
                    textBoxRF_PSK_B.Text = maxB.ToString();
                }
            }
            catch
            {
                textBoxRF_PSK_B.Text = defaultB.ToString();
            }
            SendTSettings(selectedDevice, false);
        }

        private void textBoxRF_LFT_SWR_Leave(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(textBoxRF_LFT_SWR.Text) < minSWR)
                {
                    textBoxRF_LFT_SWR.Text = minSWR.ToString();
                }
                if (int.Parse(textBoxRF_LFT_SWR.Text) > maxSWR)
                {
                    textBoxRF_LFT_SWR.Text = maxSWR.ToString();
                }
            }
            catch
            {
                textBoxRF_LFT_SWR.Text = defaultSWR.ToString();
            }
            SendTSettings(selectedDevice, false);
        }

        private void textBoxRF_F_Leave(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            try
            {
                if (textBox.Text.EndsWith(","))
                {
                    textBox.Text.Remove(textBox.Text.Length - 1);
                }
                if (double.Parse((textBox).Text) < minFreq)
                {
                    textBox.Text = minFreq.ToString();
                }
                if (double.Parse(textBox.Text) > maxFreq)
                {
                    textBox.Text = maxFreq.ToString();
                }
            }
            catch
            {
                textBox.Text = defaultFreq.ToString();
            }

            CheckFreqSettings();
        }

        private void CheckFreqSettings()
        {
            freqError = false;
            textBoxRF_PSK_F.ForeColor = Color.Black;
            textBoxRF_LFT_F1.ForeColor = Color.Black;
            textBoxRF_LFT_F2.ForeColor = Color.Black;

            double FREQ = double.Parse(textBoxRF_PSK_F.Text);
            double FREQ1 = double.Parse(textBoxRF_LFT_F1.Text);
            double FREQ2 = double.Parse(textBoxRF_LFT_F2.Text);

            if (checkBoxRF_LFTen.Checked)
            {
                if ((FREQ2 - FREQ1 > 40) || (FREQ2 < FREQ1))
                {
                    textBoxRF_LFT_F1.ForeColor = Color.DarkRed;
                    textBoxRF_LFT_F2.ForeColor = Color.DarkRed;
                    freqError = true;
                }
            }

            if ((checkBoxRF_LFTen.Checked) && (checkBoxRF_PSKen.Checked))
            {
                if (Math.Abs(FREQ1 - FREQ) > 40)
                {
                    textBoxRF_PSK_F.ForeColor = Color.DarkRed;
                    textBoxRF_LFT_F1.ForeColor = Color.DarkRed;
                    freqError = true;
                }
                if (Math.Abs(FREQ2 - FREQ) > 40)
                {
                    textBoxRF_PSK_F.ForeColor = Color.DarkRed;
                    textBoxRF_LFT_F2.ForeColor = Color.DarkRed;
                    freqError = true;
                }
            }

            checkBoxRFen.Enabled = !freqError;
            checkBoxRFen.Checked &= !freqError;
            SendTSettings(selectedDevice, false);
        }

        private void checkBoxRF_PSK_LFT_CheckedChanged(object sender, EventArgs e)
        {
            if (settingsLoaded) CheckFreqSettings();
        }

        #endregion RFsettings_changed

        #region RFsettings

        private void SendTSettings(int index, bool emptySettings)
        {
            Serp.TSettingsTextShell tSettingsText;
            if (emptySettings)
            {
                tSettingsText = new Serp.TSettingsTextShell(7);
            }
            else
            {
                tSettingsText = new Serp.TSettingsTextShell(3);
                tSettingsText.ATT = textBoxRF_ATT.Text;
                tSettingsText.F = textBoxRF_PSK_F.Text;
                tSettingsText.B = textBoxRF_PSK_B.Text;
                tSettingsText.F1 = textBoxRF_LFT_F1.Text;
                tSettingsText.F2 = textBoxRF_LFT_F2.Text;
                tSettingsText.SWR = textBoxRF_LFT_SWR.Text;
                tSettingsText.RFen = checkBoxRFen.Checked;
                tSettingsText.PSKen = checkBoxRF_PSKen.Checked;
                tSettingsText.LFTen = checkBoxRF_LFTen.Checked;
                tSettingsText.ch900 = checkBox900M.Checked;
                tSettingsText.ch1200 = checkBox1200M.Checked;
                tSettingsText.ch2400 = checkBox2400M.Checked;
                tSettingsText.ch5800 = checkBox5800M.Checked;
                tSettingsText.leds = checkBoxLEDS.Checked;
                tSettingsText.Update();
            }

            Serp.serps[index].SendTransmitterSettings(tSettingsText);
        }

        private void SendTSettings(int index, bool RFen, bool ch900, bool ch1200, bool ch2400, bool ch5800)
        {
            Serp.TSettingsTextShell tSettingsText;
            tSettingsText = new Serp.TSettingsTextShell(3);
            tSettingsText.ATT = "5";
            tSettingsText.F = "1575,42";
            tSettingsText.B = "51";
            tSettingsText.F1 = "1598";
            tSettingsText.F2 = "1609";
            tSettingsText.SWR = "1000";
            tSettingsText.RFen = RFen;
            tSettingsText.PSKen = RFen;
            tSettingsText.LFTen = RFen;
            tSettingsText.ch900 = ch900;
            tSettingsText.ch1200 = ch1200;
            tSettingsText.ch2400 = ch2400;
            tSettingsText.ch5800 = ch5800;
            tSettingsText.leds = false;
            tSettingsText.Update();
            Serp.serps[index].SendTransmitterSettings(tSettingsText);
        }

        private void tSettingsCheckBox_Click(object sender, EventArgs e)
        {
            checkBoxRFen.Checked = (checkBoxRFen.Checked && (checkBoxRF_LFTen.Checked || checkBoxRF_PSKen.Checked));
            SendTSettings(selectedDevice, false);
        }

        private void buttonResend_Click(object sender, EventArgs e)
        {
            Serp.serps[selectedDevice].lastTSettingsText = new Serp.TSettingsTextShell(7);
            SendTSettings(selectedDevice, false);
        }

        #endregion RFsettings

        #endregion PROGRAM_LOGIC

        /////////////////////////////////////////////////////////////
        #region EXTERNAL_CONTROL

        private void checkBoxExtControl_CheckedChanged(object sender, EventArgs e)
        {
            #region switch_elements_enable

            panelAD9361.Enabled = !checkBoxExtControl.Checked;
            panelLFTChannels.Enabled = !checkBoxExtControl.Checked;
            panelRotatorSettings.Enabled = !checkBoxExtControl.Checked;
            panelGPSsettings.Enabled = !checkBoxExtControl.Checked;

            #endregion switch_elements_enable

            if (checkBoxExtControl.Checked)
            {
                locator.targetingDataReceived += Locator_targetingDataReceived;
            }
            else
            {
                locator.targetingDataReceived -= Locator_targetingDataReceived;
                labelTargetAzimuth.Text = "n/a";
                labelTargetElevation.Text = "n/a";
                labelTargetDistance.Text = "n/a";
                for (int i = 0; i < deviceCount; i++)
                {
                    Serp.ClearQueueAndStop();
                    SendTSettings(i, false, false, false, false, false);
                    if (i == selectedDevice)
                    {
                        panelTarget.Enabled = false;
                        checkBox5800M.Checked = false;
                        checkBox900M.Checked = false;
                        checkBox1200M.Checked = false;
                        checkBox2400M.Checked = false;
                        checkBoxRFen.Checked = false;
                    }
                }
            }
            checkBoxShowSettings.Checked = !checkBoxExtControl.Checked;
            checkBoxShowSettings.Enabled = !checkBoxExtControl.Checked;
        }

        private void Locator_targetingDataReceived(object sender, Locator.TargetReceivedEventAgrs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler<Locator.TargetReceivedEventAgrs>(Locator_targetingDataReceived), new object[] { sender, e });
            }
            else
            {
                checkBoxNewCoordinatesReceived.Checked = !checkBoxNewCoordinatesReceived.Checked;
                checkBoxErr.Checked = !e.correct;

                if (e.correct)
                {
                    if (!e.target.EndFollowTarget)
                    {
                        //e.target.ReceivedCorrectly(locator);
                        if (e.target.DeviceID == selectedDevice)
                        {
                            panelTarget.Enabled = true;
                            checkBox5800M.Checked = e.target.CH1.RF;
                            checkBox900M.Checked = e.target.CH2.RF;
                            checkBox1200M.Checked = e.target.CH3.RF;
                            checkBox2400M.Checked = e.target.CH5.RF;
                            checkBoxRFen.Checked = e.target.CH4.RF;

                            labelTargetAzimuth.Text = String.Format("{0:0.##}", e.target.Azimuth) + "°";
                            labelTargetElevation.Text = String.Format("{0:0.##}", e.target.Elevation) + "°";
                            labelTargetDistance.Text = String.Format("{0:0}", e.target.Distance) + " m";

                        }
                        SendTSettings(e.target.DeviceID, e.target.CH4.RF, e.target.CH2.RF, e.target.CH3.RF, e.target.CH5.RF, e.target.CH1.RF);
                        System.Console.WriteLine("RF_ON serp " + e.target.DeviceID.ToString());
                        System.Console.WriteLine("433=" + e.target.CH1.RF.ToString() + "; 900=" + e.target.CH2.RF.ToString() + " 1200=" + e.target.CH3.RF.ToString() + " 1500=" + e.target.CH4.RF.ToString() + " 2400=" + e.target.CH5.RF.ToString());
                    }
                    else
                    {
                        SendTSettings(e.target.DeviceID, false, false, false, false, false);
                        System.Console.WriteLine("RF_OFF serp " + e.target.DeviceID.ToString());

                        if (e.target.DeviceID == selectedDevice)
                        {
                            panelTarget.Enabled = false;
                            checkBox5800M.Checked = false;
                            checkBox900M.Checked = false;
                            checkBox1200M.Checked = false;
                            checkBox2400M.Checked = false;
                            checkBoxRFen.Checked = false;
                        }
                        //try
                        //{
                        //    Serp.serps[e.target.DeviceID].target.followingTargetTimer.Stop();
                        //}
                        //catch
                        //{
                        //}
                        //Serp.serps[e.target.DeviceID].target = null;
                    }
                }
                this.Refresh();
            }
        }

        private void Target_targetFollowingStopped(object sender, EventArgs e)
        {
            //e.target.targetFollowingStopped += Target_targetFollowingStopped;

            if (InvokeRequired)
            {
                Invoke(new EventHandler(Target_targetFollowingStopped), new object[] { sender, e });
            }
            else
            {
                if (((Target)sender).DeviceID == selectedDevice)
                {

                }
            }
        }

        private void timerSendDataToLocator_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < Serp.serps.Count; i++)
            {
                Serp.serps[i].RDY = checkBoxExtControl.Checked;
                Serp.serps[i].CORR = Serp.serps[i].ConnectionOK && (Serp.serps[i].ControllerTemperature < 50);
                if (Serp.serps[i].target != null)
                {
                    Serp.serps[i].TRG = ((Math.Abs(Serp.serps[i].target.Azimuth - Serp.serps[i].AzimuthGeo)) < 3) && (((Math.Abs(Serp.serps[i].target.Elevation - Serp.serps[i].ElevationGeo)) < 3));
                }
                else
                {
                    Serp.serps[i].TRG = false;
                }
                Serp.serps[i].RF = Serp.serps[i].lastTSettingsText.RFen || Serp.serps[i].lastTSettingsText.RFen || Serp.serps[i].lastTSettingsText.ch900 || Serp.serps[i].lastTSettingsText.ch1200 || Serp.serps[i].lastTSettingsText.ch2400 || Serp.serps[i].lastTSettingsText.ch5800;
            }
            locator.DeviceState(Serp.serps);
            checkBoxRF.Checked = Serp.serps[selectedDevice].RF;
            checkBoxRDY.Checked = Serp.serps[selectedDevice].RDY;
            checkBoxCORR.Checked = Serp.serps[selectedDevice].CORR;
            checkBoxTRG.Checked = Serp.serps[selectedDevice].TRG;
        }

        #endregion EXTERNAL_CONTROL

        /////////////////////////////////////////////////////////////


    }


}

