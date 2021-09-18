
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;


namespace UDP_FPGA
{
    class Serp
    {
        #region DEFINE

        #region static

        static string subnet = "10.2.25.";
        static string myIPLastByte = "200";
        static byte newID = 0;
        static int startIP = 223;
        static UdpClient udpClient;
        static IPAddress myIP;
        static public string IPaddr;
        static IPEndPoint myEP;
        static IPEndPoint EP;
        static int port = 5224;
        static public int deviceCount =1;
        static Queue<PackageToSend> toSendQueue = new Queue<PackageToSend>();
        static public List<Serp> serps = new List<Serp>();
        static Thread udpThread;
        static bool runUdpThread = false;

        #endregion static

        #region dynamic

        public TSettingsTextShell lastTSettingsText = new TSettingsTextShell(7);
        byte[] sendBytes;
        public IPAddress IP;
        public IPEndPoint serpEP;
        public Target target;

        public bool ConnectionOK { get; private set; }
        public bool TSettingsConfirmed { get; private set; }
        public bool RebootFlag { get; set; }
        System.Timers.Timer pingCheckTimer;

        public event EventHandler pingReceived;
        public event EventHandler pingMissed;
        public event EventHandler controllerDataReceived;
        public event EventHandler rotatorDataReceived;
        public event EventHandler gpsDataReceived;

        #endregion dynamic

        #region properties

        public float CompassHead { get; private set; }
        public float CompassPitch { get; private set; }
        public double CompassRoll { get; private set; }

        public GPScoordinates GPS { get; private set; }
        public GPScoordinates GPSmemory { get; set; }
        public bool ManualGPS { get; set; }

        public UInt32 ControllerFreq { get; private set; }
        public UInt32 ControllerOAV { get; set; }
        public UInt32 ControllerOBV { get; set; }
        public int ControllerTemperature { get; set; }

        public float AzimuthGeo { get; private set; }
        public float ElevationGeo { get; private set; }
        public float AzimuthRot { get; private set; }
        public float ElevationRot { get; private set; }
        public string RotatorStatus { get; private set; }
        public bool RotatorStatusActual { get; private set; }
        public float AzimuthOffset { get; private set; }
        public float ElevationOffset { get; private set; }

        public byte ID { get; set; }
        public bool RDY { get; set; }
        public bool CORR { get; set; }
        public bool RF { get; set; }
        public bool TRG { get; set; }

        #endregion properties

        #region constructor

        public Serp()
        {
            if (!runUdpThread)
            {
                StartUdpThread();
            }

            ID = newID++;
            IP = IPAddress.Parse(subnet + (startIP++).ToString());
            ///////////////////////////////////////////
            //if (ID == 0)
            //{
            //    IP = IPAddress.Parse(subnet + "224");
            //}
            //if (ID == 1)
            //{
            //    IP = IPAddress.Parse(subnet + "225");
            //}
            //////////////////////////////////////////
            serpEP = new IPEndPoint(IP, port);
            target = new Target();
            RebootFlag = true;
            GPS = new GPScoordinates();
            GPSmemory = new GPScoordinates();

            RefreshOffsets(0, 0);
            pingCheckTimer = new System.Timers.Timer(5000);
            pingCheckTimer.Elapsed += PingCheckTimer_Elapsed;

            serps.Add(this);

            SendRotatorCommand("Y/r");

            SendTransmitterSettings(lastTSettingsText);
        }

        #endregion constructor

        #endregion DEFINE

        #region MESSAGES

        #region GPS

        public void GpsRequest()
        {
            sendBytes = new byte[] { 0x22 };
            toSendQueue.Enqueue(new PackageToSend(sendBytes, serpEP));
        }

        #endregion GPS

        #region ping

        public void StatusOKRequest()
        {
            sendBytes = new byte[] { 0x99 };
            toSendQueue.Enqueue(new PackageToSend(sendBytes, serpEP));
            pingCheckTimer.Start();
        }

