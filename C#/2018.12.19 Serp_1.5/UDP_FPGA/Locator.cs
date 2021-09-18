using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace UDP_FPGA
{
    class Locator
    {
        #region DEFINE

        UdpClient udpClient;
        Thread udpThread;
        byte[] receivedBytes;
        static Queue<byte[]> toSendQueue = new Queue<byte[]>();

        static byte protocolVersion = 1;
        static int port = 7800;
        static IPAddress IP = IPAddress.Parse("192.168.0.1");
        static IPAddress IP1 = IPAddress.Parse("10.2.25.180");
        bool runLocatorUdpThread;

        public event EventHandler<TargetReceivedEventAgrs> targetingDataReceived;

        #endregion DEFINE

        #region MESSAGES

        public void DeviceState(List<Serp> devices) // 01
        {
            List<byte> byteCollection = CommandHeader(1);

            foreach (var device in devices)
            {
                //номер устройства
                byteCollection.Add(device.ID);
                // готовность
                if (device.RDY) byteCollection.Add(0x1);
                else byteCollection.Add(0x0);
                // исправность 
                if (device.CORR) byteCollection.Add(0x1);
                else byteCollection.Add(0x0);
                // признак включенного радиосигнала
                if (device.RF) byteCollection.Add(0x1);
                else byteCollection.Add(0x0);
                // признак ориентации на цель
                if (device.TRG) byteCollection.Add(0x1);
                else byteCollection.Add(0x0);
                // азимут
                byteCollection.AddRange(BitConverter.GetBytes(device.AzimuthGeo).ToArray());
                // элевация
                byteCollection.AddRange(BitConverter.GetBytes(device.ElevationGeo).ToArray());
                // широта
                byteCollection.AddRange(BitConverter.GetBytes(device.GPS.Latitude).ToArray());
                // долгота
                byteCollection.AddRange(BitConverter.GetBytes(device.GPS.Longitude).ToArray());
                // высота
                byteCollection.AddRange(BitConverter.GetBytes((float)(device.GPS.Altitude)).ToArray());
            }

            InsertCollectionLength(byteCollection);

            // контрольная сумма
            byteCollection.Add(CheckSum(byteCollection));
            // конец пакета
            byteCollection.AddRange(new byte[4]);

            toSendQueue.Enqueue(byteCollection.ToArray());
        }

        public void ConfirmTargeting(Target target) // 02
        {
            List<byte> byteCollection = CommandHeader(2);

            // номер устройства
            byteCollection.Add(target.DeviceID);
            // номер цели
            byte[] tID = BitConverter.GetBytes(target.ID);
            Array.Reverse(tID);
            byteCollection.AddRange(tID);

            InsertCollectionLength(byteCollection);

            // контрольная сумма
            byteCollection.Add(CheckSum(byteCollection));
            // конец пакета
            byteCollection.AddRange(new byte[4]);

            toSendQueue.Enqueue(byteCollection.ToArray());
        }

        public void TargetingError(Target target)  // 03
        {
            List<byte> byteCollection = CommandHeader(3);
            // номер устройства
            byteCollection.AddRange(BitConverter.GetBytes(target.DeviceID));
            // код ошибки
            byteCollection.Add(target.ErrorType);
            // сообщение об ошибке
            byteCollection.AddRange(System.Text.Encoding.UTF8.GetBytes(target.ErrorMessage));
            // ID цели
            byte[] tID = BitConverter.GetBytes(target.ID);
            Array.Reverse(tID);
            byteCollection.AddRange(tID);

            InsertCollectionLength(byteCollection);

            // контрольная сумма
            byteCollection.Add(CheckSum(byteCollection));
            // конец пакета
            byteCollection.AddRange(new byte[4]);

            toSendQueue.Enqueue(byteCollection.ToArray());
        }

        #endregion MESSAGES

        #region PARSING

        void ParsePacket()
        {
            Target target = new Target();
            if (receivedBytes.Length >= 10)
            {
                switch (receivedBytes[9])
                {
                    case 4: // Targeting
                        target = TargetingDataParse(receivedBytes);
                        bool correct = target.ReceivedCorrectly(this);
                        System.Console.WriteLine("targetingReceived");

                        if (targetingDataReceived != null) targetingDataReceived.Invoke(this, new TargetReceivedEventAgrs(target, correct));
                        break;
                    default:
                        target = new Target();
                        target.ErrorMessage = "Unknown message type: " + receivedBytes[9];
                        target.ErrorType = 1;
                        if (targetingDataReceived != null) targetingDataReceived.Invoke(this, new TargetReceivedEventAgrs(target, false));
                        break;
                }
            }
        }

        private Target TargetingDataParse(byte[] receivedBytes) // 04
        {
            Target newTarget = new Target();

            // проверка длины посылки
            if (receivedBytes.Length >= 8)
            {
                Array.Reverse(receivedBytes, 4, 4);
                UInt32 packLength = BitConverter.ToUInt32(receivedBytes, 4);
                if (receivedBytes.Length != packLength)
                {
                    newTarget.ErrorType = 1;
                    newTarget.ErrorMessage = "Wrong message length.";
                    return newTarget;
                }
            }
            else
            {
                newTarget.ErrorType = 1;
                newTarget.ErrorMessage = "Too short message length.";
                return newTarget;
            }


            // проверка версии протокола
            if (protocolVersion != receivedBytes[8])
            {
                newTarget.ErrorType = 2;
                newTarget.ErrorMessage = "Incorrect protocol version: " + receivedBytes[8].ToString() + ". Actual version is " + protocolVersion.ToString() + ".";
                return newTarget;
            }

            try
            {
                // Номер устройства
                newTarget.DeviceID = receivedBytes[10];
                // ID цели
                //byte[] targetIDArr = new byte[4] { receivedBytes[14], receivedBytes[13], receivedBytes[12], receivedBytes[11] };
                //newTarget.ID = BitConverter.ToUInt32(targetIDArr, 0);
                Array.Reverse(receivedBytes, 11, 4);
                newTarget.ID = BitConverter.ToUInt32(receivedBytes, 11);
                // Широта
                newTarget.GPS.Latitude = BitConverter.ToSingle(receivedBytes, 15);
                // Долгота
                newTarget.GPS.Longitude = BitConverter.ToSingle(receivedBytes, 19);
                // Высота
                newTarget.GPS.Altitude = (int)BitConverter.ToSingle(receivedBytes, 23);
                // СкорСев
                newTarget.SpeedNorth = BitConverter.ToSingle(receivedBytes, 27);
                // СкорВост
                newTarget.SpeedEast = BitConverter.ToSingle(receivedBytes, 31);
                // СкорВерт
                newTarget.SpeedAlt = BitConverter.ToSingle(receivedBytes, 35);
                // канал 1
                newTarget.CH1 = RFChannelDataParse(receivedBytes, 39);
                // канал 2
                newTarget.CH2 = RFChannelDataParse(receivedBytes, 57);
                // канал 3
                newTarget.CH3 = RFChannelDataParse(receivedBytes, 75);
                // канал 4
                newTarget.CH4 = RFChannelDataParse(receivedBytes, 93);
                // канал 5
                newTarget.CH5 = RFChannelDataParse(receivedBytes, 111);
                // Признак сброса
                if (receivedBytes[129] > 0) newTarget.EndFollowTarget = true; else newTarget.EndFollowTarget = false;
                // проверка контрольной суммы
                List<byte> testCheckSum = new List<byte>();
                for (int i = 0; i <= 129; i++) testCheckSum.Add(receivedBytes[i]);
                if (receivedBytes[130] != CheckSum(testCheckSum))
                {
                    newTarget.ErrorType = 1;
                    newTarget.ErrorMessage = "Invalid checksum.";
                }
            }
            catch
            {
                newTarget.ErrorType = 1;
                newTarget.ErrorMessage = "Unable to parse targeting data.";
            }

            return newTarget;
        }

        #endregion PARSING

        #region THREADING

        void StartUdpThread()
        {
            udpThread = new Thread(new ThreadStart(() =>
            {
                DateTime lastSendDateTime = DateTime.Now;
                DateTime currentDateTime = DateTime.Now;
                TimeSpan timeSpan = new TimeSpan();
                udpClient = new UdpClient(port);
                IPEndPoint ipEndPointLocator = new IPEndPoint(IP, 7800);
                //IPEndPoint ipEndPointLocator1 = new IPEndPoint(IP1, 7800);
                runLocatorUdpThread = true;
                //
                udpClient.Send(new byte[] { 1 }, 1, ipEndPointLocator);

                while (runLocatorUdpThread)
                {
                    if (udpClient.Available > 0)
                    {
                        try
                        {
                            IPEndPoint ep = new IPEndPoint(IP, port);
                            receivedBytes = udpClient.Receive(ref ep);
                            ParsePacket();
                        }
                        catch
                        { }
                    }
                    if (toSendQueue.Count != 0)
                    {
                        try
                        {
                            currentDateTime = DateTime.Now;
                            timeSpan = currentDateTime.Subtract(lastSendDateTime);
                            if (timeSpan.Milliseconds > 10)
                            {
                                byte[] sendBytes = toSendQueue.Dequeue();
                                udpClient.Send(sendBytes, sendBytes.Length, ipEndPointLocator);
                                //Thread.Sleep(5);
                                //udpClient.Send(sendBytes, sendBytes.Length, ipEndPointLocator1);
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Locator sending error");
                        }
                    }
                    Thread.Sleep(1);
                }
                udpClient.Close();
            }));
            udpThread.Name = "LocatorThread";
            udpThread.Start();
        }

        public void Stop()
        {
            runLocatorUdpThread = false;
            udpThread.Abort();
            //if (udpThread != null) udpThread.Join();
        }

        public void Start()
        {
            StartUdpThread();
        }

        #endregion THREADING

        #region ADDITIONAL

        private List<byte> CommandHeader(byte messageType)
        {
            // начало пакета
            List<byte> byteCollection = new List<byte>();
            byteCollection.Add(0xFF);
            byteCollection.Add(0xFF);
            byteCollection.Add(0xFF);
            byteCollection.Add(0xFF);
            // размер пакета
            byteCollection.AddRange(new byte[4]);
            // версия протокола
            byteCollection.Add(protocolVersion);
            // тип сообщения
            byteCollection.Add(messageType);
            return byteCollection;
        }

        public byte CheckSum(List<byte> byteCollection)
        {
            byte checkSum = 0;
            foreach (var bytE in byteCollection)
            {
                checkSum ^= bytE;
            }
            return checkSum;
        }

        private void InsertCollectionLength(List<byte> byteCollection)
        {
            byte[] collectionLength = BitConverter.GetBytes(byteCollection.Count + 5).ToArray();
            byteCollection[4] = collectionLength[3];
            byteCollection[5] = collectionLength[2];
            byteCollection[6] = collectionLength[1];
            byteCollection[7] = collectionLength[0];
        }

        static public void NetSettings(string _IP, int _port)
        {
            port = _port;
            IP = IPAddress.Parse(_IP);
        }

        static public void NetSettings(out string _IP, out int _port)
        {
            _port = port;
            _IP = IP.ToString();
        }

        private static Serp.RFchannel RFChannelDataParse(byte[] receivedBytes, int startByte)
        {
            Serp.RFchannel channelData = new Serp.RFchannel();
            // ПризнПодавление
            if (receivedBytes[startByte] > 0) channelData.RF = true; else channelData.RF = false;
            // Тип модуляции
            channelData.modulationType = receivedBytes[startByte + 1];
            // Начальная частота
            Array.Reverse(receivedBytes, startByte + 2, 8);
            channelData.startFreq = BitConverter.ToInt64(receivedBytes, startByte + 2);
            // Конечная частота
            Array.Reverse(receivedBytes, startByte + 10, 8);
            channelData.stopFreq = BitConverter.ToInt64(receivedBytes, startByte + 10);

            /*
            byte[] targetFreqArr = new byte[8]
            {
                receivedBytes[startByte + 9],
                receivedBytes[startByte + 8],
                receivedBytes[startByte + 7],
                receivedBytes[startByte + 6],
                receivedBytes[startByte + 5],
                receivedBytes[startByte + 4],
                receivedBytes[startByte + 3],
                receivedBytes[startByte + 2]
            };
            channelData.startFreq = BitConverter.ToInt64(targetFreqArr, 0);

             Конечная частота

            targetFreqArr = new byte[8]
            {
                receivedBytes[startByte + 17],
                receivedBytes[startByte + 16],
                receivedBytes[startByte + 15],
                receivedBytes[startByte + 14],
                receivedBytes[startByte + 13],
                receivedBytes[startByte + 12],
                receivedBytes[startByte + 11],
                receivedBytes[startByte + 10]
            };
            Array.Reverse(receivedBytes, startByte + 10, 8);
            channelData.stopFreq = BitConverter.ToInt64(targetFreqArr, 0);
            */

            return channelData;
        }

        public class TargetReceivedEventAgrs : EventArgs
        {
            public Target target { get; }
            public bool correct { get; }
            public TargetReceivedEventAgrs(Target newTarget, bool correct)
            {
                this.target = newTarget;
                this.correct = correct;
            }
        }

        #endregion ADDITIONAL
    }
}