        // по истечении таймаута происходит событие, указывающее, что связь потеряна
        private void PingCheckTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ConnectionOK = false;
            if (pingMissed != null) pingMissed.Invoke(this, null);
        }

        #endregion ping

        #region transmitter

        // отправка настроек излучения  0x06
        public bool SendTransmitterSettings(TSettingsTextShell tSettingsText)
        {
            bool settingsChanged = TSettingsTextShell.SettingsChanged(lastTSettingsText, tSettingsText);
            if (settingsChanged)
            {
                TSettings transmitterSetup = TSettingsTextShell.GetSettings(tSettingsText.settingsArray, tSettingsText.checkBoxesArray);

                List<byte> byteCollection = new List<byte>();
                byteCollection.Add(0x33);
                byteCollection.Add(0x01); // setttingsCount
                byteCollection.AddRange(new byte[6]);
                byteCollection.AddRange(BitConverter.GetBytes(transmitterSetup.adFreq));
                byteCollection.AddRange(BitConverter.GetBytes(transmitterSetup.adAttenuation));
                byteCollection.AddRange(BitConverter.GetBytes(transmitterSetup.stateTime));
                byteCollection.AddRange(BitConverter.GetBytes(transmitterSetup.ctrl));
                byteCollection.AddRange(BitConverter.GetBytes(transmitterSetup.freqa));
                byteCollection.AddRange(BitConverter.GetBytes(transmitterSetup.modw));
                byteCollection.AddRange(BitConverter.GetBytes(transmitterSetup.step));
                byteCollection.AddRange(BitConverter.GetBytes(transmitterSetup.freqb));
                byteCollection.AddRange(BitConverter.GetBytes(transmitterSetup.pskt));
                byteCollection.AddRange(BitConverter.GetBytes(transmitterSetup.fpskt));
                byteCollection.Add(transmitterSetup.level);
                byteCollection.Add(transmitterSetup.txchn);

                sendBytes = byteCollection.ToArray();
                toSendQueue.Enqueue(new PackageToSend(sendBytes, serpEP));

                lastTSettingsText = tSettingsText;
                return true;
            }
            else return false;
        }

        #endregion transmitter

        #region rotator

        public void SendRotatorCommand(string command)
        {
            //RotatorStatusActual = false;
            command = command.Replace(',', '.');
            sendBytes = CreateRotatorCommand(command);

            toSendQueue.Enqueue(new PackageToSend(sendBytes, serpEP));
        }

        public void SetCorner(float azimuth, float elevation, bool trueRotatorAngles, bool godMode)
        {
            SendRotatorCommand("Y\r");


            Thread.Sleep(5);
            if (trueRotatorAngles)
            {
                azimuth = (float)Math.Round(azimuth, 1);
                elevation = (float)Math.Round(elevation, 1);
            }
            else
            {
                azimuth = Clamp((float)Math.Round(azimuth, 1) + AzimuthOffset);
                elevation = (float)Math.Round(elevation, 1) + ElevationOffset;
            }


            if ((azimuth == 180) && (elevation == 0)&&(!godMode))
            {
                return;
            }

            string command = "Q" + (String.Format("{0:0.##}", azimuth).ToString()).Replace(",", ".") + " " + (String.Format("{0:0.##}", elevation).ToString()).Replace(",", ".") + "\r";
            System.Console.WriteLine("serp " + ID.ToString() + " " + command);
            SendRotatorCommand(command);
        }

        private static byte[] CreateRotatorCommand(string command)
        {
            List<byte> byteCollection = new List<byte>();
            byteCollection.Add(0x55);
            byteCollection.AddRange(new byte[7]);
            byteCollection.AddRange(Encoding.UTF8.GetBytes(command));
            byteCollection[1] = (byte)(byteCollection.Count - 8);
            byteCollection.AddRange(new byte[1]);
            return byteCollection.ToArray();
        }

        #endregion rotator

        #region controllerDataRequest

        public void ControllerDataRequest()
        {
            sendBytes = new byte[] { 0x44 };
            toSendQueue.Enqueue(new PackageToSend(sendBytes, serpEP));
        }

        #endregion controllerDataRequest

        #endregion MESSAGES

        #region PARSING

        void ParsePacket(byte[] receivedBytes)
        {
            if ((receivedBytes != null) && (receivedBytes.Length != 0))
            {
                switch (receivedBytes[0])
                {
                    case 0x22: // GPS
                        GPSDataParse(receivedBytes);
                        if (gpsDataReceived != null) gpsDataReceived.Invoke(this, new EventArgs());
                        break;
                    case 0x44: //controller
                        ControllerDataParse(receivedBytes);
                        if (controllerDataReceived != null) controllerDataReceived.Invoke(this, null);
                        break;
                    case 0x55: // OPU
                        RotatorDataReceived(receivedBytes);
                        if (rotatorDataReceived != null) rotatorDataReceived.Invoke(this, null);
                        break;
                    case 0x99: //status
                        PingReceived(receivedBytes);
                        if (pingReceived != null) pingReceived.Invoke(this, null);
                        break;
                }
            }
        }

        private void GPSDataParse(byte[] receivedBytes)
        {
            if (ManualGPS)
            {
                GPS = GPSmemory;
            }
            else
            {
                Array.Reverse(receivedBytes, 60, 8); GPS.Latitude = (float)(BitConverter.ToDouble(receivedBytes, 60) * 180 / Math.PI);
                Array.Reverse(receivedBytes, 68, 8); GPS.Longitude = (float)(BitConverter.ToDouble(receivedBytes, 68) * 180 / Math.PI);
                Array.Reverse(receivedBytes, 76, 8); GPS.Altitude = (int)BitConverter.ToDouble(receivedBytes, 76);
            }
        }

        private void ControllerDataParse(byte[] receivedBytes)
        {
            ControllerFreq = BitConverter.ToUInt32(receivedBytes, 1);
            ControllerOAV = BitConverter.ToUInt32(receivedBytes, 5);
            ControllerOBV = BitConverter.ToUInt32(receivedBytes, 9);
            if (receivedBytes[13] >= 128)
            {
                ControllerTemperature = Convert.ToByte(receivedBytes[13]) - 255;
            }
            else
            {
                ControllerTemperature = Convert.ToByte(receivedBytes[13]);
            }
        }

        private void RotatorDataReceived(byte[] receivedBytes)  // 0x01
        {
            RotatorStatus = Encoding.UTF8.GetString(receivedBytes, 1, receivedBytes.Length - 1);
            if ((RotatorStatus.Length >= 2) && (RotatorStatus[0] == 'O') && (RotatorStatus[1] == 'K'))
            {
                int spacePosition = RotatorStatus.LastIndexOf(" ");
                AzimuthRot = float.Parse(((RotatorStatus.Remove(spacePosition)).Remove(0, 2)).Replace(".", ","));
                ElevationRot = float.Parse(((RotatorStatus.Remove(0, spacePosition)).Remove(7)).Replace(".", ","));
                AzimuthGeo = Clamp(AzimuthRot - AzimuthOffset);
                ElevationGeo = ElevationRot - ElevationOffset;
            }
        }

        private void PingReceived(byte[] receivedBytes)  // 0x04
        {
            if ((receivedBytes.Length == 3) && (((char)receivedBytes[1]).ToString() + ((char)receivedBytes[2]).ToString() == "OK"))
            {
                pingCheckTimer.Stop();
                if (ConnectionOK == false)
                    RebootFlag = true;
                ConnectionOK = true;
            }
            if (pingReceived != null) pingReceived.Invoke(this, null);
        }

        #endregion PARSING

        #region THREADING

        static void StartUdpThread()
        {
            udpThread = new Thread(new ThreadStart(() =>
            {
                byte[] receivedBytes;
                string IP = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
                if (subnet + myIPLastByte == IP)
                    myIP = IPAddress.Parse(subnet + (myIPLastByte).ToString());
                else
                    myIP = IPAddress.Parse(IP);
                IPaddr = myIP.ToString();
                myEP = new IPEndPoint(myIP, port);
                EP = new IPEndPoint(IPAddress.Any, port);
                udpClient = new UdpClient(myEP);

                DateTime lastSendDateTime = DateTime.Now;
                DateTime currentDateTime = DateTime.Now;
                TimeSpan timeSpan = new TimeSpan();

                runUdpThread = true;

                while (runUdpThread)
                {
                    if (serps.Count != 0)
                    {
                        if (udpClient.Available > 0)
                        {
                            receivedBytes = udpClient.Receive(ref EP);
                            Serp srp = serps.Find(x =>
                            x.IP.ToString() == EP.Address.ToString());
                            if (srp != null)
                            {
                                srp.ParsePacket(receivedBytes);
                            }
                        }
                        if (toSendQueue.Count != 0) //////////////////////////////
                        {
                            try
                            {
                                currentDateTime = DateTime.Now;
                                timeSpan = currentDateTime.Subtract(lastSendDateTime);
                                if (timeSpan.Milliseconds > 10)
                                {
                                    PackageToSend package = toSendQueue.Dequeue();
                                    udpClient.Send(package.bytes, package.bytes.Length, package.ep);
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Serp sending error");
                            }
                        }
                    }


                    Thread.Sleep(1);
                }
                udpClient.Close();
            }));
            udpThread.Name = "SerpThread";
            udpThread.Start();
        }

        public void Stop()
        {
            for (int i = 0; i < serps.Count; i++)
            {
                serps[i].target.followingTargetTimer.Stop();
                System.Console.WriteLine("timer Stopped F serp " + serps[i].ID.ToString());
            }
            runUdpThread = false;
            if (udpThread != null) udpThread.Join();
        }

        #endregion THREADING

        #region ADDITIONAL

        public void Reboot()
        {
            RebootFlag = false;
            TSettingsTextShell tSettingsText = lastTSettingsText;
            lastTSettingsText = new TSettingsTextShell(7);
            SendTransmitterSettings(tSettingsText);
            SendRotatorCommand("Y\r");
        }

        static public void ClearQueueAndStop()
        {
            toSendQueue.Clear();
            for (int i = 0; i < serps.Count; i++)
            {
                serps[i].target.followingTargetTimer.Stop();
                System.Console.WriteLine("timer Stopped C serp " + serps[i].ID.ToString());
                serps[i].SendRotatorCommand("S\r");
            }
        }

        float Clamp(float angle)
        {
            while (angle >= 360)
            {
                angle -= 360;
            }
            while (angle < 0)
            {
                angle += 360;
            }
            return angle;
        }

        public void RefreshOffsets(float azimuthOffset, float elevationOffset)
        {
            AzimuthOffset = Clamp(azimuthOffset);
            ElevationOffset = elevationOffset;
            AzimuthGeo = Clamp(AzimuthRot - AzimuthOffset);
            ElevationGeo = ElevationRot - ElevationOffset;
        }

        public void ApplyManualGPS(float latitude, float longitude, float altitude, bool manualGPS)
        {
            if (manualGPS)
            {
                GPS.Latitude = latitude;
                GPS.Longitude = longitude;
                GPS.Altitude = (int)altitude;
            }
            else
            {
                GPS = new GPScoordinates();
                GpsRequest();
            }

            GPSmemory.Latitude = latitude;
            GPSmemory.Longitude = longitude;
            GPSmemory.Altitude = (int)altitude;
            ManualGPS = manualGPS;
        }

        public string GPSDecToDeg(double dec)
        {
            string deg = "";
            int part1 = (int)dec;
            deg += part1.ToString() + "° ";
            int part2 = (int)((dec - part1) * 60);
            deg += part2.ToString() + "' ";
            int part3 = (int)((dec - part1 - (double)part2 / 60) * 3600);
            deg += part3.ToString() + "\"";
            return deg;
        }

        public static void NetSettings(string _subnet, string _myIpLastByte, int _port, int _startIP)
        {
            subnet = _subnet;
            myIPLastByte = _myIpLastByte;
            port = _port;
            startIP = _startIP;
        }

        public static void NetSettings(out string _subnet, out string _myIpLastByte, out int _port, out int _startIP)
        {
            _subnet = subnet;
            _myIpLastByte = myIPLastByte;
            _port = port;
            _startIP = startIP - deviceCount;
        }

        #region RFchannelCLASS

        public struct RFchannel
        {
            public bool RF;
            public double startFreq;
            public double stopFreq;
            public byte modulationType;
        }

        #endregion RFchannelCLASS

        #region ReceiverSetup

        public struct RSetup
        {
            public UInt64 centerFreq;
            public UInt32 sampleFreq;
            public UInt32 bw;
            public Int32 gain;
        }

        #endregion ReceiverSetup

        #region TransmitterSettings

        public struct TSettings
        {
            public UInt64 adFreq;
            public UInt32 adAttenuation;
            public UInt32 stateTime;
            public UInt16 ctrl;
            public Int16 freqa;
            public UInt16 modw;
            public UInt16 step;
            public Int16 freqb;
            public UInt16 pskt;
            public UInt16 fpskt;
            public byte level;
            public byte txchn;
        }

        #endregion TransmitterSettings

        #region PackageToSend

        public class PackageToSend
        {
            public byte[] bytes;
            public IPEndPoint ep;

            public PackageToSend(byte[] bytes, IPEndPoint ep)
            {
                this.bytes = bytes;
                this.ep = ep;
            }
        }

        #endregion PackageToSend

        #region tSettingsTextShell

        public class TSettingsTextShell
        {
            public bool RFen, PSKen, LFTen;
            public bool ch900, ch1200, ch2400, ch5800;
            public bool leds;
            public string ATT, F, B, F1, F2, SWR;
            public bool[] checkBoxesArray = new bool[16];
            public double[] settingsArray = new double[7];
            int txchn;

            public TSettingsTextShell(int _txchn)
            {
                RFen = false;
                PSKen = false;
                LFTen = false;
                ch900 = false;
                ch1200 = false;
                ch2400 = false;
                ch5800 = false;

                ATT = "0";
                F = "1500";
                B = "0";
                F1 = "1500";
                F2 = "1500";
                SWR = "0";

                txchn = _txchn;

                Update();
            }

            public void Update() // put settings into arrays 
            {
                checkBoxesArray[0] = RFen;         // ResetTXNRX
                checkBoxesArray[1] = RFen;         // 28v1
                checkBoxesArray[2] = false;        // 28v2
                checkBoxesArray[3] = PSKen;
                checkBoxesArray[4] = false;        // FPSKen
                checkBoxesArray[5] = LFTen;
                checkBoxesArray[6] = ch900;
                checkBoxesArray[7] = ch1200;
                checkBoxesArray[8] = ch2400;
                checkBoxesArray[9] = ch5800;
                checkBoxesArray[10] = leds;
                checkBoxesArray[11] = leds;
                checkBoxesArray[12] = false;       // Vcotune 
                checkBoxesArray[13] = false;       // Adenable
                checkBoxesArray[14] = false;       // Adenfifo
                checkBoxesArray[15] = false;       // Reset

                settingsArray[0] = double.Parse(ATT);
                settingsArray[1] = double.Parse(F);
                settingsArray[2] = double.Parse(B);
                settingsArray[3] = double.Parse(F1);
                settingsArray[4] = double.Parse(F2);
                settingsArray[5] = double.Parse(SWR);
                settingsArray[6] = txchn;
            }

            public static TSettings GetSettings(double[] settings, bool[] checkBoxes)  // get settings 
            {
                UInt16 ctrl = CreateCTRL(checkBoxes);

                double FREQ = settings[1];
                double FREQ1 = settings[3];
                double FREQ2 = settings[4];

                Serp.TSettings tSettings;
                #region find adFreq
                UInt64 frequency;
                if ((checkBoxes[3]) && (checkBoxes[5]))
                {
                    frequency = (UInt64)((Math.Max(FREQ, FREQ2) + Math.Min(FREQ, FREQ1)) * Math.Pow(10, 6) / 2);
                }
                else
                {
                    if (checkBoxes[5])
                    {
                        frequency = (UInt64)((FREQ2 + FREQ1) * Math.Pow(10, 6) / 2);
                    }
                    else frequency = (UInt64)(FREQ * Math.Pow(10, 6));

                }
                #endregion
                tSettings.adFreq = frequency;
                tSettings.freqa = (Int16)((FREQ1 * Math.Pow(10, 6) - tSettings.adFreq) / Math.Pow(10, 4));
                tSettings.freqb = (Int16)((FREQ * Math.Pow(10, 6) - tSettings.adFreq) / Math.Pow(10, 4));
                tSettings.ctrl = ctrl;
                tSettings.adAttenuation = (UInt32)(settings[0] * 1000);
                tSettings.stateTime = (UInt32)(Convert.ToUInt32(0) * Math.Pow(10, 3) * 50); // TIME
                tSettings.modw = (UInt16)((FREQ2 - FREQ1) * 100);
                tSettings.step = (UInt16)(settings[5] * Math.Pow(2, 17) * Math.Pow(10, -6));
                tSettings.pskt = (UInt16)(settings[2]);
                tSettings.fpskt = (UInt16)(0); // B2
                tSettings.level = 127;
                int TXCHN = 3;
                tSettings.txchn = (byte)TXCHN;

                return tSettings;
            }

            public static ushort CreateCTRL(bool[] checkBoxes)  // get CTRL settings 
            {
                bool ResetTxnrx = checkBoxes[0];
                bool V281en = checkBoxes[1];
                bool V282en = checkBoxes[2];
                bool PSKen = checkBoxes[3];
                bool FPSKen = checkBoxes[4];
                bool LFTen = checkBoxes[5];
                bool v5V2EN = checkBoxes[6];
                bool v5V3EN = checkBoxes[7];
                bool v5V4EN = checkBoxes[8];
                bool v5V5EN = checkBoxes[9];
                bool Bled1 = checkBoxes[10];
                bool Bled4 = checkBoxes[11];
                bool Vcotune = checkBoxes[12];
                bool Adenable = checkBoxes[13];
                bool Adenfifo = checkBoxes[14];
                bool Reset = checkBoxes[15];

                UInt16 ctrl = 0;
                if (ResetTxnrx) ctrl += (1 << 0);
                if (LFTen) ctrl += (1 << 1);
                if (PSKen) ctrl += (1 << 2);
                if (FPSKen) ctrl += (1 << 3);
                if (Adenable) ctrl += (1 << 4);
                if (Bled4) ctrl += (1 << 5);
                if (Bled1) ctrl += (1 << 6);
                if (V281en) ctrl += (1 << 7);
                if (V282en) ctrl += (1 << 8);
                if (Adenfifo) ctrl += (1 << 9);
                if (Reset) ctrl += (1 << 10);
                if (Vcotune) ctrl += (1 << 11);
                if (v5V2EN) ctrl += (1 << 12);
                if (v5V3EN) ctrl += (1 << 13);
                if (v5V4EN) ctrl += (1 << 14);
                if (v5V5EN) ctrl += (1 << 15);
                return ctrl;
            }

            static public bool SettingsChanged(TSettingsTextShell lastTSettingsText, TSettingsTextShell nextTSettingsText)  // if settings are not similar returnes true 
            {
                lastTSettingsText.Update();
                nextTSettingsText.Update();

                bool answer = false;
                for (int i = 0; i < 16; i++)
                {
                    answer |= (nextTSettingsText.checkBoxesArray[i] != lastTSettingsText.checkBoxesArray[i]);
                }
                for (int i = 0; i < 7; i++)
                {
                    answer |= (nextTSettingsText.settingsArray[i] != lastTSettingsText.settingsArray[i]);
                }
                return answer;
            }
        }

        #endregion tSettingsTextShell

        #endregion ADIITIONAL
    }
}
